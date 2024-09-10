import {cp} from "fs";

const cpButton = document.getElementById("cpButton")
const stepSelect = document.getElementById("cp_step") as HTMLSelectElement
const answerSelect = document.getElementById("answeroption") as HTMLSelectElement
const nameInput = document.getElementById('nameInput') as HTMLTextAreaElement

export function getStepId() {
    const urlSearchParams = new URLSearchParams(window.location.search)
    let stepId = urlSearchParams.get("stepId")
    if (!stepId) {
        const path = window.location.pathname
        stepId = path.substring(path.lastIndexOf("=") + 1)
    }
    return parseInt(stepId)
}

function getIdFromURL(url: string | null) {
    if (url != null) {
        let id = url.substring(url.lastIndexOf("/") + 1)
        return parseInt(id, 10) || 0;
    }
    return -1
}

async function addConditionalPoint() {
    const response = await fetch(`/api/ConditionalPoints`,
        {
            method: "POST",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                "ConditionalPointName": nameInput!.value.trim(),
                "StepId": JSON.parse(stepSelect!.value)
            })
        })
    const createdConditionalPoint = await response.json();
    const url = response.headers.get("location")
    console.log(url)
    const cpId = getIdFromURL(url);

    await addCpToAnswerOption(cpId)
    //await updateStepToConditional()
    await getStep();
    //TODO: maak id niet hardcoded
    window.location.href = `/Step/StepDetail?stepId=${getStepId()}`;
}

async function addCpToAnswerOption(cpId: Number) {
    const response2 = await fetch(`/api/AnswerOptions/${answerSelect.value}`,
        {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                "ConditionalPointId": cpId
            })
        })
}


/*async function updateStepToConditional() {
    const response3 = await fetch(`/api/Steps/${stepSelect.value}/conditional`,
        {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                "isConditioneel": true
            })
        })
}*/

async function getStep() {
    const response = await fetch(`/api/Steps/Get/${getStepId()}`,
        {
            method: "GET"
        })
    const step = await response.json()
    const nextStepId= step.nextStep.id;
    await updateNextStepOfConditionalStep(nextStepId)
    
}
async function updateNextStepOfConditionalStep(nextStepId: Number){
    const response = await fetch(`/api/Steps/${stepSelect.value}/nextStep`,
        {
            method: "PUT",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                "NextStepId": nextStepId
            })
        })
}

cpButton!.addEventListener('click', addConditionalPoint)