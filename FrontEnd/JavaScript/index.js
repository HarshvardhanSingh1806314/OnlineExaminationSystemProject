const sideMEnu = document.querySelector("aside");
const menuBtn = document.querySelector("#menu-btn");
const closeBtn = document.querySelector("#close-btn");
const themeToggler = document.querySelector(".theme-toggle");

menuBtn.addEventListener('click',() =>{
    sideMEnu.style.display='block';
})

closeBtn.addEventListener('click',() =>{
    sideMEnu.style.display='none';
})

themeToggler.addEventListener('click',() =>{
    document.body.classList.toggle('dark-theme-variables');

    themeToggler.querySelector('i:nth-child(1)').classList.toggle('active');
    themeToggler.querySelector('i:nth-child(2)').classList.toggle('active');
})