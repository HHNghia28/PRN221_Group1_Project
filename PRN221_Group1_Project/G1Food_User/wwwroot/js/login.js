const email = document.getElementById("email");
const password = document.getElementById("password");
const formLogin = document.getElementById("login-form");

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
         /*input.focus();*/
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
         //input.focus();
        return false;
    }
}

email.addEventListener('blur', (e) => {
    checkEmail(email);
})
password.addEventListener('blur', (e) => {
    checkPassword(password);
})

formLogin.addEventListener("submit", (event) => {
    let isEmailValid = checkEmail(email);
    let isPasswordValid = checkPassword(password);
    if (isEmailValid && isPasswordValid) {
        return true;
    } else {
        return false;
    }
});
