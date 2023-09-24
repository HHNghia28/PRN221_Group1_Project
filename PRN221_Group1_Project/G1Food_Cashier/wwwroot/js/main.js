const toastList = document.querySelector(".toast-wrapper");
const btnSuccess = document.querySelector(".wrapper .bg-success");
const btnWarning = document.querySelector(".wrapper .bg-warning");
const btnError = document.querySelector(".wrapper .bg-error");

const toastItemSuccess = document.querySelector(
    ".toast-item.toast-success"
);
const toastItemWarning = document.querySelector(
    ".toast-item.toast-warning"
);
const toastItemError = document.querySelector(".toast-item.toast-error");

const createToast = (statusToast) => {
    const toastItem = document.createElement("div");
    toastItem.classList.add("toast-item");
    toastItem.classList.add("toast-" + statusToast);
    toastItem.innerHTML = `
        <i class="fa-regular fa-circle-check"></i>
        <span class="toast-message">This is ${statusToast} message!</span>
        <span class="count-down"></span>
      `;
    toastList.appendChild(toastItem);
    setTimeout(() => {
        toastItem.style.animationName = "slideHide";
    }, 6500);
    setTimeout(() => {
        toastItem.remove();
    }, 8000);
};

btnSuccess.addEventListener("click", (event) => {
    createToast("success");
});

btnWarning.addEventListener("click", (event) => {
    createToast("warning");
});

btnError.addEventListener("click", (event) => {
    createToast("error");
});