import {getIdPartFromUrl} from "../getIdPartFromUrl";
import {isInputRequiredMinLength, spanMessageByInput} from "../validateName";

const editNote = document.getElementById('editNote') as HTMLDivElement
const detailNote = document.getElementById('detailNote') as HTMLDivElement
const confirmNoteButton = document.getElementById('confirmNoteButton') as HTMLButtonElement
const editNoteButton = document.getElementById('editNoteButton') as HTMLButtonElement
const editNoteTitle = document.getElementById('editNoteTitle') as HTMLInputElement
const editNoteText = document.getElementById('editNoteText') as HTMLInputElement
const detailNoteTitleSpan = document.getElementById('detailNoteTitleSpan') as HTMLSpanElement;
const detailNoteTextSpan = document.getElementById('detailNoteTextSpan') as HTMLSpanElement
const noteTitleErrorSpan = document.getElementById('noteTitleErrorSpan') as HTMLSpanElement
const noteTextErrorSpan = document.getElementById('noteTextError') as HTMLSpanElement

function validateNoteDetailPage(): boolean {
    const isValidEditNoteTitle = isInputRequiredMinLength(editNoteTitle);
    const isValidEditNoteText = isInputRequiredMinLength(editNoteText);
    if (!isValidEditNoteTitle) {
        spanMessageByInput(editNoteTitle, noteTitleErrorSpan)
    }
    if (!isValidEditNoteText) {
        spanMessageByInput(editNoteText, noteTextErrorSpan)
    }
    return !(!isValidEditNoteTitle || !isValidEditNoteText);
}

function updateNotePage() {
    if (!validateNoteDetailPage()) {
        return;
    }
    fetch(`../../api/Notes/${getIdPartFromUrl("noteId")}/update`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            "NoteTitle": editNoteTitle.value,
            "NoteText": editNoteText.value
        })
    }).then(response => {
        if (!response.ok) {
            throw new Error("Update not discharge: " + response.statusText);
        }
    }).then(() => {
        detailNoteTitleSpan.textContent = editNoteTitle.value;
        detailNoteTextSpan.textContent = editNoteText.value
        detailNote.style.display = "block";
        editNote.style.display = "none";
    }).catch(error =>{
        console.error(error)
    })
}

window.addEventListener("DOMContentLoaded", () => {
    editNote.style.display = "none";
    detailNote.style.display = "block";
})
editNoteButton.addEventListener("click", () => {
    editNote.style.display = "block";
    detailNote.style.display = "none";
})
confirmNoteButton.addEventListener("click", () => {
    updateNotePage();
})
editNoteTitle.addEventListener("blur", () => spanMessageByInput(editNoteTitle, noteTitleErrorSpan));
editNoteText.addEventListener("blur", () => spanMessageByInput(editNoteText, noteTextErrorSpan));
