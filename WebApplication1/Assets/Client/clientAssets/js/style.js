const translations = {
    vi: {
        greeting: 'Xin chào!',
        description: 'Đây là trang web của tôi.'
    },
    en: {
        greeting: 'Hello!',
        description: 'This is my website.'
    },
    fr: {
        greeting: 'Bonjour!',
        description: 'Ceci est mon site web.'
    }
};

const languageItems = document.querySelectorAll('.dropdown-item');
const translatableElements = document.querySelectorAll('[data-i18n]');
const languageText = document.querySelector('.language-text');

languageItems.forEach(item => {
    item.addEventListener('click', (event) => {
        event.preventDefault(); // Ngăn chặn hành vi mặc định của thẻ a
        const lang = item.dataset.lang;
        updateContent(lang);
        languageText.textContent = `NGÔN NGỮ HIỂN THỊ: ${item.textContent}`; // Cập nhật văn bản hiển thị
    });
});

function updateContent(lang) {
    translatableElements.forEach(element => {
        const key = element.dataset.i18n;
        element.textContent = translations[lang][key];
    });
}