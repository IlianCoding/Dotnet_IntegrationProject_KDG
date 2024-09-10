import "../../scss/logo.scss"
import {Url} from "../models/Url.interface";
import {getUrl} from "../files/fileManaging";
import {SignedUrl} from "../models/SignedUrl.interface";

document.addEventListener("DOMContentLoaded", insertLogo)

const saveLogo = document.getElementById("save-logo") as HTMLElement;
if (saveLogo) {
    saveLogo.addEventListener("click", ev => changeLogo(ev))
}


function insertLogo() {

    const logoChange = document.getElementById("logo-change") as HTMLElement
 
    const platformData = document.getElementById("platformData") as HTMLElement
    const objectName = platformData?.dataset?.platformLogo || ""

    if (logoChange) {
        insertRightHtmlImage(true, logoChange, objectName)
    }
}

export function insertRightHtmlImage(isChangeableLogo: boolean, logoDiv: HTMLElement, objectName: string | null) {


    if (objectName) {
        fetch("/api/files?objectName=" + objectName, {

            method: "GET"
        })
            .then(response => {
                if (!response.ok) {
                    alert("Upload failed!");
                }

                if (response.status === 200) {
                    return response.json() as Promise<SignedUrl>;
                } else {
                    throw new Error("Unexpected response status: " + response.status);
                }
            })
            .then(signedUrl => {

                logoDiv.innerHTML = getImageHtml(isChangeableLogo, signedUrl.url)


            })
            .catch(error => {
                console.error("Error:", error);
            });
    } else {
        const avatar = "https://mdbootstrap.com/img/Photos/Others/placeholder-avatar.jpg"

        logoDiv.innerHTML = getImageHtml(isChangeableLogo, avatar)
    }
}


function getImageHtml(isChangeableLogo: boolean, url: string): string {
    if (isChangeableLogo) {

        return `
       <div class="col-11 bg-image rounded-circle hover-zoom min-vh-50 min-vw-50">
                       <img id="image-logo" role="button" class="img-fluid rounded-circle" data-bs-toggle="modal" data-bs-target="#exampleModal" alt="avatar1" src=${url} >
                   </div>
    `
    } else {
        return `
       <div class="col-11 rounded-circle min-vh-50 min-vw-50">
                       <img id="image-logo-static"  class="img-fluid rounded-circle"  alt="avatar1" src=${url} >
                   </div>
    `
    }
}

const fileInputElement = document.getElementById('customFile2') as HTMLInputElement;
if (fileInputElement) {
    fileInputElement.addEventListener('change', function (event) {
        displaySelectedImage(event, 'selectedAvatar');
    });
}

function displaySelectedImage(event: Event, elementId: string): void {
    const selectedImage = document.getElementById(elementId) as HTMLImageElement;
    const fileInput = event.target as HTMLInputElement;

    if (fileInput.files && fileInput.files[0]) {
        const reader = new FileReader();

        reader.onload = function (e: ProgressEvent<FileReader>) {
            selectedImage.src = e.target?.result as string;
        };

        reader.readAsDataURL(fileInput.files[0]);
    }
}


function changeLogo(event: Event) {
    const clickedButton = event.target as HTMLButtonElement;
    const clickedValue = clickedButton.value;
    const fileInput =
        document.getElementById("customFile2") as HTMLInputElement

    const formData = new FormData(document.getElementById("logo-form") as HTMLFormElement);

    if (fileInput.files?.length === 0) {
        return
    }

    fetch("/api/files", {
        method: "POST",
        body: formData
    }).then(response => {
        if (!response.ok) {
            alert("Upload failed!");
        }

        if (response.status === 200) {
            return response.json() as Promise<Url>;
        } else {
            throw new Error("Unexpected response status: " + response.status);
        }
    })
        .then(responseObject => {

            console.log(`Here is the image in the cloud: ${responseObject.objectName}`);

            changeLogoOfPlatform(clickedValue, responseObject)

        })
        .catch(error => {
            console.error("Error:", error);
        });
}

function changeLogoOfPlatform(clickedValue: string, responseObject: Url) {
    const dataPlatform = {
        "id": clickedValue,
        "objectName": responseObject.objectName,
        "contentType": responseObject.contentType
    }

    fetch("/api/Platforms", {
        method: "PUT", headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        }, body: JSON.stringify(dataPlatform)
    }).then(
        response => {
            if (!response.ok) {
                alert("update failed: " + response.status)
            } else {

                const logoDiv = document.getElementById("image-logo") as HTMLImageElement;

                getUrl(logoDiv, dataPlatform.objectName)
            }
        }
    ).catch(() => {
        alert("Oops, something went wrong!")
    })
}

