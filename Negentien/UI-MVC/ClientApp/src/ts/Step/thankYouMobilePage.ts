import "../../scss/thankYouPageStyling.scss"
import {getIdPartFromUrl} from "../getIdPartFromUrl";

const updateButton : HTMLButtonElement = document.getElementById('updateButton') as HTMLButtonElement
const alreadyButton : HTMLButtonElement = document.getElementById('alreadyButton') as HTMLButtonElement
const dontButtonMobile : HTMLButtonElement = document.getElementById('dontButtonMobile') as HTMLButtonElement
const flowIdThankPage : HTMLInputElement = document.getElementById('flowIdThankPage') as HTMLInputElement;

updateButton.addEventListener("click", () => {
    window.location.href = `/EndUser/FillInContact?runningFlowId=${getIdPartFromUrl("runningFlowId")}`;
})

alreadyButton.addEventListener("click", () => {
    window.location.href = `/EndUser/LinkFinishedSessionToExistingUser?runningFlowId=${getIdPartFromUrl("runningFlowId")}`;
})

dontButtonMobile.addEventListener("click", () => {
    window.location.href = `/Step/FinishMobilePage?runningFlowId=${getIdPartFromUrl("runningFlowId")}`;
})