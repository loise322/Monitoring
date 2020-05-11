async function processAdd() {
    var item = {
        Name: document.getElementById('addNameInput').value,
        IsBoolean: document.getElementById('addIsBooleanInput').value,
        WarningThreshold: Number(document.getElementById('addWarningThresholdInput').value),
        AlarmThreshold: Number(document.getElementById('addAlarmThresholdInput').value),
        Priority: Number(document.getElementById('addPriorityInput').value),
        Kind: document.getElementById('addKindInput').value
    };
    var response = await fetch("http://localhost:3000/Home/AddMetric", {
        method: 'POST',
        body: JSON.stringify(item),
        headers: {
            'Content-Type': 'application/json'
        }
    });
    if (response.ok) {
        alert("Metric added!");
        document.location.href = 'http://localhost:3000/Home/Metrics'
    } else {
        if (response.status == "400") {
            let json = await response.json();
            let text = "Ошибка валидации!\r\n";
            for (let i = 0; i < json.length; i++) {
                text = text + json[i] + "\r\n";
            }
            alert(text);
        } else {
            if (response.status == "500") {
                let text = await response.text();
                alert(text);
            } else {
                alert("Ошибка HTTP: " + response.status);
            }
        }
    }
};