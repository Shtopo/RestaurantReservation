const navBtn = document.querySelector('.header__nav-btn');
const navClose = document.querySelector('.header__nav-close');
const navlist = document.querySelector('.header__nav-list');
const menuClose = document.querySelector('.menu--close');

navBtn.addEventListener('click', () => {
  navlist.classList.toggle('header__nav-list--open');
  menuClose.classList.toggle('menu--open');
});
navClose.addEventListener('click', () => {
  navlist.classList.remove('header__nav-list--open');
  menuClose.classList.remove('menu--open');
}); 


//скрол до секциій
document.querySelectorAll('.booking, .tables').forEach(anchor => {
    anchor.addEventListener('click', function(e) {
  
        const targetId = this.classList.contains('booking') ? 'tables' : 'tables';
        const targetSection = document.getElementById(targetId);

        if (targetSection) {
            window.scrollTo({
                top: targetSection.offsetTop - 20,
                behavior: 'smooth'
            });
        }
    });
});

