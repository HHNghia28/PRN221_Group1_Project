// Set active navbar tab
const navItems = document.querySelectorAll('.header-list li');
console.group(navItems);
navItems.forEach(item => {
    item.addEventListener('click', () => {
        const currentActiveTab = document.querySelector('.header-list li.active');
        console.log(currentActiveTab);
        currentActiveTab.classList.remove('active');
        item.classList.add('active');
    })
})

// Show hide menu 
const btnOpenMenu = document.querySelector('.btn-menu');
const btnCloseMenu = document.querySelector('.close-nav-menu');
const headerMenu = document.querySelector('.header-menu');
const overplay = document.querySelector('.overplay');
const formWrapper = document.querySelector('.form-wrapper');
const btnCloseForm = document.querySelector('.close-form ');


btnOpenMenu.addEventListener('click', () => {
    console.log('clicked open');
    headerMenu.classList.add('d-block', 'd-lg-none');
})
btnCloseMenu.addEventListener('click', () => {
    console.log('clicked close');
    headerMenu.classList.remove('d-block', 'd-lg-none');
})

// Show hide login, register form
const btnLogin = document.querySelector('.btn-login');
const btnRegister = document.querySelector('.btn-register');

const showOverplay = () => {
    overplay.classList.add('d-flex');
}
const hideOverplay = () => {
    overplay.classList.remove('d-flex');
}

btnLogin.addEventListener('click', () => {
    showOverplay();
    formWrapper.classList.add('d-block');
})

btnRegister.addEventListener('click', () => {
    showOverplay();
    formWrapper.classList.add('d-block');
})
btnCloseForm.addEventListener('click', () => {
    hideOverplay();
    formWrapper.classList.remove('d-block');
})
