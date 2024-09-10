import {Theme} from "../models/Theme.interface";
import {addEventListenerToUpdateHeadButtons, addEventListenerToUpdateSubButtons, updateSubTheme} from "./updateTheme";
import {addEventListenerToDeleteButtons, deleteSubTheme} from "./deleteSubTheme";

addEventListenerToUpdateHeadButtons()
addEventListenerToUpdateSubButtons()
addEventListenerToDeleteButtons()

const toevoegenSubThemaButton = document.getElementById("toevoegenSubThema")
if (toevoegenSubThemaButton) {
    toevoegenSubThemaButton.addEventListener("click",
        (event) => {
            toevoegenSubThema();
        })
}

function collectSubThemeData() {
    const urlSearchParams = new URLSearchParams(window.location.search);
    const shortInfoElement = <HTMLInputElement>document.getElementById("shortInfo")
    const themeNameElement = <HTMLInputElement>document.getElementById("themeName")

    let projectId: string | null = urlSearchParams.get("id");

    if (!projectId) {
        const path: string = window.location.pathname;
        projectId = path.substring(path.lastIndexOf("/") + 1);
    }
    projectId = projectId || "0";

    const shortInfo: string | null = shortInfoElement.value;
    const themeName: string | null = themeNameElement.value;

    return {
        "ProjectId": projectId,
        "ShortInformation": shortInfo,
        "ThemeName": themeName,
        "IsHeadTheme": false
    }
}

function toevoegenSubThema() {
    if ((<HTMLInputElement>document.getElementById("themeName")).value.trim().length === 0) {
        alert("Can't create a SubTheme without a name!")
        return
    }


    const subTheme = collectSubThemeData();
    fetch("/api/Themes", {
        method: "POST", headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        }, body: JSON.stringify(subTheme)
    }).then(
        response => {
            if (!response.ok) {
                if (response.status == 400) {
                    alert("Failed to add subThemes to project do to wrong input:\nDo not use a Theme-name that " +
                        "already exist in project")
                } else {
                    alert("Failed to add subThemes to project: " + response.status)
                }
            
            } else {
                return response.json();
            }
        }
    ).then(data => {
        insertSubthemeDataToPage(data as Theme)
    }).catch(() => {
        alert("Oeps, something went wrong!")
    })
}

function insertSubthemeDataToPage(subThema: Theme) {
    const subThemaList = <HTMLElement>document.querySelector("#subThemas");
    const idElement = getIdFromLastElement();

    subThemaList.insertAdjacentHTML('beforeend', `<div id="div${idElement}" >
                <input id="i${idElement}"  placeholder="${subThema.themeName}" />
                <input id="s${idElement}"  placeholder="${subThema.shortInformation}" />
                 <button id="b${idElement}" class="btn btn-primary" >Update subthema</button>
                 <button id="d${idElement}" class="btn btn-primary" >Delete subthema</button>
            </div>`);

    const inputEl = <HTMLElement>document.getElementById("i"+idElement);
    inputEl.style.color = 'grey';
    const discEl = <HTMLElement>document.getElementById("s"+idElement);
    discEl.style.color = 'grey';
    attachEventListeners("b" + idElement, "d" + idElement);
}

function attachEventListeners(updateButtonId: string, deleteButtonId: string) {
    const updateButton = <HTMLElement>document.getElementById(updateButtonId);
    if (updateButton) {
        updateButton.addEventListener("click", ev => {
            updateSubTheme(ev)
        })
    }

    const deleteButton = <HTMLElement>document.getElementById(deleteButtonId);
    if (deleteButton) {
        deleteButton.addEventListener("click", ev => {
            deleteSubTheme(ev)
        })
    }
}

function getIdFromLastElement(): number {
    const subThemaList = <HTMLElement>document.querySelector("#subThemas");

    if (subThemaList) {
        const lastChild = subThemaList.lastElementChild;
        if (lastChild) {
            const id = parseInt(lastChild.id.split("div")[1])
            return id + 1;
        }
    }

    return -1
}
document.addEventListener('DOMContentLoaded', function() {
    console.log("zit in de functieeee!!")
    const inputs = document.querySelectorAll('.form-control') as NodeListOf<HTMLInputElement>;
    inputs.forEach(input => {
        input.addEventListener('focus', function() {
            this.style.color = 'black';
        });
        input.addEventListener('blur', function() {
            this.style.color = 'grey';
        });
    });
});

