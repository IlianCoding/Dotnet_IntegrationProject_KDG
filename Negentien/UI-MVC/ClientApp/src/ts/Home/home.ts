const navbarButton: HTMLElement | null = document.getElementById("navbar_button");
function getYear(): void {
    const currentDate: Date = new Date();
    const currentYear: number = currentDate.getFullYear();
    const displayYearElement: HTMLElement | null = document.querySelector("#displayYear");
    if (displayYearElement) {
        displayYearElement.innerHTML = currentYear.toString();
    }
}

getYear();

function openNav(): void {
    const myNavElement: HTMLElement | null = document.getElementById("myNav");
    const customMenuBtnElement: HTMLElement | null = document.querySelector(".custom_menu-btn");
    if (myNavElement) {
        myNavElement.classList.toggle("menu_width");
    }
    if (customMenuBtnElement) {
        customMenuBtnElement.classList.toggle("menu_btn-style");
    }
}

if (navbarButton) {
    navbarButton.addEventListener("click", openNav);
}



