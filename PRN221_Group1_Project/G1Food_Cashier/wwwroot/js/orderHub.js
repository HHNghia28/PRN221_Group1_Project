"use strict";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:44303/orderHub")
    .build();


connection.start().then(function () {
    console.log("connect success");
    callApi();
}).catch(function (err) {
    console.log("connect fail " + err);
});

connection.on("ReceiveMessage", function (message) {
    console.log("ReceiveMessage" + message);
});

function callApi() {
    const apiUrl = "https://localhost:44303/api/Order/getOrderPending";

    fetch(apiUrl)
        .then(response => {
            if (response.status === 200) {
                return response.json();
            } else {
                throw new Error('Lỗi khi gọi API');
            }
        })
        .then(data => {
            console.log("Dữ liệu từ API:", data);
            console.dir(data);
        })
        .catch(error => {
            console.error('Lỗi: ', error);
        });
}