import {UrlObjectName} from "../models/Url.interface";
import {insertRightHtmlImage} from "./logo";

document.addEventListener("DOMContentLoaded", insertLogoFlow)



function insertLogoFlow() {
    const logoStatic = document.getElementById("logo-static") as HTMLElement

    const platformData = document.querySelector(".step-id-logo") as HTMLElement
    const stepId = platformData.getAttribute("data-step-logo") || ""

    if (stepId) {
        fetch("/api/Platforms/GetLogoByStepId?stepId=" + stepId, {

            method: "GET"
        }).then(response => {
            if (response.ok) {
                return response.json() as Promise<UrlObjectName>
            }
        }).then(data => {
            if (logoStatic) {
                if (data) {
                    insertRightHtmlImage(false, logoStatic, data.objectName)
                } else {
                    insertRightHtmlImage(false, logoStatic, null)
                }
            }
        })
    }
}