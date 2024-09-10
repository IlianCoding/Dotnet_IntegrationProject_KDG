import '../scss/slider.scss';
const prev = document.getElementById('prev-btn') as HTMLElement;
const next = document.getElementById('next-btn') as HTMLElement;
const list = document.getElementById('item-list') as HTMLElement;

const itemWidth = 150;
const padding = 10;

prev.addEventListener('click', () => {
    if (list) {
        list.scrollLeft -= itemWidth + padding;
    }
});

next.addEventListener('click', () => {
    if (list) {
        list.scrollLeft += itemWidth + padding;
    }
});
