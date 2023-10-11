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

    if (message === "Pending") {
        window.location.reload();
    }
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

            const list = document.querySelector('.order-pending');
            list.innerHTML = "";

            data.data.forEach(item => {
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
                let price_before_discount = 0;
                let price_after_discount = 0;
                let discount = 0;

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
                    discount += detail.price * (detail.salePercent / 100) * detail.quantity;
                    price_after_discount += detail.price * detail.quantity;
                });

                elementHTML += `
                                <div class="px-2 py-1">
                                    <div class="d-flex">
                                        <div class="col-md-4 text-xs font-weight-bold text-dark mb-1 p-0">
                                            Tổng giá gốc:
                                        </div>
                                        <div class="col-md-8 text-xs text-dark mb-1 p-0 text-right">
                                            ${price_after_discount}vnd
                                        </div>
                                    </div>
                                    <div class="d-flex">
                                        <div class="col-md-4 text-xs font-weight-bold text-dark mb-1 p-0">
                                            Giá giảm:
                                        </div>
                                        <div class="col-md-8 text-xs text-dark mb-1 p-0 text-right">
                                            ${discount}vnd
                                        </div>
                                    </div>
                                    <div class="d-flex">
                                        <div class="col-md-4 text-xs font-weight-bold text-dark mb-1 p-0">
                                            Thành tiền:
                                        </div>
                                        <div class="col-md-8 text-xs font-weight-bold text-dark mb-1 p-0 text-right">
                                            ${price_after_discount - discount}vnd
                                        </div>
                                    </div>
                                </div>
                                <div class="d-flex justify-content-center">
                                    <form method="post">
                                                <div class="py-1">
                                                    <input type="hidden" name="formType" value="reject">
                                                    <input type="hidden" name="orderID" value="${item.id}">
                                                    <button class="btn sm">Từ chối</button>
                                                </div>
                                            </form>

                                            <form method="post">
                                                <div class="py-1">
                                                    <input type="hidden" name="formType" value="confirm">
                                                    <input type="hidden" name="orderID" value="${item.id}">
                                                    <button class="btn sm bg-success">Xác nhận</button>
                                                </div>
                                            </form>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `;

                list.innerHTML += elementHTML;
            });
        })
        .catch(error => {
            console.error('Lỗi: ', error);
        });
}



