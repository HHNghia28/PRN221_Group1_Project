// Show hide order detail
const overplay = document.querySelector('.overplay');
const btnAdd = document.querySelector('.btn-add');
const btnUpdateList = document.querySelectorAll('.btn-table-edit');

const popUpAddForm = document.querySelector('.pop-up-form.add-form');
const popUpEditForm = document.querySelector('.pop-up-form.update-form');

const showFormAdd = (form) => {
    overplay.classList.add('d-block');
    form.classList.add('d-block');
}

const showFormUpdate = (form, itemID, itemName, itemImg, itemDescrption, itemCategory, itemDiscount, itemPrice) => {
    overplay.classList.add('d-block');
    form.classList.add('d-block');
    const formID = form.querySelector('#id-update');
    const formName = form.querySelector('#name-update');
    const formImage = form.querySelector('#image-update');
    const formDescrption = form.querySelector('#description-update');
    const formDiscount = form.querySelector('#discount-update');

    const formPrice = form.querySelector('#price-update');
    const formCategory = form.querySelector('#category-update');

    formID.innerText = itemID;
    formName.value = itemName;
    formDescrption.value = itemDescrption;
    formDiscount.value = itemDiscount;
    formImage.src = itemImg;
    formPrice.value = parseInt(itemPrice);
    formCategory.value = itemCategory;
}

const closeForm = (form) => {
    overplay.classList.remove('d-block');
    form.classList.remove('d-block');
}

btnAdd.addEventListener('click', () => {
    showFormAdd(popUpAddForm);
})

overplay.addEventListener('click', () => {
    closeForm(popUpAddForm);
    closeForm(popUpEditForm);
})

btnUpdateList.forEach((btnUpdate, index) => {
    btnUpdate.addEventListener('click', () => {
        const tableItemRow = btnUpdate.parentNode.parentNode;
        const itemID = tableItemRow.querySelector('.id').innerText;
        const itemName = tableItemRow.querySelector('.name').innerText;
        const itemImg = tableItemRow.querySelector('td img').src;
        const itemDescrption = tableItemRow.querySelector('.description').innerText;
        const itemCategory= tableItemRow.querySelector('.category').innerText;
        const itemDiscount = tableItemRow.querySelector('.discount').innerText;
        const itemPrice = tableItemRow.querySelector('.price').innerText;

        showFormUpdate(popUpEditForm, itemID, itemName, itemImg, itemDescrption, itemCategory, itemDiscount, itemPrice);
})})