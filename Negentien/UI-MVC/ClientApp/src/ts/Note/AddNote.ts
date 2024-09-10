import {isInputRequiredMinLength, spanMessageByInput} from "../validateName";

const noteTitleInput = document.getElementById('noteTitleInput') as HTMLInputElement
const noteTitleSpan = document.getElementById('noteTitleSpan') as HTMLSpanElement
const noteTextInput = document.getElementById('noteTextInput') as HTMLInputElement
const noteTextSpan = document.getElementById('noteTextSpan') as HTMLSpanElement
const attendantNameInput = document.getElementById('attendantNameInput') as HTMLInputElement
const addNoteButton = document.getElementById('addNoteButton') as HTMLButtonElement
const noteStepSelect = document.getElementById('noteStepSelect') as HTMLSelectElement
const noteDiv = document.getElementById('noteDiv') as HTMLDivElement
const noteWarning = document.getElementById('noteWarning') as HTMLDivElement

function validateNote(): boolean {
    const isValidNoteTitleInput = isInputRequiredMinLength(noteTitleInput);
    const isValidNoteTextInput = isInputRequiredMinLength(noteTextInput);
    const isValidAttendantNameInput = isInputRequiredMinLength(attendantNameInput);
    if (!isValidNoteTitleInput) {
        spanMessageByInput(noteTitleInput, noteTitleSpan)
    }
    if (!isValidNoteTextInput) {
        spanMessageByInput(noteTextInput, noteTextSpan)
    }
    return !(!isValidNoteTitleInput || !isValidNoteTextInput || !isValidAttendantNameInput);
}

function addNote() {
    if (!validateNote()) {
        return;
    }
    fetch('../../api/Notes', {
        method: "POST",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
        body: JSON.stringify({
            "StepId": Number(noteStepSelect.value),
            "NoteTitle": noteTitleInput.value,
            "NoteText": noteTextInput.value,
            "CreatedAttendantName": attendantNameInput.value
        }),
    }).then(response => {
        if (!response.ok) {
            return new Error("Post note didn't work!: " + response.statusText)
        }
        return response.json();
    }).then(noteDto => {
        noteTitleInput.value = "";
        noteTextInput.value = "";
        noteWarning.style.display= "none"
        toTabAllNote();
        addElementLayoutToNotes(noteDto.id, noteDto.noteTitle, noteDto.noteText)
    }).catch(error => {
        console.error(error)
    });
}

const buttonToAllNotes = document.getElementById('pills-noted-tab')

function toTabAllNote() {
    const allNotesTab = document.querySelector('#pills-noted') as HTMLElement;
    const allTabs = document.querySelectorAll('.nav-link');
    const allPanes = document.querySelectorAll('.tab-pane');
    const allPanesContent = document.querySelectorAll('.tab-pane');

    allTabs.forEach(tab => {
        tab.classList.remove('active');
    });
    buttonToAllNotes?.classList.add('active');
    allPanes.forEach(pane => {
        pane.classList.remove('active');
    });
    allNotesTab.classList.add('active');
    allPanesContent.forEach(pane => {
        pane.classList.remove('show');
    });
    allNotesTab.classList.add('show');
}

function addElementLayoutToNotes(noteId: number, title: string, text: string) {
    noteDiv.innerHTML += `
    <section class="noteSection">
        <input type="checkbox" id="@toggleId" class="note-toggle">
        <label for="@toggleId" class="note-title">${title}</label>
        <section class="note-text">
            <p >${text}</p>
            <a href="/Note/NoteDetail?noteId=${noteId}" class="text-muted" >Edit</a>
            </section>
    </section>
    `;
}

addNoteButton.addEventListener("click", addNote)
noteTitleInput.addEventListener("blur", () => spanMessageByInput(noteTitleInput, noteTitleSpan));
noteTextInput.addEventListener("blur", () => spanMessageByInput(noteTextInput, noteTextSpan));