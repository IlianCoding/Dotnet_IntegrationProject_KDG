import {isInputRequiredMinLength, spanMessageByInput} from "../validateName";
import {getIdOfCurrentHtml} from "../getIdOfCurrentHtml";
import * as flow from "../Project/Project";


const editDiv = document.getElementById('editDiv') as HTMLDivElement
const flowNameInput = document.getElementById('flowName') as HTMLInputElement
const flowNameSpan = document.getElementById('flowNameSpan') as HTMLSpanElement
const radioOpenStatus = document.getElementById('radioOpenStatus') as HTMLInputElement
const radioCloseStatus = document.getElementById('radioCloseStatus') as HTMLInputElement
const radioTrueLineair = document.getElementById('radioTrueLineair') as HTMLInputElement
const radioFalseLineair = document.getElementById('radioFalseLineair') as HTMLInputElement
const updateFlowButton = document.getElementById('updateFlow') as HTMLButtonElement
const HeaderFlowName = document.getElementById('HeaderFlowName') as HTMLButtonElement
const projectStatusFlowDetail = document.getElementById('projectStatusFlowDetail') as HTMLInputElement


const detailDiv = document.getElementById('detailDiv') as HTMLDivElement
const flowNameDetailSpan = document.getElementById('flowNameDetailSpan') as HTMLSpanElement
const flowStatusDetailSpan = document.getElementById('flowStatusDetailSpan') as HTMLSpanElement
const flowLinairStatusDetailSpan = document.getElementById('flowLinairStatusDetailSpan') as HTMLSpanElement
const editFlowButton = document.getElementById('editFlowButton') as HTMLButtonElement
const changeFlowStatusButon = document.getElementById('changeFlowStatus') as HTMLButtonElement


const defaultFlowNameInput = document.getElementById('defaultFlowNameInput') as HTMLInputElement
const defaultFlowStatusInput = document.getElementById('defaultFlowStatusInput') as HTMLInputElement
const defaultFlowIsLinearInput = document.getElementById('defaultFlowIsLinearInput') as HTMLInputElement
const modalFlowDetail = document.getElementById('modalFlowDetail') as HTMLElement
const noteWarning = document.getElementById('noteWarning') as HTMLDivElement
const noteWarningStartNote = document.getElementById('noteWarningStartNote') as HTMLDivElement

function showFlowDivWarning(status: string) {
    noteWarning.style.display = ((status == "true") ? "none" : "block")
}

function showStartFlowDivWarning(status: number) {
    noteWarningStartNote.style.display = ((status == 0) ? "none" : "block")
}

function getStatusByInputs(openInput: HTMLInputElement, closeInput: HTMLInputElement) {
    if (openInput.checked && !closeInput.checked) {
        return 0;
    } else {
        return 2;
    }
}


function beginLayoutFlow() {
    switch (defaultFlowStatusInput.value) {
        case "Closed":
            radioCloseStatus.checked = true
            radioOpenStatus.checked = false
            break;
        case "Open":
            radioCloseStatus.checked = false
            radioOpenStatus.checked = true
            break;
    }
    switch (defaultFlowIsLinearInput.value) {
        case "true":
            radioTrueLineair.checked = true
            radioFalseLineair.checked = false
            break;
        case "false":
            radioTrueLineair.checked = false
            radioFalseLineair.checked = true
            break;
    }
    if (projectStatusFlowDetail.value == "false") {
        radioOpenStatus.disabled = true;
        radioCloseStatus.disabled = true;
    }

}

function modalFlowDetailLayout() {
    if (defaultFlowStatusInput.value == "Closed") {
        modalFlowDetail.textContent = "Are you certain you wish to open the flow?\n This will allow attendants to initiate it.";
    } else {
        modalFlowDetail.textContent = `Are you certain you wish to close the flow?\n Doing so will prevent attendants from initiating it.`
    }

}

function validateFlow(name: HTMLInputElement, status: number) {
    let validateName = isInputRequiredMinLength(name);
    let validateStatus = () => {
        return status == 0 || status == 2;
    };
    return validateName && validateStatus()
}

async function updateFlow(name: HTMLInputElement, status: number, isLineair: boolean) {
    
    if (!validateFlow(name, status)) {
        return;
    }
    try {
        const respons = await fetch(`/api/Flows/${getIdOfCurrentHtml()}/update`, {
            method: "PUT",
            headers: {
                accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                "flowName": name.value,
                "state": status,
                "isLinear": isLineair
            })
        })
        if (respons.status === 202) {
            editDiv.style.display = "none";
            detailDiv.style.display = "block";
            if (status) {

            }
            defaultFlowNameInput.value = name.value
            flowNameDetailSpan.textContent = name.value
            HeaderFlowName.textContent = name.value
            defaultFlowStatusInput.value = (status === 0) ? "Open" : (status === 2) ? "Closed" : "Unknown";
            flowStatusDetailSpan.textContent = (status === 0) ? "Open" : (status === 2) ? "Closed" : "Unknown";
            if (status == 0) {
                radioOpenStatus.checked = true;
                radioCloseStatus.checked = false;

            } else if (status == 2) {
                radioOpenStatus.checked = false;
                radioCloseStatus.checked = true;
            }
            defaultFlowIsLinearInput.value = String(isLineair)
            flowLinairStatusDetailSpan.textContent = isLineair ? "Linear" : "Circular"
            showStartFlowDivWarning(status)

        }
    } catch (error) {
        console.error("Error creating project:", error);
    }
}

function statusSetClose() {
    let flowIsLinearr;
    if (defaultFlowIsLinearInput.value == "true") {
        flowIsLinearr = true
    } else if (defaultFlowIsLinearInput.value == "false") {
        flowIsLinearr = false
    } else {
        console.error("Flow isLinearrr error")
        return;
    }
    updateFlow(defaultFlowNameInput, 2, flowIsLinearr)
        .then(() => {
            console.log("Updaten done")
        })
}

function statusSetOpen() {
    let flowIsLinearr;
    if (defaultFlowIsLinearInput.value == "true") {
        flowIsLinearr = true
    } else if (defaultFlowIsLinearInput.value == "false") {
        flowIsLinearr = false
    } else {
        console.error("Flow isLinearrr error")
        return;
    }
    updateFlow(defaultFlowNameInput, 0, flowIsLinearr).then(() => {
        console.log("update done")
    })
}


function changeFlowStatusButonAction(flowStatus: number) {
    switch (flowStatus) {
        case 0:
            statusSetClose();
            break;
        case 2:
            statusSetOpen();
            break;
        default:
            console.error("flowStatus cannot foudn")
            return;
    }

}

window.addEventListener('DOMContentLoaded', () => {
    modalFlowDetailLayout();
    editDiv.style.display = "none";
    detailDiv.style.display = "block";
    showStartFlowDivWarning((defaultFlowStatusInput.value == "Closed" ? 2 : 0))
    showFlowDivWarning(projectStatusFlowDetail.value);
})

window.addEventListener('DOMContentLoaded', () => {
    beginLayoutFlow();
})
if (changeFlowStatusButon) {
    changeFlowStatusButon.addEventListener('click', () => {
        modalFlowDetailLayout()
        let flowStatuss;
        if (defaultFlowStatusInput.value == "Closed") {
            flowStatuss = 2
        } else if (defaultFlowStatusInput.value == "Open") {
            flowStatuss = 0
        } else {
            console.error("Werkt niet flowStatus")
            return;
        }
        changeFlowStatusButonAction(flowStatuss);
    })
}
if (editFlowButton) {
    editFlowButton.addEventListener('click', () => {
        editDiv.style.display = "block";
        detailDiv.style.display = "none";
    })
}

if (updateFlowButton) {
    flowNameInput.addEventListener("blur", () => spanMessageByInput(flowNameInput, flowNameSpan));
    updateFlowButton.addEventListener("click", () => {
        updateFlow(
            flowNameInput,
            getStatusByInputs(radioOpenStatus, radioCloseStatus),
            flow.getStatus(radioTrueLineair, radioFalseLineair)).then(() => {
            console.log("update done")
        })

        showFlowDivWarning(projectStatusFlowDetail.value);
    })
}
