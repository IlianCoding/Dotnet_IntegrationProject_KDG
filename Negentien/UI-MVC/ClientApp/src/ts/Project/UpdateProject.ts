import * as project from "./Project"
import {getIdOfCurrentHtml} from "../getIdOfCurrentHtml"
import {setRadioIsActiveByInputs, setSpanByIsactive} from "./Project";
import {isInputRequiredMinLength, spanMessageByInput} from "../validateName";

const radioTrue = document.getElementById('radioTrue1') as HTMLInputElement;
const radioFalse = document.getElementById('radioFalse1') as HTMLInputElement;
const changeProjectStatusButton = document.getElementById('changeProjectStatus') as HTMLButtonElement
const projectNameInput = document.getElementById('projectName') as HTMLInputElement;
const projectNameSpan = document.getElementById('projectNameSpan') as HTMLSpanElement;
const updateprojectbutton = document.getElementById('updateProject') as HTMLButtonElement;
const editDiv = document.getElementById('editDiv') as HTMLElement
const detailDiv = document.getElementById('detailDiv') as HTMLElement
const editButton = document.getElementById('editProjectButton') as HTMLButtonElement
const projectNameDetailSpan = document.getElementById('projectNameDetailSpan') as HTMLSpanElement
const projectInformationSpan = document.getElementById('projectInformationDetailSpan') as HTMLSpanElement
const projectStatusDetailSpan = document.getElementById('projectStatusDetailSpan') as HTMLSpanElement
const fontDetailSpan = document.getElementById('fontDetailSpan') as HTMLSpanElement
const primaryColorDetailSpan = document.getElementById('primaryColorDetailSpan') as HTMLSpanElement
const projectInformation = document.getElementById('projectInformation') as HTMLInputElement;
const primaryColorSelect = document.getElementById('primaryColor') as HTMLSelectElement;
const fontSelect = document.getElementById('font') as HTMLSelectElement;
const projectNameHeader = document.getElementById('projectNameHeader') as HTMLHeadElement;
const ModalBody = document.getElementById('ModalBody') as HTMLElement;
const noteWarning = document.getElementById('noteWarning') as HTMLElement;

let status: boolean;
let projectName: string;

async function beginLayoutProject() {
    const response = await fetch(`/api/Projects/${getIdOfCurrentHtml()}`, {
        headers: {
            Accept: "application/json"
        }
    })
    if (response.status === 200) {
        const project = await response.json()
        status = project.isActive;
        projectName = project.name;
        primaryColorSelect.value = project.primaryColor;
        fontSelect.value = project.font;
        setRadioIsActiveByInputs(status, radioTrue, radioFalse)
        setSpanByIsactive(status, projectStatusDetailSpan);
    } else {
        console.error("fetch project not work!");
    }
}

function validateProject(name: HTMLInputElement): boolean {
    return isInputRequiredMinLength(name);
}

async function updateProject(name: HTMLInputElement, isActive: boolean) : Promise<void> {
    if (!validateProject(name)) {
        return;
    }
    try {
        const response : Response = await fetch(`/api/Projects/${getIdOfCurrentHtml()}/update`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                "name": projectNameInput.value,
                "isActive": isActive,
                "projectInformation": projectInformation.value,
                "primaryColor": primaryColorSelect.value,
                "font": fontSelect.value
            })
        })
        if (response.status === 202) {
            editDiv.style.display = "none";
            detailDiv.style.display = "block";
            projectName = projectNameInput.value;
            status = isActive
            projectNameDetailSpan.textContent = projectNameInput.value;
            projectNameHeader.textContent = projectNameInput.value;
            projectInformationSpan.textContent = projectInformation.value;
            fontDetailSpan.textContent = fontSelect.value;
            primaryColorDetailSpan.textContent = primaryColorSelect.value;
            project.setRadioIsActiveByInputs(status, radioTrue, radioFalse)
            project.setSpanByIsactive(status, projectStatusDetailSpan)
            setFlowStatus(isActive)
        } else {
            console.error(`Update project failed: ${response.status}`);
        }
    } catch (error) {
        console.error("Error creating project:", error);
    }
}

async function updateProjectStatus(name: HTMLInputElement, isActive: boolean) : Promise<void> {
    if (!validateProject(name)) {
        return;
    }
    try {
        const response : Response = await fetch(`/api/Projects/${getIdOfCurrentHtml()}/updateStatus`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                "name": projectNameInput.value,
                "isActive": isActive,
            })
        })
        if (response.status === 202) {
            editDiv.style.display = "none";
            detailDiv.style.display = "block";
            status = isActive
            project.setRadioIsActiveByInputs(status, radioTrue, radioFalse)
            project.setSpanByIsactive(status, projectStatusDetailSpan)
            setFlowStatus(isActive)
        } else {
            console.error(`Update project failed: ${response.status}`);
        }
    } catch (error) {
        console.error("Error creating project:", error);
    }
}
function showWarningDive(status:boolean){
    noteWarning.style.display = (status? "none":"block")
}

async function fetchAttendants() : Promise<void> {
    const response : Response = await fetch(`/api/Projects/${getIdOfCurrentHtml()}/attendants`, {
        headers: {
            Accept: "application/json"
        }
    });

    const attendantsTableBody : HTMLElement | null = document.getElementById('attendantsTableBody');
    if (response.status === 200 && attendantsTableBody) {
        const attendants = await response.json();
        if (attendants.length > 0) {
            attendantsTableBody.innerHTML = attendants.map((attendant: { firstname: string, lastname: string, username: string, birthDay: string }) => {
                const birthDate : Date = new Date(attendant.birthDay);
                const formattedDate : string = `${String(birthDate.getDate()).padStart(2, '0')}-${String(birthDate.getMonth() + 1).padStart(2, '0')}-${birthDate.getFullYear()}`;

                return `
                    <tr>
                        <td>${attendant.firstname}</td>
                        <td>${attendant.lastname}</td>
                        <td>${attendant.username}</td>
                        <td>${formattedDate}</td>
                    </tr>
                `;
            }).join('');
        } else {
            attendantsTableBody.innerHTML = '<tr><td colspan="4">No attendants found.</td></tr>';
        }
    } else {
        if (attendantsTableBody) {
            attendantsTableBody.innerHTML = '<tr><td colspan="4">Failed to load attendants.</td></tr>';
        }
    }
}
document.getElementById('pills-attendants-tab')?.addEventListener('click', fetchAttendants);


function modalProjectDetailLayout() {
    ModalBody.textContent =`Are you certain you wish to transition the status to '${((!status) ? "Active" : "Inactive")}'?\n This action will also set all associated flows to '${((!status) ? "Open" : "Closed")}'`
}

function changeProjectStatus() {
    status = !status
    modalProjectDetailLayout()
    updateProjectStatus(projectNameInput, status);
}

window.addEventListener('DOMContentLoaded', beginLayoutProject)
if (updateprojectbutton) {
    projectNameInput.addEventListener("blur", () => spanMessageByInput(projectNameInput, projectNameSpan));
    updateprojectbutton.addEventListener("click", () : void => {
        updateProject(projectNameInput, project.getStatus(radioTrue, radioFalse))
    })
}
window.addEventListener('DOMContentLoaded', async () => {
    await beginLayoutProject();
    showWarningDive(status);
    modalProjectDetailLayout();
    editDiv.style.display = "none";
});

if (editButton) {
}
editButton.addEventListener("click", ():void => {
    showWarningDive(status)
    editDiv.style.display = "block"
    detailDiv.style.display = "none"
})

if (changeProjectStatusButton) {
    changeProjectStatusButton.addEventListener("click", ()=>{
        changeProjectStatus();
        showWarningDive(status);
    })
}

function setFlowStatus(isActive: boolean): void {
    const stateCells : NodeListOf<Element> = document.querySelectorAll('table.table-striped tbody tr td:nth-child(2)');
    const statusText : "Open" | "Closed" = isActive ? 'Open' : 'Closed';
    stateCells.forEach((cell : Element) : void => {
        cell.textContent = statusText;
    });
}
document.getElementById('pills-attendants-tab')?.addEventListener('click', fetchAttendants);