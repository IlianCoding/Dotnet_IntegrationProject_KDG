import {getIdPartFromUrl} from "../getIdPartFromUrl";
document.addEventListener("DOMContentLoaded", function() : void {
    const video : HTMLVideoElement | null = document.querySelector('#video');
    const canvas : HTMLCanvasElement | null = document.querySelector('#canvas');
    const runningFlowId: string | undefined = getRunningFlowIdFromUrl();
    
    if (runningFlowId != undefined){
        closeAllSessions(runningFlowId);
    }

    if (canvas){
        var context : CanvasRenderingContext2D | null = canvas.getContext('2d');
    }

    navigator.mediaDevices.getUserMedia({ video: true })
        .then(function(stream : MediaStream) : void {
            if (video){
                video.srcObject = stream;
            }
        })
        .catch(function(error) : void {
            console.error("Cannot access camera: ", error);
        });

    // Handelen van de button presses van de user
    function handleCaptureButtonClick() : void {
        const surveyStartIndicator : HTMLElement | null = document.getElementById('surveyStartIndicator');
        if (context && video){
            context.drawImage(video, 0, 0, 640, 480);
        } else {
            console.log("The context (2D) could not be found!");
        }
        if (canvas) {
            if (surveyStartIndicator) {
                console.log("Survey start indicator found. Starting survey.");
                createSessionAndStartSurvey(canvas);
            } else {
                console.log("Survey start indicator not found. Creating session only.");
                createSessionOnly(canvas);
            }
        } else {
            console.log("There was a problem finding the canvas!");
        }
    }

    document.addEventListener('keydown', function(event : KeyboardEvent) : void {
        if (['KeyH'].includes(event.code)){
            console.log("You pressed H!");
            handleCaptureButtonClick();
        }
    });
});

function getRunningFlowIdFromUrl() : string | undefined{
    const urlParams : URLSearchParams = new URLSearchParams(window.location.search);
    const runningFlowId : string | null = urlParams.get('runningFlowId');
    if (runningFlowId){
        return runningFlowId
    } else {
        console.error("The RunningFlowId could not be found!")
        return undefined;
    }
}
function closeAllSessions(runningFlowId: string): void {
    fetch(`/api/Detection/CloseSessions?runningFlowId=${runningFlowId}`, {
        method: 'PUT'
    })
        .then((response: Response) => {
            if (response.ok) {
                return response.json();
            } else {
                throw new Error('Failed to close all sessions');
            }
        })
        .then((data) => {
            console.log('Success:', data);
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}
function createSessionAndStartSurvey(canvas : HTMLCanvasElement) : void {
    canvas.toBlob((blob: Blob | null): void => {
        if (blob) {
            const formData: FormData = new FormData();
            formData.append('image', blob);

            const runningFlowId : string | undefined = getRunningFlowIdFromUrl();
            if (runningFlowId) {
                formData.append('RunningFlowId', runningFlowId);
            } else {
                console.error("The RunningFlowId could not be found!");
            }
            fetch('/api/Detection/StartingSession', {
                method: 'POST',
                body: formData,
            })
                .then((response: Response) : Promise<any> => response.json())
                .then((data): void => {
                    window.location.href = `/Step/FirstStep?runningFlowId=${getIdPartFromUrl('runningFlowId')}&remainingTime=${210000}&flowId=${getIdPartFromUrl('flowId')}`;
                    console.log('Success:', data);
                })
                .catch((error): void => {
                    window.location.href = `/Step/FirstStep?runningFlowId=${getIdPartFromUrl('runningFlowId')}&remainingTime=${210000}&flowId=${getIdPartFromUrl('flowId')}`;
                    console.error('Error:', error);
                });
        } else {
            console.log("Failed to get blob from canvas");
        }
    }, 'image/png');
}
function createSessionOnly(canvas : HTMLCanvasElement) : void {
    canvas.toBlob((blob: Blob | null): void => {
        if (blob) {
            const formData: FormData = new FormData();
            formData.append('image', blob);

            const runningFlowId : string | undefined = getRunningFlowIdFromUrl();
            if (runningFlowId) {
                formData.append('RunningFlowId', runningFlowId);
            } else {
                console.error("The RunningFlowId could not be found!");
            }
            fetch('/api/Detection/StartingSession', {
                method: 'POST',
                body: formData,
            })
                .then((response: Response) : Promise<any> => response.json())
                .then((data): void => {
                    console.log('Success:', data);
                })
                .catch((error): void => {
                    console.error('Error:', error);
                });
        } else {
            console.log("Failed to get blob from canvas");
        }
    }, 'image/png');
}