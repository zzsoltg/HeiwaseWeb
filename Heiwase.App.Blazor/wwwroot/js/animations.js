const SLIDER_TRANSITION = 'transform 0.5s ease-in-out';

export function initAnimations() {
    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('visible');
                observer.unobserve(entry.target);
            }
        });
    }, { threshold: 0.1 });

    const cards = document.querySelectorAll('.feature-card, .info-bubble, .feature-item');
    cards.forEach(card => observer.observe(card));
}
function getSlotWidth(trackEl) {
    const card = trackEl.firstElementChild;
    if (!card) return 0;
    const gap = parseFloat(getComputedStyle(trackEl).gap) || 0;
    return card.offsetWidth + gap;
}
function awaitTransitionEnd(trackEl) {
    return new Promise(resolve => {
        function handler(e) {
            if (e.propertyName === 'transform') {
                trackEl.removeEventListener('transitionend', handler);
                resolve();
            }
        }
        trackEl.addEventListener('transitionend', handler);
    });
}

export function initTrack(trackId) {
    const track = document.getElementById(trackId);
    if (!track) return;
    const slot = getSlotWidth(track);
    track.style.transition = 'none';
    track.style.transform  = `translateX(-${slot}px)`;
    void track.offsetHeight;
}

export function slideTrackRight(trackId) {
    const track = document.getElementById(trackId);
    if (!track) return Promise.resolve();
    const done = awaitTransitionEnd(track);
    track.style.transition = SLIDER_TRANSITION;
    track.style.transform  = 'translateX(0)';
    return done;
}

export function slideTrackLeft(trackId) {
    const track = document.getElementById(trackId);
    if (!track) return Promise.resolve();
    const slot = getSlotWidth(track);
    const done = awaitTransitionEnd(track);
    track.style.transition = SLIDER_TRANSITION;
    track.style.transform  = `translateX(-${2 * slot}px)`;
    return done;
}

export function resetTrack(trackId) {
    const track = document.getElementById(trackId);
    if (!track) return;
    const slot = getSlotWidth(track);
    track.style.transition = 'none';
    track.style.transform  = `translateX(-${slot}px)`;
    void track.offsetHeight;
}