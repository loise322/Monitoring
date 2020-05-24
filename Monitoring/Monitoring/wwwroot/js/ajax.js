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

    function processData(item) {
        fetch("http://localhost:3000/Home/ProcessData", {
            method: 'POST',
            body: JSON.stringify(item),
            headers: {
                'Content-Type': 'application/json'
            }
        });
    };

    function processJson() {
        $.ajax({
            type: 'GET',
            url: 'AcceptRequest',
            dataType: 'json',
            timeout: 5000,
            success: function (data) {
                var globalState = 0;
                $.each(data.metrics, function (index, item) {
                    var itemState = processItem(item);
                    if (itemState > globalState) {
                        globalState = itemState;
                    }
                });
                processData(data);
                onGlobalStateChanged(lastGlobalState, globalState);
                lastUpdateTimestamp = new Date();
                $('#footer').text('Данные от ' + lastUpdateTimestamp.toLocaleString()).removeClass('error');
                lastGlobalState = globalState;
                setTimeout(processJson, 5000);
            },
            error: function () {
                var errorText = 'Ошибка обновления данных';
                if (lastUpdateTimestamp) {
                    errorText += ', последние данные от ' + lastUpdateTimestamp.toLocaleString();
                }
                $('#footer').text(errorText).addClass('error');
                setTimeout(processJson, 5000);
            }
        });
    };
    initialize();
    processJson();
})