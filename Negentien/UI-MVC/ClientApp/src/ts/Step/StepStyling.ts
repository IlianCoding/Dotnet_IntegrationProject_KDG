const colorMap = {
    "Default": "#fd9c6b",
    "Light Coral": "#F08080",
    "Khaki": "#F0E68C",
    "Lavender": "#E6E6FA",
    "Thistle": "#D8BFD8",
    "Pale Turquoise": "#AFEEEE"
}

const stepId : string = document.getElementById("stepId")!.textContent!;

function applyStylingElements() {
    fetch(`/api/Steps/GetStylingElements/${stepId}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        }
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Error fetching project styling " + response.statusText);
            }
            return response.json();
        })
        .then(projectStyling => {
            let color: string = colorMap[projectStyling.primaryColor as keyof typeof colorMap]
            document.documentElement.style.setProperty('--primary1', color);
            document.documentElement.style.setProperty('--main-font', `"${projectStyling.font}"`);
        })
        .catch(error => {
            console.log(error);
        });
}

applyStylingElements();