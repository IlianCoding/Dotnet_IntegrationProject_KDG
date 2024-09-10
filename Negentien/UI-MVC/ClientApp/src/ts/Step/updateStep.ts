
const buttonUpdate = document.getElementById("updateStep2")

//const fileInput = document.querySelector('#upload-input') as HTMLInputElement;
//const formData = new FormData(document.querySelector('#upload-form') as HTMLFormElement);
const qwoStep = document.getElementById("questionOption")
const qoStep = document.getElementById("questionOpen")
const infoStep = document.getElementById("information")

const qwoInput: HTMLInputElement | null = document.getElementById("vraagInput_m") as HTMLInputElement
const qoInput: HTMLInputElement | null = document.getElementById("vraagInput_o") as HTMLInputElement
const nameInput: HTMLInputElement | null = document.getElementById("nameInput_m") as HTMLInputElement
const themaInput: HTMLInputElement | null = document.getElementById("themes_m") as HTMLInputElement
const titleInput: HTMLInputElement = document.getElementById("title_input") as HTMLInputElement
const infoInput: HTMLInputElement = document.getElementById("info_input") as HTMLInputElement

function getStepId3() {
    const urlSearchParams = new URLSearchParams(window.location.search)
    let stepId = urlSearchParams.get("stepId")
    if (!stepId) {
        const path = window.location.pathname
        stepId = path.substring(path.lastIndexOf("=") + 1)
    }
    return parseInt(stepId)
}

async function updateContent() {
    console.log("Ive been called");
    if (qwoStep !== null) {
        const updateQuestionWithOptionsDto = {
            "QuestionText": qwoInput?.value
        };
        fetch(`/api/Steps/UpdateQuestionWithOptions/${parseInt(qwoStep.innerText)}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(updateQuestionWithOptionsDto)
        }).then(r => {
            if (!r.ok) {
                return r.json().then(error => {
                    let errorMessage = "";
                    error.forEach((e: any) => errorMessage += e.errorMessage + "\n");
                    alert(errorMessage);
                });
            }
            r.json().then(data => updateStep(data.id));
        }).then( ()=>   window.location.href = `/Step/StepDetail?stepId=${getStepId3()}`
        );
    } else if (qoStep !== null) {
        const updateQuestionOpenDto = {
            "QuestionText": qoInput?.value
        };
        fetch(`/api/Steps/UpdateQuestionOpen/${parseInt(qoStep.innerText)}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(updateQuestionOpenDto)
        }).then(r => {
            if (!r.ok) {
                return r.json().then(error => {
                    let errorMessage = "";
                    error.forEach((e: any) => errorMessage += e.errorMessage + "\n");
                    alert(errorMessage);
                });
            }
            r.json().then(data => updateStep(data.id));
        }).then( ()=>   window.location.href = `/Step/StepDetail?stepId=${getStepId3()}`
        );
    } else {
        const updateInfoDto = {
            "title": titleInput?.value,
            "textInformation": infoInput?.value,
            "objectName": "",
            "contentType": ""
        };
        fetch(`/api/Steps/UpdateInformation/${parseInt(infoStep!.innerText)}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            }
            ,
            body: JSON.stringify(updateInfoDto)
        }).then(r => {
            if (!r.ok) {
                return r.json().then(error => {
                    let errorMessage = "";
                    error.forEach((e: any) => errorMessage += e.errorMessage + "\n");
                    alert(errorMessage);
                });
            }
            r.json().then(data => updateStep(data.id));
        }).then( ()=>   window.location.href = `/Step/StepDetail?stepId=${getStepId3()}`
    );
        }
        /*if (fileInput.files?.length === 0) {
            const updateInfoDto = {
                "title": titleInput?.value,
                "textInformation": infoInput?.value,
                "objectName": "",
                "contentType": ""
            };
            fetch(`/api/Steps/UpdateInformation/${parseInt(infoStep!.innerText)}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(updateInfoDto)
            }).then(r => {
                if (!r.ok) {
                    return r.json().then(error => {
                        let errorMessage = "";
                        error.forEach((e: any) => errorMessage += e.errorMessage + "\n");
                        alert(errorMessage);
                    });
                }
                r.json().then(data => updateStep(data.id));
            });
        } else {
            fetch("/api/files", {
                method: "POST",
                body: formData
            }).then(response => {
                if (!response.ok) {
                    alert("Upload failed!");
                }

                if (response.status === 200) {
                    return response.json() as Promise<Url>;
                } else {
                    throw new Error("Unexpected response status: " + response.status);
                }
            })
                .then(responseObject => {

                    const updateInfoDto = {
                        "title": titleInput?.value,
                        "textInformation": infoInput?.value,
                        "objectName": responseObject.objectName,
                        "contentType": responseObject.contentType
                    };

                    fetch('/api/Steps/AddInformation', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(updateInfoDto)
                    }).then(r => {
                        if (!r.ok) {
                            return r.json().then(error => {
                                let errorMessage = "";
                                error.forEach((e: any) => errorMessage += e.errorMessage + "\n");
                                alert(errorMessage);
                            });
                        }
                        r.json().then(data => updateStep(data.id));
                    })
                })

        }

    }*/

}

async function updateStep(contentID: number) {
    const newStepDto = {
        ThemeId: themaInput!.value,
        ContentId: contentID.toString(),
        Name: nameInput!.value
    }
    const response = await fetch(`/api/Steps/UpdateStep/${getStepId3()}`,
        {
            method: "PUT",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(newStepDto)
        })
}

console.log(buttonUpdate)
buttonUpdate!.addEventListener('click', () => updateContent())