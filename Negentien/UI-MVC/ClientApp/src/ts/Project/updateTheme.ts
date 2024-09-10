import {Theme} from "../models/Theme.interface";

export function addEventListenerToUpdateSubButtons() {
    const editSubThemes = document.querySelectorAll('[id^="b"]');

    if (editSubThemes) {
        editSubThemes.forEach(button => {
            button.addEventListener("click", (event) => {
                updateSubTheme(event);
            });
        });
    }
}

export function addEventListenerToUpdateHeadButtons() {
    const editHeadTheme = document.getElementById("updateHeadTheme")
    
    if (editHeadTheme) {
        editHeadTheme.addEventListener("click", (event) => {
            updateHeadTheme("headthemeName", "headthemeNameInfo");
        })
    }
}

function updateHeadTheme(themeNameId: string, infoId: string) {
    const updateThemeDto =
        gatherInputTheme("headthemeName", "headthemeNameInfo", true);

    if (updateThemeDto === null) {
        return
    }


    fetch("/api/Themes", {
        method: "PUT", headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        }, body: JSON.stringify(updateThemeDto)
    }).then(
        response => {
            if (!response.ok) {
                alert("update failed: " + response.status)
            } else {
                return response.json()
            }
        }
    ).then(data => {
        changeContentPage(themeNameId, infoId, data as Theme)
    }).catch(() => {
        alert("Oeps, something went wrong!")
    })
}


function getProjectId(): string {
    const urlSearchParams = new URLSearchParams(window.location.search);
    let projectId: string | null = urlSearchParams.get("id");

    if (!projectId) {
        const path: string = window.location.pathname;
        projectId = path.substring(path.lastIndexOf("/") + 1);
    }

    return projectId || "0";
}

function gatherInputTheme(themeNameId: string, infoId: string, isHeadTheme: boolean = false) {
    const themeNameElement = <HTMLInputElement>document.getElementById(themeNameId);
    const ShortInfoElement = <HTMLInputElement>document.getElementById(infoId);

    if (themeNameElement.value === themeNameElement.placeholder &&
        ShortInfoElement.value === ShortInfoElement.placeholder) {

        return null
    } else if (themeNameElement.value === "" &&
        ShortInfoElement.value === "") {
        return null
    }

    const projectId = getProjectId();
    const oldName = themeNameElement.placeholder;
    const newName = themeNameElement.value;

    return {
        "ProjectId": projectId,
        "ShortInformation": ShortInfoElement.value,
        "OldThemeName": oldName,
        "NewThemeName": newName,
        "IsHeadTheme": isHeadTheme
    }
}

export function updateSubTheme(event: Event) {
    const buttonId = (event.target as Element).id;
    const buttonIndex = buttonId.split("b")[1];
    const themeNameId = `i${buttonIndex}`;
    const infoId = `s${buttonIndex}`;

    const updateThemeDto =
        gatherInputTheme(themeNameId, infoId) // optionele parameter staat op false

    if (updateThemeDto === null) {
        return
    }


    fetch("/api/Themes", {
        method: "PUT", headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        }, body: JSON.stringify(updateThemeDto)
    }).then(
        response => {
            if (!response.ok) {
                if (response.status == 400) {
                    alert("update failed do to wrong input:\nDo not use a Theme-name that " +
                        "already exist in project")
                    return
                } else {
                    alert("update failed: " + response.status)
                }
            } else {
                return response.json()
            }
        }
    ).then(data => {
        changeContentPage(themeNameId, infoId, data as Theme)
    }).catch(() => {
        alert("Oeps, something went wrong!")
    })
}

function changeContentPage(themeNameId: string, infoId: string, subTheme: Theme) {

    const themeNameElement = <HTMLInputElement>document.getElementById(themeNameId);
    const ShortInfoElement = <HTMLInputElement>document.getElementById(infoId);
    themeNameElement.placeholder = subTheme.themeName;
    ShortInfoElement.placeholder = subTheme.shortInformation;
} 

