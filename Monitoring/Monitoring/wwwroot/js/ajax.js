$(document).ready(function ($) {
    var highPriorityDivsArr, mediumPriorityDivsArr, lowPriorityDivsArr;
    var highFieldOutput = {}, mediumFieldOutput = {}, lowFieldOutput = {};
    var targetdiv, busyDivObj;
    var lastGlobalState = 0;
    var lastUpdateTimestamp;
    var goodSignal = new Audio('good_signal.mp3');
    var badSignal = new Audio('bad_signal_1.mp3');

    function initialize() {
        highPriorityDivsArr = $('.high').children().toArray();
        mediumPriorityDivsArr = $('.medium').children().toArray();
        lowPriorityDivsArr = $('.low').children().toArray();
    };

    function buildItem(item) {
        var output = '<div class="name">' + item.name + '</div>';
        if (item.name == "") {
            output = '<div class="name">EmptyName</div>';
        }
        if (!item.isBoolean) {
            output += '<div class="value">' + item.value + '</div>';
        }
        return output;
    };

    function processItem(item) {
        var html = buildItem(item);
        switch (item.priority) {
            case 0:
                freeDivs = highPriorityDivsArr;
                busyDivObj = highFieldOutput;
                break;
            case 1:
                freeDivs = mediumPriorityDivsArr;
                busyDivObj = mediumFieldOutput;
                break;
            case 2:
                freeDivs = lowPriorityDivsArr;
                busyDivObj = lowFieldOutput;
                break;
        };
        var targetdiv;
        if (busyDivObj.hasOwnProperty(item.kind)) {
            targetdiv = busyDivObj[item.kind];
        }
        else if (freeDivs.length) {
            targetdiv = $(freeDivs[0]);
            freeDivs.splice(0, 1);
            busyDivObj[item.kind] = targetdiv;
        };

        if (!targetdiv) {
            return 0;
        }
        item.isBoolean && targetdiv.addClass('is-boolean');
        targetdiv.html(html);
        return processItemThresholds(item, targetdiv);
    };

    // returns: 0 - ok, 1 - warning, 2 - alarm
    function processItemThresholds(item, targetDiv) {
        if ((!item.isBoolean && item.alarmThreshold > 0 && item.value >= item.alarmThreshold) || (item.isBoolean && item.value > 0)) {
            return setItemAlarm(targetDiv);
        } else if (item.warningThreshold > 0 && item.value >= item.warningThreshold && !item.isBoolean) {
            return setItemWarning(targetDiv);
        } else {
            return setItemOk(targetDiv);
        }
    };

    function setItemOk(targetDiv) {
        targetDiv.addClass('ok').removeClass('warning').removeClass('alarm');
        return 0;
    };

    function setItemWarning(targetDiv) {
        targetDiv.removeClass('ok').addClass('warning').removeClass('alarm');
        return 1;
    };

    function setItemAlarm(targetDiv) {
        targetDiv.removeClass('ok').removeClass('warning').addClass('alarm');
        return 2;
    };

    function onGlobalStateChanged(lastGlobalState, newGlobalState) {
        var now = new Date();
        if (now.getHours() < 8 || now.getHours() > 19 || now.getDay() == 0 || now.getDay() == 6) { // в нерабочее время звук не играем
            return;
        }
        if (newGlobalState == 2 && lastGlobalState < 2) {
            badSignal.play();
        } else if (newGlobalState <= 1 && lastGlobalState > 1) {
            goodSignal.play();
        }
    };

    function sleep(ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }

    async function requestRequiredData(data) {
        var response = await fetch("http://localhost:3000/api/rabbit", {
            method: 'POST',
            body: JSON.stringify(data),
            headers: {
                'Content-Type': 'application/json'
            }
        });
        if (response.ok) {
            console.log("Запрос за данными отправлен!");
        } else {
            console.log("Ошибка HTTP: " + response.status);
        }
    }

    function createDataToRabbitMQ() {
         $.ajax({
            type: 'GET',
            url: 'http://localhost:3000/Metric/CreateMetricRequiredNotification/',
            dataType: 'json',
            timeout: 30000,
            success: function (data) {
                requestRequiredData(data);      
                setTimeout(processJson, 5000);
                setTimeout(createDataToRabbitMQ, 30000);
            },
            error: function () {
                console.log("Произошла ошибка CreateMetricRequiredNotification");
                setTimeout(createDataToRabbitMQ, 30000);
            }
        });
    }

    function processJson() {
        $.ajax({
            type: 'GET',
            url: 'http://localhost:3000/Testing/AcceptRequest',
            dataType: 'json',
            success: function (data) {
                var globalState = 0;
                $.each(data.metrics, function (index, item) {
                    var itemState = processItem(item);
                    if (itemState > globalState) {
                        globalState = itemState;
                    }
                });
                onGlobalStateChanged(lastGlobalState, globalState);
                lastUpdateTimestamp = new Date();
                $('#footer').text('Данные от ' + lastUpdateTimestamp.toLocaleString()).removeClass('error');
                lastGlobalState = globalState;
            },
            error: function () {
                var errorText = 'Ошибка обновления данных';
                if (lastUpdateTimestamp) {
                    errorText += ', последние данные от ' + lastUpdateTimestamp.toLocaleString();
                }
                $('#footer').text(errorText).addClass('error');
            }
        });
    };
    initialize();
    createDataToRabbitMQ()
})