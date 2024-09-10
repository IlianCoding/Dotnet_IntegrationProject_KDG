import {insertRightHtmlImage} from "./logo";
import {UrlObjectName} from "../models/Url.interface";

document.addEventListener("DOMContentLoaded", insertLogo)



function insertLogo() {

    const logoStatic = document.getElementById("logo-static") as HTMLElement

    const platformData = document.getElementById("platformData") as HTMLElement
    const projectId = platformData.getAttribute("data-project-id") || ""


    fetch("/api/Platforms/GetLogoByProjectId?projectId="+projectId, {

        method: "GET"
    }).then(response =>{
        if (response.ok){
            return response.json() as Promise<UrlObjectName>
        }
    }).then(data => {
        if (logoStatic ) {
            if (data){
            insertRightHtmlImage(false, logoStatic, data.objectName)
            }else {
                insertRightHtmlImage(false, logoStatic,null )
            }
        }
    })
   

}