﻿
async function processEdit() {
    var item = {
        id: Number(document.getElementById('addIdInput').value),
        Name: document.getElementById('editNameInput').value,
        IsBoolean: document.getElementById('editIsBooleanInput').value,
        WarningThreshold: Number(document.getElementById('editWarningThresholdInput').value),
        AlarmThreshold: Number(document.getElementById('editAlarmThresholdInput').value),
        Priority: Number(document.getElementById('editPriorityInput').value),
        Kind: document.getElementById('editKindInput').value
    };
    var response = await fetch("http://localhost:3000/Home/EditMetric", {
        method: 'POST',
        body: JSON.stringify(item),
        headers: {
           'Content-Type': 'application/json'
        }
    });
    if (response.ok) {
        alert("Changes saved!");
        document.location.href = 'http://localhost:3000/Home/Metrics'
    } else {
        alert("Ошибка HTTP: " + response.status);
    }
};