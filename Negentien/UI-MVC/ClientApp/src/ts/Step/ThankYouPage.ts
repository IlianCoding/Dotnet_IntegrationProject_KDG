import "../../scss/thankYouPageStyling.scss"
import {getIdPartFromUrl} from "../getIdPartFromUrl";

const updateButton : HTMLButtonElement = document.getElementById('updateButton') as HTMLButtonElement
const alreadyButton : HTMLButtonElement = document.getElementById('alreadyButton') as HTMLButtonElement
const dontButton : HTMLButtonElement = document.getElementById('dontButton') as HTMLButtonElement
const flowIdThankPage : HTMLInputElement = document.getElementById('flowIdThankPage') as HTMLInputElement;

if (updateButton){
    updateButton.addEventListener("click", () => {
        window.location.href = `/EndUser/FillInContact?runningFlowId=${getIdPartFromUrl("runningFlowId")}`;
    }) 
}

if (alreadyButton){
    alreadyButton.addEventListener("click", () => {
        window.location.href = `/EndUser/LinkFinishedSessionToExistingUser?runningFlowId=${getIdPartFromUrl("runningFlowId")}`;
    })  
}


if (dontButton){
    dontButton.addEventListener("click", () => {
        window.location.href = `/SurveyHome/IndexSurveyHome?runningFlowId=${getIdPartFromUrl("runningFlowId")}&flowId=${Number(flowIdThankPage.value)}`;
    })
}

document.addEventListener('keydown', function (event) {
    if (['KeyG'].includes(event.code)) {
        console.log("You pressed G!");
        window.location.href = `/SurveyHome/IndexSurveyHome?runningFlowId=${getIdPartFromUrl("runningFlowId")}&flowId=${Number(flowIdThankPage.value)}`;
    }
});