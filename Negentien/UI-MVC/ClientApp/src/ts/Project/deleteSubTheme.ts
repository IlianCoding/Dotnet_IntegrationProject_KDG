
export function addEventListenerToDeleteButtons() {
    const editSubThemes = document.querySelectorAll('button[id^="d"]');
    
    if (editSubThemes) {
        editSubThemes.forEach(button => {
            button.addEventListener("click", (event: Event) => {

                deleteSubTheme(event);

            });
        });
    }
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

function gatherInputTheme(themeNameId: string) {
    const themeNameElement = <HTMLInputElement>document.getElementById(themeNameId);
    const projectId = getProjectId();
    const name = themeNameElement.placeholder || "";
    return {
        "ProjectId": projectId,
        "ThemeName": name
    }
}


export function deleteSubTheme(event: Event) {
    const buttonId = (event.target as Element).id;
    const buttonIndex = buttonId.split("d")[1];
    const themeNameId = `i${buttonIndex}`;

    const data = gatherInputTheme(themeNameId)

        fetch("/api/Themes", {
            method: "DELETE", headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            }, body: JSON.stringify(data)
        })
            .then(
                response => {
                    if (!response.ok) {
                        alert("Delete failed: " + response.status)
                    } else {
                        deleteSubThemeElements(buttonIndex)
                    }
                }
            )
            .catch(() => {
                alert("Oeps, something went wrong!")
            })

}

function deleteSubThemeElements(buttonIndex: string) {
    const elementToRemove = <HTMLElement>document.getElementById(`div${buttonIndex}`);

    elementToRemove.remove();
}
