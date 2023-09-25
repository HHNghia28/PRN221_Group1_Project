// Set active navbar tab
const navItems = document.querySelectorAll('.header-list li');
navItems.forEach(item => {
    item.addEventListener('click', () => {
        const currentActiveTab = document.querySelector('.header-list li.active');
        console.log(currentActiveTab);
        currentActiveTab.classList.remove('active');
        item.classList.add('active');
    })
})

// Show hide menu 
const overplay = document.querySelector('.overplay');
const orderedList = document.querySelectorAll('.order-list tr');
const orderDetailContainer = document.querySelector('.oderDetail-container');
const btnCloseOrderDetail = document.querySelector('.btn-close-order-detail');
console.log(overplay);
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

// Handle inscrease quantity in product detail
const increaseQuantity = (button)  => {
    const parentlEement = button.parentNode ;
    const quantityInput = parentlEement.querySelector('#quantity-input');
    console.log(quantityInput);
    let quantity = parseInt(quantityInput.value, 10);
    quantityInput.value = quantity + 1;
  }
  
const decreaseQuantity = (button) => {
    const parentlEement = button.parentNode;
    const quantityInput = parentlEement.querySelector('#quantity-input');
    let quantity = parseInt(quantityInput.value, 10);
    if (quantity > 1) {
      quantityInput.value = quantity - 1;
    }
}
//   Handle show form input comment
// const inputComment = document.querySelector('.comment-wrapper.reply input');
// const buttonReplyComment = document.querySelector('.comment-wrapper.reply button');
// console.log(inputComment);
// console.log(buttonReplyComment)
// buttonReplyComment.addEventListener('click', () => {
//     inputComment.style.display = 'block';
// })
