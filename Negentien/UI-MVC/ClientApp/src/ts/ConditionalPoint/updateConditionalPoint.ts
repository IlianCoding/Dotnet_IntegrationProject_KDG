import {getStepId} from "./addConditionalPoint";
const answerSelect = document.getElementById("answeroption") as HTMLSelectElement

const cpButton = document.getElementById("cpButton")
const stepSelect = document.getElementById("cp_step") as HTMLSelectElement
const conditionalPoint = document.getElementById("conditional") as HTMLSelectElement
const conditionalPointId = parseInt(conditionalPoint.innerText!)
const nameInput = document.getElementById('nameInput') as HTMLTextAreaElement
function getStepIds() {
    const urlSearchParams = new URLSearchParams(window.location.search)
    let stepId = urlSearchParams.get("stepId")
    if (!stepId) {
        const path = window.location.pathname
        stepId = path.substring(path.lastIndexOf("/") + 1)
    }
    return parseInt(stepId)
}

async function updateConditionalPoint() {
    const response = await fetch(`/api/ConditionalPoints/Update/${conditionalPointId}`,
        {
            method: "PUT",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                "ConditionalPointName": nameInput!.value.trim(),
                "StepId": JSON.parse(stepSelect!.value)
            })
        })
    await getStep();
    //TODO: maak id niet hardcoded
    window.location.href = `/Step/StepDetail?stepId=${getStepIds()}`;
}
async function getStep() {
    const response = await fetch(`/api/Steps/Get/${getStepIds()}`,
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

cpButton!.addEventListener('click', updateConditionalPoint)