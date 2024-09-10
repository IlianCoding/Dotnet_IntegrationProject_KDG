import {getIdPartFromUrl} from "../getIdPartFromUrl";

const removeNoteButton = document.getElementById('removeNoteButton') as HTMLButtonElement

function removeNote() {
    console.log("Zit nu in de removeNote functie")
    console.log("noteId: " + getIdPartFromUrl("noteId"))
    fetch(`../../api/Notes/${getIdPartFromUrl('noteId')}/delete`, {
        method: "DELETE"
    }).then(response => {
        console.log(response)
        if (!response.ok) {
            throw new Error("Delete not dismissed: " + response.statusText)
        }
        return response.json();
    }).then(() => {
        window.location.href="/Flow/AttendantFlowDetail/1";
    }).catch(error => {
        console.error(error)
    });
}

removeNoteButton.addEventListener("click",()=>{
    console.log("RemoveButton is clicked!")
    removeNote();
})