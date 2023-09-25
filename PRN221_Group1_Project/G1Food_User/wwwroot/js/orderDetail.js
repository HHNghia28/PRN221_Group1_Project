// Show hide order detail
const overplay = document.querySelector('.overplay');
const orderedList = document.querySelectorAll('.order-list tr');
const orderDetailContainer = document.querySelector('.oderDetail-container');
const btnCloseOrderDetail = document.querySelector('.btn-close-order-detail');
orderedList.forEach((order, index) => {
    order.addEventListener('click', () => {
        overplay.classList.add('d-block');
        orderDetailContainer.classList.add('d-block');
    })
})

btnCloseOrderDetail.addEventListener('click', () => {
    overplay.classList.remove('d-block');
    orderDetailContainer.classList.remove('d-block');
})