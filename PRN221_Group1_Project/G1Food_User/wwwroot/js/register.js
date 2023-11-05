

const fullName = document.getElementById("fullName");
const email = document.getElementById("email");
const phone = document.getElementById("phone");
const password = document.getElementById("password");
const passwordConfirm = document.getElementById("password-confirm");
const address = document.getElementById("address");
const formConfirm = document.getElementById("form-confirm");
const formRegister = document.getElementById("register-form");


function showError(input, message) {
    let parentTag = input.parentElement;
    let errorMessage = parentTag.querySelector(".form-error");
    errorMessage.innerText = message;
    input.style.borderColor = "var(--main-color)";
}

function showSuccess(input) {
    let parentTag = input.parentElement;
    let errorMessage = parentTag.querySelector(".form-error");
    errorMessage.innerText = "";
    input.style.borderColor = "var(--success)";
}

const checkEmpty = function (listInput) {
    let isEmpty = false;
    listInput.forEach((item) => {
        if (!item.value.trim()) {
            showError(item, "Input can not be empty!");
            isEmpty = true;
        } else {
            showSuccess(item);
        }
    });
    return isEmpty;
};

const checkEmail = function (input) {
    const emailPattern =
        /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (emailPattern.test(input.value.trim())) {
        showSuccess(input);
        return true;
    } else {
        showError(input, 'Email không hợp lệ!')
        // input.focus();
        return false;
    }
}

const checkPassword = function (input) {
    //const passwordPattern = /^[A-Za-z@]\w{4,14}$/;
    if (input.value.trim().length >= 6) {
        showSuccess(input);
        return true;
    } else {
        showError(input, 'Mật khẩu không hợp lệ!')
        // input.focus();
        return false;
    }
}

const checkPasswordConfirm = function (password, passwordConfirm) {
    const passwordPattern = /^[A-Za-z]\w{7,14}$/;
    const passValue = password.value.trim();
    const confirmPassValue = passwordConfirm.value.trim();

    if (passValue === confirmPassValue) {
        showSuccess(passwordConfirm);
        return true;
    } else {
        showError(passwordConfirm, 'Mật khẩu nhập lại sai!')
        // input.focus();
        return false;
    }
}


const checkPhone = function (input) {
    const passwordPattern = /^(((\+|)84)|0)(3|5|7|8|9)+([0-9]{8})\b$/;
    if (passwordPattern.test(input.value.trim())) {
        showSuccess(input);
        return true;
    } else {
        showError(input, 'Số điện thoại không hợp lệ!')
        // input.focus();
        return false;
    }
}

const checkAddress = function (input) {
    if (input.value.trim() !== '') {
        showSuccess(input);
        return true;
    } else {
        showError(input, 'Địa chỉ không hợp lệ!')
        // input.focus();
        return false;
    }
}

const checkFullName = function (input) {
    if (input.value.trim() !== '') {
        showSuccess(input);
        return true;
    } else {
        showError(input, 'Tên không hợp lệ!')
        // input.focus();
        return false;
    }
}

const checkFormConfirm = function (input) {
    console.log(input.checked);
    if (input.checked) {
        showSuccess(input);
        return true;
    } else {
        showError(input, 'Vui lòng xác nhận thông tin!')
        // input.focus();
        return false;
    }
}

email.addEventListener('blur', (e) => {
    checkEmail(email);
})
password.addEventListener('blur', (e) => {
    checkPassword(password);
})

fullName.addEventListener('blur', (e) => {
    checkFullName(fullName);
})
passwordConfirm.addEventListener('blur', (e) => {
    checkPasswordConfirm(password,passwordConfirm);
})

phone.addEventListener('blur', (e) => {
    checkPhone(phone);
})

address.addEventListener('blur', (e) => {
    checkAddress(address);
})

formRegister.addEventListener("submit", (event) => {
    //event.preventDefault(); // prevent load page
    let isEmailValid = checkEmail(email);
    let isPasswordValid = checkPassword(password);
    let isPasswordConfirmValid = checkPasswordConfirm(password, passwordConfirm);
    let isFullName = checkFullName(fullName);
    let isPhoneValid = checkPhone(phone);
    let isAddressValid = checkAddress(address);
    let isFormConfirmed = checkFormConfirm(formConfirm);
    console.log(isFormConfirmed);
    if(isFormConfirmed && isFullName && isEmailValid && isPasswordValid && isPasswordConfirmValid && isPhoneValid && isAddressValid) {
        return true;
        console.log('success');
    } else {
        console.log('failed');
        return false;
    }
});