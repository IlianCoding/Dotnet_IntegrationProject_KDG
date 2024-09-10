import {SignedUrl} from "../models/SignedUrl.interface";

document.addEventListener("DOMContentLoaded", addMedia)

export function addMedia() {
    // Use document.querySelectorAll to get all elements with the specified class
    const elements = document.querySelectorAll(`.media-link`);

    elements.forEach(el => {
        const element = el as  HTMLImageElement
        const objectName = element.getAttribute("data-object-name")
        
        if (objectName){
        getUrl(el as HTMLImageElement, objectName)
        }
    })
}


export function getUrl(element: HTMLImageElement, objectName: string): void {
   

    console.log(objectName + "hiur")

    fetch("/api/files?objectName=" + objectName, {

        method: "GET"
    })
        .then(response => {
            if (!response.ok) {
                alert("Upload failed!");
            }

            if (response.status === 200) {
                return response.json() as Promise<SignedUrl> ;
            } else {
                throw new Error("Unexpected response status: " + response.status);
            }
        })
        .then(signedUrl => {
            element.src = signedUrl.url

        })
        .catch(error => {
            console.error("Error:", error);
        });
}