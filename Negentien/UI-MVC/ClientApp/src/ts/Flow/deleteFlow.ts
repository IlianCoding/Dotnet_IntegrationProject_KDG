import {getIdOfCurrentHtml} from "../getIdOfCurrentHtml";


export function addDeleteEventListener() {
    const deleteBut = document.getElementById("delete-flow") as HTMLElement
    deleteBut.addEventListener("click", deleteFlowSequence)
}

function deleteFlowSequence(){
    getAlert()
}
function getAlert(){
    const alertElement= document.getElementById("alert-delete") as HTMLElement
    alertElement.innerHTML = `
    <div class="float-none alert alert-warning alert-dismissible fade 
show shadow-lg  justify-content-center align-items-center my-5" role="alert">
        <div class="text-center">
            <strong>Warning!</strong> Deleting the flow will also delete all associated objects, including questions and their answers
        </div>
        <div class="mt-3 d-flex justify-content-center align-items-center">
            <button class="btn btn-light border-black border-1 mx-3" id="yes-but">Yes</button>
            <button class="btn btn-light border-black border-1 mx-3"  id="no-but">No</button>
        </div>
    </div>                
            `;

    addButtonsEventListener()
    const page = document.getElementById("flow-page") as HTMLElement
    page.classList.add("opacity-50")
    disableElementAndChildren(page)
}

function deleteFlow() {
    const id = getIdOfCurrentHtml()
    console.log(id)
   
    fetch("/api/Flows/" + id, {
        method: "DELETE", headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        }
    })
        .then(
            response => {
                if (!response.ok) {
                    alert("Delete failed: " + response.status)
                } else {
                 return  response.json() 
                }
            }
        )
        .then(data => {
        window.location.href = "/Project/ProjectDetail/" + data.id
    })
        .catch(() => {
            alert("Oeps, something went wrong!")
        })
}



function addButtonsEventListener(){
        const yesButton = document.getElementById("yes-but");
        if (yesButton) {
            yesButton.addEventListener("click", deleteFlow);
        }

        const noButton = document.getElementById("no-but");
        if (noButton) {
            noButton.addEventListener("click", handleNoButtonClick);
        }
}

function handleNoButtonClick(){
    const alertElement= document.getElementById("alert-delete") as HTMLElement
    alertElement.innerHTML = ``;
    const page = document.getElementById("flow-page") as HTMLElement
    page.classList.remove("opacity-50")
    enableElementAndChildren(page)
}

function enableElementAndChildren(page: HTMLElement){
    page.removeAttribute("disabled");
    
    page.classList.remove("disabled");
    
    const children = page.children;
    for (let i = 0; i < children.length; i++) {
        enableElementAndChildren(children[i] as HTMLElement);
    }
}


function disableElementAndChildren(element: HTMLElement) {
    element.setAttribute("disabled", "true"); 
    
    element.classList.add("disabled");
    
    const children = element.children;
    for (let i = 0; i < children.length; i++) {
        disableElementAndChildren(children[i] as HTMLElement);
    }
}
