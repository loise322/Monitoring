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

async function requestDataFromRabbitMQ() {
    await $.ajax({
        type: 'GET',
        url: 'http://localhost:3000/Metric/CreateMetricRequiredNotification/',
        dataType: 'json',
        timeout: 30000,
        success: async function (data) {
            await requestRequiredData(data);
            setTimeout(requestDataFromRabbitMQ(), 30000);
        },
        error: function () {
            console.log("Произошла ошибка CreateMetricRequiredNotification");
            setTimeout(requestDataFromRabbitMQ(), 30000);
        }
    });
}