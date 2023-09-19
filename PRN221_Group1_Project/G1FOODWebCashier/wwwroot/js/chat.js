"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/orderHub").build();

connection.on("ReceiveMessage", function (aPIResponseDTO) {
    const list = document.querySelector('.data-container');
    list.innerHTML = "";

    aPIResponseDTO.data.map(item => {
        const elemnet = document.createElement('div');
        elemnet.innerHTML = `
            <div class="product" style="width: 30%; margin: 10px; float: left;">
                <h2 style="font-size: 12px;">ID: ${item.id}</h2>
                <h2 style="font-size: 12px;">Username: ${item.username}</h2>
                <h2 style="font-size: 12px;">Status: ${item.status}</h2>
                <h2 style="font-size: 12px;">Note: ${item.note}</h2>
                <h2 style="font-size: 12px;">Date: ${item.date}</h2>
            </div>
        `;
        list.appendChild(elemnet);
    })
    
    console.dir(aPIResponseDTO)
});

document.addEventListener("DOMContentLoaded", function () {
    // Initialize the SignalR connection
    connection.start().then(function () {
        //alert('Connected to orderHub');
    }).catch(function (err) {
        console.error(err.toString());
    });
});