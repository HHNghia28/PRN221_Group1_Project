"use strict";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:44303/orderHub")
    .build();


connection.start().then(function () {
    console.log("connect success");
}).catch(function (err) {
    console.log("connect fail " + err);
});

connection.on("ReceiveMessage", function (message) {
    console.log("ReceiveMessage" + message);

    if (message === "Delivering") {
        window.location.reload();
    }
});



