import {getStatusInString} from "./Project"

const tableBody = document.getElementById('projectsTableBody') as HTMLBodyElement
const AddProjectButton = document.getElementById('AddProject') as HTMLButtonElement

class Theme {
    private _id: number;
    private _name: string;

    constructor(id: number, name: string) {
        this._id = id;
        this._name = name;
    }

    // Getter voor het attribuut 'id'
    get id(): number {
        return this._id;
    }

    // Getter voor het attribuut 'name'
    get name(): string {
        return this._name;
    }
}

let themes: Theme[] = [];
async function retrieveAllProjects() {
    await retrieveHeadThemes()
    const response = await fetch('/api/Projects', {
        headers: {
            Accept: 'application/json'
        }
    })
    if (response.status === 200) {
        tableBody.innerHTML = ''
        const projects = await response.json()
        for (const project of projects) {
            tableBody.innerHTML += `
        <tr>
        <td>${project.name}</td>
        <td>${getStatusInString(project.isActive)}</td>
        <td>${retrieveHeadThemeNameById(project.themeId)}</td>
        <td>
            <a style="color: black" href="/Project/ProjectDetail/${project.id}">Details</a>
        </td>
        </tr>`;
        }
    }
}
function retrieveHeadThemeNameById(id: number): string | null {
    for (const theme of themes) {
        if (theme.id === id) {
            return theme.name;
        }
    }
    return "Unknown";
}

async function retrieveHeadThemes() {
    const response = await fetch("/api/Themes/headThemes",
        {
            headers: {
                Accept: 'application/json'
            }
        })
    if (response.status === 200) {
        const headThemes = await response.json()
        for (const theme of headThemes) {
            themes.push(new Theme(theme.id, theme.name))
        }
    } else {
        console.error("Retrieve HeadTheme's not working")
        return "Unknown"
    }
}
function AddProjectRedirect(){
    window.location.href= "/Project/AddProject"
}

window.addEventListener('DOMContentLoaded', retrieveAllProjects)
AddProjectButton.addEventListener('click', AddProjectRedirect)