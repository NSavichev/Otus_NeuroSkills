window.setTheme = (theme) => {
    const root = document.documentElement;
    
    if (theme === 'dark') {
        root.setAttribute('data-theme', 'dark');
    } else if (theme === 'light') {
        root.setAttribute('data-theme', 'light');
    } else {
        // Auto - определяем по системной теме
        if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
            root.setAttribute('data-theme', 'dark');
        } else {
            root.setAttribute('data-theme', 'light');
        }
    }
};

// Инициализация темы при загрузке
document.addEventListener('DOMContentLoaded', () => {
    window.setTheme('auto');
});

// Слушаем изменения системной темы
if (window.matchMedia) {
    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', (e) => {
        const root = document.documentElement;
        const currentTheme = root.getAttribute('data-theme');
        if (currentTheme === 'auto' || !currentTheme) {
            window.setTheme('auto');
        }
    });
}
