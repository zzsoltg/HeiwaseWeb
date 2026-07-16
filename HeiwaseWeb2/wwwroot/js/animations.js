
export function observeElement(element) {
    if (!element) return;

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                element.classList.add('visible');
                observer.unobserve(element);
            }
        });
    }, { threshold: 0.1 });

    observer.observe(element);
}

export function initScrollAnimations() {
    const cards = document.querySelectorAll('.feature-card');
    const windowHeight = window.innerHeight;

    const checkVisibility = () => {
        cards.forEach(card => {
            const position = card.getBoundingClientRect().top;
            if (position < windowHeight - 100) {
                card.classList.add('visible');
            }
        });
    };

    window.addEventListener('scroll', checkVisibility);
    checkVisibility();
}