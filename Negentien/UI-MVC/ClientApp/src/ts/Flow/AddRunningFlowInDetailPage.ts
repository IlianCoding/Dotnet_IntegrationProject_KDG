import {addRunningFlow} from "./AddRunningFlow";
import {getStatus} from "../Project/Project";

const startFlowButton = document.getElementById("startFlowButton") as HTMLButtonElement
const flowIdAttendantDetailPage = document.getElementById("flowIdAttendantDetailPage") as HTMLInputElement
const attendName = document.getElementById("attendName") as HTMLInputElement
const radioTrueKiosk = document.getElementById("radioTrueKiosk") as HTMLInputElement
const radioFalseKiosk  = document.getElementById("radioFalseKiosk") as HTMLInputElement


if (startFlowButton) {
    startFlowButton.addEventListener("click", () => {
        console.log("button is clicked!")
        let isKiosk = getStatus(radioTrueKiosk, radioFalseKiosk);
        console.log(isKiosk);
        addRunningFlow(Number(flowIdAttendantDetailPage.value), attendName.value, isKiosk)
        console.log()
    })
}