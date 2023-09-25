// Set active navbar tab
const navItems = document.querySelectorAll('.header-list li');
navItems.forEach(item => {
    item.addEventListener('click', (e) => {
        //e.preventDefault();
        const currentActiveTab = document.querySelector('.header-list li.active');
        console.log(currentActiveTab);
        currentActiveTab.classList.remove('active');
        item.classList.add('active');
    })
})

//   Handle show form input comment
// const inputComment = document.querySelector('.comment-wrapper.reply input');
// const buttonReplyComment = document.querySelector('.comment-wrapper.reply button');
// console.log(inputComment);
// console.log(buttonReplyComment)
// buttonReplyComment.addEventListener('click', () => {
//     inputComment.style.display = 'block';
// })
