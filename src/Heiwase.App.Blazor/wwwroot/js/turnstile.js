const SCRIPT_URL = 'https://challenges.cloudflare.com/turnstile/v0/api.js?render=explicit';

let scriptPromise = null;

function loadScript() {
    if (window.turnstile) {
        return Promise.resolve();
    }

    if (!scriptPromise) {
        scriptPromise = new Promise((resolve, reject) => {
            const script = document.createElement('script');
            script.src = SCRIPT_URL;
            script.async = true;
            script.onload = () => resolve();
            script.onerror = () => {
                scriptPromise = null;
                script.remove();
                reject(new Error('The Turnstile script could not be loaded.'));
            };
            document.head.appendChild(script);
        });
    }

    return scriptPromise;
}

export async function render(elementId, siteKey, language, dotNetRef) {
    await loadScript();

    return window.turnstile.render(`#${elementId}`, {
        sitekey: siteKey,
        language: language,
        callback: token => dotNetRef.invokeMethodAsync('OnCaptchaSolved', token),
        'expired-callback': () => dotNetRef.invokeMethodAsync('OnCaptchaExpired'),
        'error-callback': () => dotNetRef.invokeMethodAsync('OnCaptchaExpired')
    });
}

export function reset(widgetId) {
    window.turnstile?.reset(widgetId);
}

export function remove(widgetId) {
    window.turnstile?.remove(widgetId);
}
