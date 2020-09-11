function sendUpdate() {
    var obj = { action: "Subscribe" };
    socket.send(JSON.stringify(obj));
}
let socket = new WebSocket("wss://" + window.location.host + "/ws");

socket.onopen = function (e) {
    console.log("[open] Соединение установлено");
    sendUpdate();
};

socket.onmessage = function (event) {
    let tableData = "";
    data = JSON.parse(event.data);
    data.forEach(item => {
        tableData += `<tr><td>${item.Id}</td><td>${item.Name}</td><td>${item.Memory}</td></tr>`;
    })
    document.getElementById("tableBody").innerHTML = tableData;
};

socket.onclose = function (event) {
    if (event.wasClean) {
        console.log(`[close] Соединение закрыто чисто, код=${event.code} причина=${event.reason}`);
    } else {
        console.log('[close] Соединение прервано');
    }
};

socket.onerror = function (error) {
    console.log(`[error] ${error.message}`);
};
