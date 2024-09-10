import {updateRunningFlow} from "./UpdateRunningFlow";

const closedRunningFlowTbody = document.getElementById('closedRunningFlowTbody') as HTMLTableElement


document.addEventListener("DOMContentLoaded", function () {
    const runningFlowsRows = document.querySelectorAll(".runningFlow");

    runningFlowsRows.forEach((runningFlowRow) => {
        const statusCell = runningFlowRow.querySelector("#runningState") as HTMLInputElement;
        const idCell = runningFlowRow.querySelector("#runningId") as HTMLInputElement;
        const timeCell = runningFlowRow.querySelector("#runningTime") as HTMLInputElement;
        const pauzeButton = runningFlowRow.querySelector("#setPauzeRunningflow") as HTMLButtonElement;
        const startButton = runningFlowRow.querySelector("#setStartRunningflow") as HTMLButtonElement;
        const stopButton = runningFlowRow.querySelector("#stopButton") as HTMLButtonElement;


        setStatusRunningFlowLayout(statusCell, pauzeButton, startButton, stopButton);

        if (pauzeButton) {
            pauzeButton.addEventListener("click", () => {
                updateRunningFlow(Number(idCell.value), 1);
                statusCell.value = "Pauzed";
                setStatusRunningFlowLayout(statusCell, pauzeButton, startButton, stopButton);
            })

        }
        if (startButton) {
            startButton.addEventListener("click", () => {
                updateRunningFlow(Number(idCell.value), 0);
                statusCell.value = "Open";
                setStatusRunningFlowLayout(statusCell, pauzeButton, startButton, stopButton);
            })

        }
        if (stopButton) {
            stopButton.addEventListener("click", () => {
                updateRunningFlow(Number(idCell.value), 2);
                statusCell.value = "Closed";

                if (closedRunningFlowTbody) {
                    const newRow = document.createElement('tr');
                    newRow.innerHTML = `
                                        <td>${closedRunningFlowTbody.childElementCount + 1}</td>
                                        <td>${timeCell.value}</td>
                                        <td>${statusCell.value}</td>
                                        <td></td>
                                        `;
                    closedRunningFlowTbody.appendChild(newRow)
                }
                setStatusRunningFlowLayout(statusCell, pauzeButton, startButton, stopButton);
                runningFlowRow.remove();
            })
            
        }
    });
});

function setStatusRunningFlowLayout(statusInput: HTMLInputElement, pauzeButton: HTMLButtonElement, startButton: HTMLButtonElement, stopButton: HTMLButtonElement) {
    if (statusInput) {
        const status = statusInput.value;

        switch (status) {
            case "Open":
                pauzeButton.style.display = "inline-block";
                startButton.style.display = "none";
                break;
            case "Pauzed":
                pauzeButton.style.display = "none";
                startButton.style.display = "inline-block";
                break;
            case "Closed":
                pauzeButton.style.display = "none";
                startButton.style.display = "none";
                stopButton.style.display = "none";
                break;
            default:
                console.error("Not excisting status!")
                break;
        }
    }

}


import $ from 'jquery';

document.addEventListener('DOMContentLoaded', () => {
    const table = $('#closedRunningFlowTable');
    const toggleBtn = $('#toggleTableBtn');

    table.hide();
    updateToggleButtonText(table);

    toggleBtn.on('click', () => {
        table.toggle();
        updateToggleButtonText(table);
    });
});

function updateToggleButtonText(table: JQuery<HTMLElement>) {
    const toggleBtn = $('#toggleTableBtn');

    if (table.is(':visible')) {
        toggleBtn.text('Hide Table');
    } else {
        toggleBtn.text('Show Table');
    }
}