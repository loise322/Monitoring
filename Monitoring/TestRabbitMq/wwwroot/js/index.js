async function createAndSendMessage() {
    let count = document.getElementById('countOfData').value;
    var response = await fetch("http://localhost:5000/Home/GetTestData?count=" + count, {
        method: 'GET'
    });
    if (response.ok) {
        var data = await response.json();
        renderTable(data);
        sendMessage(data);
    } else {
        alert("Ошибка HTTP: " + response.status);
    }
}

function renderTable(data) {
    var tableBody = document.getElementById('tableBody');
    tableBody.innerHTML = '';
    $.each(data, function (index, item) {
        var tr = document.createElement('div');
        tr.classList.add('table-body-tr');
        appendTd(item.name, tr);
        appendTd(item.isBoolean, tr);
        appendTd(item.warningThreshold, tr);
        appendTd(item.alarmThreshold, tr);
        appendTd(item.priority, tr);
        appendTd(item.kind, tr);
        appendTd(item.value, tr);
        tableBody.appendChild(tr);
    });
    
}

function appendTd(data, tr) {
    var td = document.createElement('div');
    td.classList.add('col-my', 'table-col');
    td.appendChild(document.createTextNode(data || "0"));
    tr.appendChild(td);
}

async function sendMessage(item) {

    var response = await fetch("http://localhost:5000/api/rabbit", {
        method: 'POST',
        body: JSON.stringify(item),
        headers: {
            'Content-Type': 'application/json'
        }
    });
    if (response.ok) {
        alert("Данные успешно отправлены!");
    } else {
        alert("Ошибка HTTP: " + response.status);
    }
}
