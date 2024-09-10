import {isInputRequiredMinLength, spanMessageByInput} from "../validateName";

const editDiv = document.getElementById("editDiv") as HTMLDivElement
const detailDiv = document.getElementById("detailDiv") as HTMLDivElement
const theme_id = document.getElementById("theme_id") as HTMLInputElement
const editThemeNameDetail = document.getElementById("editThemeNameDetail") as HTMLInputElement
const themeNameSpan = document.getElementById("themeNameSpan") as HTMLSpanElement
const editThemeInformationDetail = document.getElementById("editThemeInformationDetail") as HTMLInputElement
const themeInformationSpan = document.getElementById("themeInformationSpan") as HTMLSpanElement
const updateTheme = document.getElementById("updateTheme") as HTMLButtonElement
const editThemeButton = document.getElementById("editThemeButton") as HTMLButtonElement

const ThemeNameDetailSpan = document.getElementById("ThemeNameDetailSpan") as HTMLButtonElement
const ThemeInformationDetailSpan = document.getElementById("ThemeInformationDetailSpan") as HTMLButtonElement



function validateTheme() {
    console.log("validateTheme")
    if (!isInputRequiredMinLength(editThemeNameDetail)) {
        spanMessageByInput(editThemeNameDetail, themeNameSpan)
    }
    if (!isInputRequiredMinLength(editThemeInformationDetail)) {
        spanMessageByInput(editThemeInformationDetail, themeInformationSpan)
    }
    return !(!isInputRequiredMinLength(editThemeNameDetail) || !isInputRequiredMinLength(editThemeInformationDetail))
}

function updateHeadTheme() {
    console.log("updateHeadTheme")
    if (!validateTheme()) {
        console.log("validateTheme")
        console.log(validateTheme())
        return;
    }
    fetch(`/api/Themes/${Number(theme_id.value)}/update`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            "NewThemeName": editThemeNameDetail.value,
            "ShortInformation": editThemeInformationDetail.value,
            "IsHeadTheme": true,
        })
    }).then(response => {
        if (!response.ok) {
            console.error("Update head theme doesn' work: " + response.statusText)
        }
        ThemeNameDetailSpan.textContent = editThemeNameDetail.value;
        ThemeInformationDetailSpan.textContent = editThemeInformationDetail.value;
        editDiv.style.display = "none"
        detailDiv.style.display = "block"
    }).catch(error => {
        console.error(error)
    })
}
document.addEventListener("DOMContentLoaded", ()=>{
    detailDiv.style.display="block";
    editDiv.style.display = "none";
})
editThemeButton.addEventListener("click", ()=>{
    detailDiv.style.display="none";
    editDiv.style.display = "block";
})

updateTheme.addEventListener("click", () => {
    console.log("button clicked");
    updateHeadTheme();
})

editThemeNameDetail.addEventListener("blur", () => spanMessageByInput(editThemeNameDetail, ThemeNameDetailSpan));
editThemeInformationDetail.addEventListener("blur", () => spanMessageByInput(editThemeInformationDetail, ThemeInformationDetailSpan));
