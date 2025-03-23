async function downloadPortfolio() {
    let username = document.getElementById('github-username-download').value
    window.open(`${window.location.origin}/portfolio/${username}/download`);    
}
async function generatePortfolio() {
    let username = document.getElementById('github-username').value
    let errorContent = document.getElementById('error-message');
    let readyContent = document.getElementById('ready-message');
    if (readyContent)
        readyContent.style.display = 'none';
    
    errorContent.style.display = 'none'
    if (username == null || username.trim() == '') {
        errorContent.innerHTML = 'Please enter a valid username.'
        errorContent.style.display = '';
    }
    errorContent.style.display = 'none';
    try {
        const response = await fetch(`/portfolio/${username}`);

        if (!response.ok) {
            const data = await response.json();
            // Exibe informações baseadas no padrão ProblemDetails
            errorContent.innerHTML = `
            <strong>${data.title || 'Error'}</strong><br>
            <span>${data.detail || 'unexpected error occurred. Please try again later or open a Issue at our Repository.'}</span><br>
            <small>Star our repo and try again in 1 minute. If you believe this is a mistake, please open an issue on <a href="https://github.com/brunobritodev/awesome-github-portfolio" class="text-white" target="_blank">GitHub</a>.</small>
          `;
            errorContent.style.display = '';
            return;
        }
        let divGenerate = document.getElementById("generate");
        divGenerate.classList.add("d-none");

        let btnWaiting = document.getElementById("waiting");
        btnWaiting.classList.remove("d-none");
        btnWaiting.classList.add("waiting");

        const interval = setInterval(async () => {
            try {
                const statusResponse = await fetch(`/portfolio/${username}/status`);
                const statusData = await statusResponse.json();

                if (statusData.isReady) {
                    clearInterval(interval);
                    window.location.assign(`${window.location.origin}/${username}`);
                }
            } catch (err) {
                errorContent.innerHTML = 'unexpected error occurred. Please try again later or open a Issue at our Repository';
                errorContent.style.display = '';
                clearInterval(interval);
            }
        }, 1000);

    } catch (err) {
        errorContent.style.display = '';
        errorContent.innerHTML = 'unexpected error occurred. Please try again later or open a Issue at our Repository.';
    }
}