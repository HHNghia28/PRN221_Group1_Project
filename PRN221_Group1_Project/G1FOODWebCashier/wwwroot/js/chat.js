"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/orderHub").build();

connection.on("ReceiveMessage", function (aPIResponseDTO) {
    var li = document.createElement("li");
    
    console.dir(aPIResponseDTO)
});

document.addEventListener("DOMContentLoaded", function () {
    // Initialize the SignalR connection
    connection.start().then(function () {
        alert('Connected to orderHub');
    }).catch(function (err) {
        console.error(err.toString());
    });
});