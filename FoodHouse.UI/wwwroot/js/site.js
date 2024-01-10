// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//sliders

const swiper = new Swiper('.swiper', {
    // Optional parameters
    // autoHeight: true,
    loop: true,
    speed: 700,
    slidesPerView: 1,
    spaceBetween: 1500,
    navigation: {
      nextEl: '#sliderNext',
      prevEl: '#sliderPrev',
    },
    keyboard: {
      enabled: true,
    },
    // autoplay: {
    //   delay: 3500,
    //   disableOnInteraction: false,
    // },
  });

  const slider = new Swiper('.newswiper', {
    // Optional parameters
    // autoHeight: true,
    loop: true,
    speed: 700,
    slidesPerView: 2,
    spaceBetween: 150,
    keyboard: {
      enabled: true,
    },
    autoplay: {
      delay: 3500,
      disableOnInteraction: false,
    },
  });
  // tabs
document.addEventListener('DOMContentLoaded', function () {
  const categoryTabs = document.querySelectorAll('.category-tab');
  const catalogContents = document.querySelectorAll('.catalog-content > div');

  categoryTabs.forEach(tab => {
      tab.addEventListener('click', function () {
          const category = this.getAttribute('data-category');
          catalogContents.forEach(content => {
              content.style.display = 'none';
          });
          const selectedCategory = document.querySelector(`.${category}-items`);
          if (selectedCategory) {
              selectedCategory.style.display = 'flex';
          }
          categoryTabs.forEach(tab => {
              tab.classList.remove('category-active');
          });
          this.classList.add('category-active');
      });
  });
});

document.addEventListener("DOMContentLoaded", function() {
  const burgerButton = document.getElementById("burger");
  const navMenu = document.getElementById("openb"); // змінено селектор, щоб отримати елемент nav
  const tableContainer = document.querySelector(".table-container");

  navMenu.classList.remove("open"); // Видалення класу open при завантаженні сторінки

  burgerButton.addEventListener("click", function() {
    burgerButton.classList.toggle("active");
    navMenu.classList.toggle("open");
    tableContainer.classList.toggle("open");

    if (navMenu.classList.contains("open")) {
      setTimeout(function() {
        tableContainer.style.maxWidth = "1260px";
      }, 0); // Затримка для візуальної анімації
    } else {
      tableContainer.style.maxWidth = "1423px";
    }
  });
});

let subMenu = document.getElementById("subMenu");
function toggleMenu () {
    subMenu.classList.toggle("open-menu")
}
