// Set active navbar tab
const navItems = document.querySelectorAll('.header-list li');
if (navItems != null) {
navItems.forEach(item => {
    item.addEventListener('click', (e) => {
        //e.preventDefault();
        const currentActiveTab = document.querySelector('.header-list li.active');
        console.log(currentActiveTab);
        currentActiveTab.classList.remove('active');
        item.classList.add('active');
    })
})

}