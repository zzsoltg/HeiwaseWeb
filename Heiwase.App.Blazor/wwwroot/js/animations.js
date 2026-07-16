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

// Returns the width of one card slot (card width + gap) in pixels.
function getSlotWidth(trackEl) {
    const card = trackEl.firstElementChild;
    if (!card) return 0;
    const gap = parseFloat(getComputedStyle(trackEl).gap) || 0;
    return card.offsetWidth + gap;
}

// Returns a Promise that resolves when the CSS transition on the track ends.
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

// --- Competitor track (top slider, left→right flow) ---
// Buffer card is at DOM position 0 (off-screen left).
// Resting position: translateX(-slotWidth) → shows cards 1–3.

export function initCompetitorTrack(trackId) {
    const track = document.getElementById(trackId);
    if (!track) return;
    const slot = getSlotWidth(track);
    track.style.transition = 'none';
    track.style.transform  = `translateX(-${slot}px)`;
    // Force reflow so the browser registers the starting position.
    void track.offsetHeight;
}

// Animates right: buffer (card 0) slides in from the left, card 3 exits right.
export function slideCompetitorTrack(trackId) {
    const track = document.getElementById(trackId);
    if (!track) return Promise.resolve();
    const done = awaitTransitionEnd(track);
    track.style.transition = SLIDER_TRANSITION;
    track.style.transform  = 'translateX(0)';
    return done;
}

// Instantly resets the competitor track back to -slotWidth for the next cycle.
export function resetCompetitorTrack(trackId) {
    const track = document.getElementById(trackId);
    if (!track) return;
    const slot = getSlotWidth(track);
    track.style.transition = 'none';
    track.style.transform  = `translateX(-${slot}px)`;
    void track.offsetHeight;
}

// --- Senpai track (bottom slider, right→left flow) ---
// Buffer card is at DOM position 3 (off-screen right).
// Resting position: translateX(0) → shows cards 0–2.

export function initSenpaiTrack(trackId) {
    const track = document.getElementById(trackId);
    if (!track) return;
    track.style.transition = 'none';
    track.style.transform  = 'translateX(0)';
    void track.offsetHeight;
}

// Animates left: card 0 exits left, buffer (card 3) slides in from the right.
export function slideSenpaiTrack(trackId) {
    const track = document.getElementById(trackId);
    if (!track) return Promise.resolve();
    const slot = getSlotWidth(track);
    const done = awaitTransitionEnd(track);
    track.style.transition = SLIDER_TRANSITION;
    track.style.transform  = `translateX(-${slot}px)`;
    return done;
}

// Instantly resets the senpai track back to translateX(0) for the next cycle.
export function resetSenpaiTrack(trackId) {
    const track = document.getElementById(trackId);
    if (!track) return;
    track.style.transition = 'none';
    track.style.transform  = 'translateX(0)';
    void track.offsetHeight;
}