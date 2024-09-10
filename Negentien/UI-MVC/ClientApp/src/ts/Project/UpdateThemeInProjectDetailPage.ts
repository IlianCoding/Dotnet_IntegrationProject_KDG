import {isInputRequiredMinLength, spanMessageByInput} from "../validateName";

const headThemeId = document.getElementById("headThemeId") as HTMLInputElement
const editHeadthemeName = document.getElementById("editHeadthemeName") as HTMLInputElement
const editThemeNameSpan = document.getElementById("editThemeNameSpan") as HTMLSpanElement

const editThemeInformation = document.getElementById("editThemeInformation") as HTMLInputElement
const editThemeDescriptionSpan = document.getElementById("editThemeDescriptionSpan") as HTMLInputElement

const confirmThemeButton = document.getElementById("confirmThemeButton") as HTMLButtonElement

const detailThemeSpan = document.getElementById("detailThemeSpan") as HTMLSpanElement
const cetailThemeDescriptionSpan = document.getElementById("cetailThemeDescriptionSpan") as HTMLSpanElement

const editThemeButton = document.getElementById("editThemeButton") as HTMLButtonElement
const detailTheme = document.getElementById("detailTheme") as HTMLDivElement
const editTheme = document.getElementById("editTheme") as HTMLDivElement

function validateTheme() {
    console.log("validateTheme")
    if (!isInputRequiredMinLength(editHeadthemeName)) {
        spanMessageByInput(editHeadthemeName, editThemeNameSpan)
    }
    if (!isInputRequiredMinLength(editThemeInformation)) {
        spanMessageByInput(editThemeInformation, editThemeDescriptionSpan)
    }
    return !(!isInputRequiredMinLength(editHeadthemeName) || !isInputRequiredMinLength(editThemeInformation))
}

function updateHeadTheme() {
    console.log("updateHeadTheme")
    if (!validateTheme()) {
        console.log("validateTheme")
        console.log(validateTheme())
        return;
    }
    fetch(`/api/Themes/${Number(headThemeId.value)}/update`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            "NewThemeName": editHeadthemeName.value,
            "ShortInformation": editThemeInformation.value,
            "IsHeadTheme": true,
        })
    }).then(response => {
        if (!response.ok) {
            console.error("Update head theme doesn' work: " + response.statusText)
        }
        detailThemeSpan.textContent = editHeadthemeName.value;
        cetailThemeDescriptionSpan.textContent = editThemeInformation.value;
        editTheme.style.display = "none"
        detailTheme.style.display = "block"
    }).catch(error => {
        console.error(error)
    })
}
document.addEventListener("DOMContentLoaded", ()=>{
    detailTheme.style.display="block";
    editTheme.style.display = "none";
})
editThemeButton.addEventListener("click", ()=>{
    detailTheme.style.display="none";
    editTheme.style.display = "block";
})

confirmThemeButton.addEventListener("click", () => {
    console.log("button clicked");
    updateHeadTheme();
})

editHeadthemeName.addEventListener("blur", () => spanMessageByInput(editHeadthemeName, editThemeNameSpan));
editThemeInformation.addEventListener("blur", () => spanMessageByInput(editThemeInformation, editThemeDescriptionSpan));
