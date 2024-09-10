import * as project from "./Project"
import {getIdOfUrl} from "../getIdOfUrl";
import {isInputRequiredMinLength, spanMessageByInput} from "../validateName";

const projectNameSpan = document.getElementById('projectNameSpan') as HTMLSpanElement;
const headThemeSpan = document.getElementById('headThemeSpan') as HTMLSpanElement;
const headThemeDescriptionSpan = document.getElementById('headThemeDescriptionSpan') as HTMLSpanElement;
const projectNameInput = document.getElementById('projectName') as HTMLInputElement
const radioTrue = document.getElementById('radioTrue') as HTMLInputElement;
const radioFalse = document.getElementById('radioFalse') as HTMLInputElement;
const addProjectButton = document.getElementById('addProjectButton') as HTMLButtonElement;
const headThemeNameInput = document.getElementById('headThemeName') as HTMLInputElement
const headThemeDescriptionInput = document.getElementById('headThemeDescription') as HTMLInputElement
const projectInformation = document.getElementById('projectInformation') as HTMLInputElement;


async function addNewProject(): Promise<void> {
    if (!validateProject()) {
        return;
    }

    const projectData = {
        name: projectNameInput.value,
        isActive: project.getStatus(radioTrue, radioFalse),
        projectInformation: projectInformation.value
    };
    
    console.log(projectData);

    try {
        const response = await fetch('/api/Projects', {
            method: "POST",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json",
            },
            body: JSON.stringify(projectData),
        });
        if (response.status === 201) {
            const urlProject = response.headers.get("location");
            const projectId = getIdOfUrl(urlProject);
            await addHeadThemeToProject(projectId);
        } else {
            console.error(`Create project failed: ${response.status}`);
        }
    } catch (error) {
        console.error("Error creating project:", error);
    }
}

async function addHeadThemeToProject(id: number) {
    const projectData = {
        ProjectId: id,
        ThemeName: headThemeNameInput.value,
        ShortInformation: headThemeDescriptionInput.value,
        isHeadTheme: true
    };
    try {
        const response = await fetch("/api/Themes", {
            method: "POST",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(projectData)
        });
        if (response.status === 201) {
            window.location.href = "/Organization/ProjectOversight";
        } else {
            console.error("Create Theme failed: response")
        }
    } catch (error) {
        console.error("Error creating theme:", error);
    }


}


function validateProject(): boolean {
    const isValidProjectName = isInputRequiredMinLength(projectNameInput);
    const isValidThemeName = isInputRequiredMinLength(headThemeNameInput);
    const isValidThemeDescription = isInputRequiredMinLength(headThemeDescriptionInput);
    if (!isValidProjectName) {
        spanMessageByInput(projectNameInput, projectNameSpan)
    }
    if (!isValidThemeName ){
        spanMessageByInput(headThemeNameInput, headThemeSpan)
    }
    if (!isValidThemeDescription){
        spanMessageByInput(headThemeDescriptionInput, headThemeDescriptionSpan)
    }
    return !(!isValidProjectName || !isValidThemeName || !isValidThemeDescription);
}

projectNameInput.addEventListener("blur", () => spanMessageByInput(projectNameInput, projectNameSpan));
// projectNameInput.addEventListener("input", () => spanMessageByInput(projectNameInput, projectNameSpan));

headThemeNameInput.addEventListener("blur", () => spanMessageByInput(headThemeNameInput, headThemeSpan));
// headThemeNameInput.addEventListener("input", () => spanMessageByInput(headThemeNameInput, headThemeSpan));

headThemeDescriptionInput.addEventListener("blur", () => spanMessageByInput(headThemeDescriptionInput, headThemeDescriptionSpan));
// headThemeDescriptionInput.addEventListener("input", () => spanMessageByInput(headThemeDescriptionInput, headThemeDescriptionSpan));

addProjectButton.addEventListener("click", addNewProject);
