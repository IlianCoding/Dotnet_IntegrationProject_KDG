import {addDeleteEventListener} from "./deleteFlow";

addDeleteEventListener()

function addEventListenerDeleteStep() {
    const deleteStepButtons = document.querySelectorAll('[id^="removeStep_"]');

    deleteStepButtons.forEach((button) => {
        button.addEventListener('click', () => {
            const stepId = parseInt(button.id.split('removeStep_')[1]);
            deleteStep(stepId);
        });
    });
}

function addEventListenerDeactivateStep() {
    const deactivateStepButtons = document.querySelectorAll('[id^="deactivateStep_"]');

    deactivateStepButtons.forEach((button) => {
        button.addEventListener('click', () => {
            const stepId = parseInt(button.id.split('deactivateStep_')[1]);
            deactivateStep(stepId);
        });
    });
}

function addEventListenerActivateStep() {
    const activateStepButtons = document.querySelectorAll('[id^="activateStep_"]');

    activateStepButtons.forEach((button) => {
        button.addEventListener('click', () => {
            const stepId = parseInt(button.id.split('activateStep_')[1]);
            activateStep(stepId);
        });
    });

}

function deleteStep(stepId: number) {
    fetch(`/api/Steps/RemoveStep/${stepId}`, {
        method: "DELETE",
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        }
    }).then(
        response => {
            if (!response.ok) {
                alert("Delete failed: " + response.status)
            } else {
                deleteStepElements(stepId)
            }
        }
    )
        .catch(() => {
            alert("Oeps, something went wrong!")
        })
}

function deactivateStep(stepId: number) {
    fetch(`/api/Steps/DeactivateStep/${stepId}`, {
        method: "PUT",
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        }
    }).then(
        response => {
            if (!response.ok) {
                alert("Deactivate failed: " + response.status)
            } else {
                const stepState = document.getElementById(`stepState_${stepId}`);
                const oldButton = document.getElementById(`deactivateStep_${stepId}`);
                stepState!.innerText = "Closed";

                const newButton = document.createElement('button');
                newButton.id = `activateStep_${stepId}`;
                newButton.className = "btn btn-primary";
                newButton.type = "button";
                newButton.textContent = "Activate step";
                newButton.addEventListener('click', () => activateStep(stepId));

                oldButton!.parentNode!.replaceChild(newButton, oldButton!);
            }
        }
    )
        .catch((error) => {
            alert("Oeps, something went wrong!" + error.message)
        })
}

function activateStep(stepId: number) {
    fetch(`/api/Steps/ActivateStep/${stepId}`, {
        method: "PUT",
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        }
    }).then(
        response => {
            if (!response.ok) {
                alert("Activate failed: " + response.status)
            } else {
                const stepState = document.getElementById(`stepState_${stepId}`);
                const oldButton = document.getElementById(`activateStep_${stepId}`);
                stepState!.innerText = "Open";

                const newButton = document.createElement('button');
                newButton.id = `deactivateStep_${stepId}`;
                newButton.className = "btn btn-primary";
                newButton.type = "button";
                newButton.textContent = "Deactivate step";
                newButton.addEventListener('click', () => deactivateStep(stepId));

                oldButton!.parentNode!.replaceChild(newButton, oldButton!);
            }
        }
    )
        .catch((error) => {
            alert("Oeps, something went wrong!" + error.message)
        })

}

function deleteStepElements(stepId: number) {
    const elementToRemove = <HTMLElement>document.getElementById(`stepRow_${stepId}`);

    elementToRemove.remove();
}


addEventListenerDeleteStep();
addEventListenerDeactivateStep();
addEventListenerActivateStep();

