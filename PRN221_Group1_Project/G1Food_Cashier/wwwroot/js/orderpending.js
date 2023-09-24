"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/orderHub").build();

connection.on("ReceiveMessage", function (aPIResponseDTO) {
    const list = document.querySelector('.order-pending');
    list.innerHTML = "";

    aPIResponseDTO.data.forEach(item => {
        let elementHTML = `
            <div class="col-xl-4 col-md-12 mb-4">
                <div class="card border-left-main shadow h-100 py-2">
                    <div class="card-body-cashier">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="px-2 py-1">
                                    <div class="d-flex">
                                        <div class="col-md-2 text-xs font-weight-bold text-dark mb-1 p-0">
                                            ID:
                                        </div>
                                        <div class="col-md-10 text-xs text-dark p-0">
                                            ${item.id}
                                        </div>
                                    </div>
                                    <div class="d-flex">
                                        <div class="col-md-2 text-xs font-weight-bold text-dark mb-1 p-0">
                                            Tên:
                                        </div>
                                        <div class="col-md-10 text-xs text-dark mb-1 p-0">
                                            ${item.username}
                                        </div>
                                    </div>
                                    <div class="d-flex">
                                        <div class="col-md-3 text-xs font-weight-bold text-dark mb-1 p-0">
                                            Ghi chú:
                                        </div>
                                        <div class="col-md-9 text-xs text-dark mb-1 p-0">
                                            ${item.note}
                                        </div>
                                    </div>
                                    <div class="d-flex">
                                        <div class="col-md-4 text-xs font-weight-bold text-dark mb-1 p-0">
                                            Mã giảm giá:
                                        </div>
                                        <div class="col-md-8 text-xs text-dark mb-1 p-0">
                                            ${item.salePercent}
                                        </div>
                                    </div>
                                </div>
        `;

        let count = 1;

        item.details.forEach(detail => {
            elementHTML += `
                ${count % 2 === 0 ? `
                    <div class="border-radius-1">
                                    <div class="px-2 py-1">
                                        <div class="d-flex">
                                            <div class="col-md-10 text-xs font-weight-bold text-dark mb-1 p-0">
                                                ${detail.productName}
                                            </div>
                                            <div class="col-md-2 text-xs font-weight-bold text-dark mb-1 p-0 text-right">
                                                ${detail.quantity}
                                            </div>
                                        </div>
                                        <div class="d-flex">
                                            <div class="col-md-3 text-xs text-dark mt-note mb-1 p-0">
                                                Ghi chú:
                                            </div>
                                            <div class="col-md-9 text-xs text-dark font-italic mt-note mb-1 p-0">
                                                ${detail.note}
                                            </div>
                                        </div>
                                    </div>
                                </div>
                ` : `
                    <div class="background-primary border-radius-1">
                                    <div class="px-2 py-1">
                                        <div class="d-flex">
                                            <div class="col-md-10 text-xs font-weight-bold text-dark mb-1 p-0">
                                                ${detail.productName}
                                            </div>
                                            <div class="col-md-2 text-xs font-weight-bold text-dark mb-1 p-0 text-right">
                                                ${detail.quantity}
                                            </div>
                                        </div>
                                        <div class="d-flex">
                                            <div class="col-md-3 text-xs text-dark mt-note mb-1 p-0">
                                                Ghi chú:
                                            </div>
                                            <div class="col-md-9 text-xs text-dark font-italic mt-note mb-1 p-0">
                                                ${detail.note}
                                            </div>
                                        </div>
                                    </div>
                                </div>
                `}
            `;
            count++;
        });

        elementHTML += `
                                <div class="px-2 py-1">
                                    <div class="d-flex">
                                        <div class="col-md-4 text-xs font-weight-bold text-dark mb-1 p-0">
                                            Tổng giá gốc:
                                        </div>
                                        <div class="col-md-8 text-xs text-dark mb-1 p-0 text-right">
                                            100.000vnd
                                        </div>
                                    </div>
                                    <div class="d-flex">
                                        <div class="col-md-4 text-xs font-weight-bold text-dark mb-1 p-0">
                                            Giá giảm:
                                        </div>
                                        <div class="col-md-8 text-xs text-dark mb-1 p-0 text-right">
                                            99.000vnd
                                        </div>
                                    </div>
                                    <div class="d-flex">
                                        <div class="col-md-4 text-xs font-weight-bold text-dark mb-1 p-0">
                                            Thành tiền:
                                        </div>
                                        <div class="col-md-8 text-xs font-weight-bold text-dark mb-1 p-0 text-right">
                                            1.000vnd
                                        </div>
                                    </div>
                                </div>
                                <div class="d-flex justify-content-center">
                                    <div class="py-1 pr-3">
                                        <button class="btn sm">Từ chối</button>
                                    </div>
                                    <div class="py-1 pl-3">
                                        <button class="btn sm bg-success">Xác nhận</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `;

        list.innerHTML += elementHTML;
    });

    console.dir(aPIResponseDTO);
});

document.addEventListener("DOMContentLoaded", function () {
    // Initialize the SignalR connection
    connection.start().then(function () {
        alert('Connected to orderHub');
    }).catch(function (err) {
        console.error(err.toString());
    });
});
