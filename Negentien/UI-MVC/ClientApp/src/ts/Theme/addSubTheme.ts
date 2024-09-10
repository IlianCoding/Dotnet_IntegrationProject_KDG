const themeName = document.getElementById("themeName") as HTMLInputElement;
const shortInfo = document.getElementById("shortInfo") as HTMLInputElement;
const projectId = document.getElementById("projectId") as HTMLInputElement;
const toevoegenSubThema = document.getElementById("toevoegenSubThema") as HTMLButtonElement;
const subThemesTBody = document.getElementById("subThemesTBody") as HTMLBodyElement;

function addSubTheme() {
    fetch("/api/Themes", {
        method: "POST", headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        }, body: JSON.stringify({
            "ProjectId": Number(projectId.value),
            "ShortInformation": shortInfo.value,
            "ThemeName": themeName.value,
            "IsHeadTheme": false
        })
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
        console.log(data)
        subThemesTBody.innerHTML += `
        <tr>
            <td>${data.themeName}</td>
            <td>${data.shortInformation}</td>
            <td>
                <a href="/Theme/ThemeDetail?projectId=${Number(projectId.value)}&themeId=${data.id}">Details</a>
            </td>
            <td> <button id="d@(teller)" class="btn btn-primary">Delete</button></td>
        </tr>
        `
        themeName.value = "";
        shortInfo.value = "";
    }).catch(() => {
        alert("Oeps, something went wrong!")
    })
}

toevoegenSubThema.addEventListener("click", () => {
    addSubTheme();
})