async function processAdd() {
    var item = {
        Name: document.getElementById('addNameInput').value,
        IsBoolean: document.querySelector('input[name="isBoolean"]:checked').value,
        WarningThreshold: Number(document.getElementById('addWarningThresholdInput').value),
        AlarmThreshold: Number(document.getElementById('addAlarmThresholdInput').value),
        Priority: Number(document.getElementById('addPriorityInput').value),
        Kind: document.getElementById('addKindInput').value
    };
    var response = await fetch("http://localhost:3000/Metric/AddMetric", {
        method: 'POST',
        body: JSON.stringify(item),
        headers: {
            'Content-Type': 'application/json'
        }
    });
    if (response.ok) {
        alert("Метрика добавлена!");
        document.location.href = 'http://localhost:3000/View/Metrics'
    } else {
        if (response.status == "400") {
            let text = await response.text();
            alert(text);
        } else {
            alert("Ошибка HTTP: " + response.status);
        }
    }
};