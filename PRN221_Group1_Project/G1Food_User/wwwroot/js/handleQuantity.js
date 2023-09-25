const increaseQuantity = (button) => {
    const parentlEement = button.parentNode;
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