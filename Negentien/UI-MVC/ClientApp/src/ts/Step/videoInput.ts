import {getIdPartFromUrl} from "../getIdPartFromUrl";
import '../../scss/answerAnimation.scss';

document.addEventListener("DOMContentLoaded", function (): void {
    const video: HTMLVideoElement | null = document.querySelector('#video');
    const canvas: HTMLCanvasElement | null = document.querySelector('#canvas');
    const timerDisplay: HTMLElement | null = document.getElementById('timer');

    // Alles gerelateerd aan de timers
    let remainingTime: number;
    let timerInterval: NodeJS.Timeout | null = null;
    let pauseCheckInterval: NodeJS.Timeout | null = null;

    let totalTimer: NodeJS.Timeout | null = null;
    let totalTimeRemaining: number | null = getIdPartFromUrl('remainingTime');
    let startTime: number = Date.now();

    // Bijhouden van het meest geantwoorde
    let answerCounts: { [answerId: string]: number } = {};

    if (canvas) {
        var context: CanvasRenderingContext2D | null = canvas.getContext('2d');
    }

    // Updaten van het totaal aantal keer dat een bepaalde antwoord is geantwoord
    function updateAnswerCounts(answerId: string): void {
        answerCounts[answerId] = (answerCounts[answerId] || 0) + 1;
    }

    // Wanneer de 30 seconden voorbij zijn wordt er naar de volgende vraag genavigeerd 
    function moveToTheNextQuestion() {
        let mostAnsweredQuestionId: string | null = null;
        let maxAnswerCount: number = -1;

        const runningFlowId: string | undefined = getRunningFlowIdFromUrl();
        let stepId: number = 0;

        const stepIdDiv: HTMLElement | null = document.getElementById('stepId');
        if (stepIdDiv != null) {
            let stepIdDivText: string | null = stepIdDiv.textContent;
            if (stepIdDivText != null) {
                stepId = parseInt(stepIdDivText);
            }
        }
        let remainingTime: number = 0;

        for (const answerId in answerCounts) {
            const answerCount = answerCounts[answerId];
            if (answerCount > maxAnswerCount) {
                mostAnsweredQuestionId = answerId;
                maxAnswerCount = answerCount;
            }
        }
        
        const currentTime: number = Date.now();
        if (totalTimeRemaining != null) {
            remainingTime = Math.max(0, totalTimeRemaining - (currentTime - startTime))
        } else {
            console.log("The time has passed!");
            window.location.href = `/SurveyHome/IndexSurveyHome?runningFlowId=${getIdPartFromUrl('runningFlowId')}&flowId=${getIdPartFromUrl('flowId')}`;
        }

        if (mostAnsweredQuestionId) {
            console.log(`Next question ID: ${mostAnsweredQuestionId}`);
            if (stepId != null) {
                window.location.href = `/Step/NextStep?runningFlowId=${getIdPartFromUrl('runningFlowId')}&remainingTime=${remainingTime}&flowId=${getIdPartFromUrl('flowId')}&stepId=${stepId}&answerOptionId=${parseInt(mostAnsweredQuestionId)}`;
            } else {
                console.log("The stepId was incorrect");
            }
        } else {
            console.log("No question found. Redirecting to the next question with default answer ID.");
            if (stepId != null) {
                window.location.href = `/Step/NextStep?runningFlowId=${getIdPartFromUrl('runningFlowId')}&remainingTime=${remainingTime}&flowId=${getIdPartFromUrl('flowId')}&stepId=${stepId}&answerOptionId=undefined`;
            } else {
                console.log("The stepId was incorrect");
            }
        }
    }

    // Question Timer
    function displayTime(time: number): void {
        const timerElement = document.getElementById('timer');
        if (timerElement) {
            timerElement.innerText = `${time} seconds`;
        }
    }

    function startTimer(): void {
        remainingTime = 30;
        const runningFlowId = getRunningFlowIdFromUrl();
        if (!runningFlowId) return;

        timerInterval = setInterval(async () => {
            remainingTime--;
            displayTime(remainingTime);

            if (remainingTime <= 1) {
                if (timerInterval !== null) {
                    clearInterval(timerInterval);
                }
                await handlePauseCheck(runningFlowId);
            }
        }, 1000);

        if (totalTimeRemaining != null) {
            startTotalTimer(totalTimeRemaining);
        }
    }

    // Total Timer
    function startTotalTimer(remainingTime: number) {
        totalTimer = setTimeout(() => {
            console.log("Redirecting to the home page due to inactivity");

            window.location.href = `/SurveyHome/IndexSurveyHome?runningFlowId=${getIdPartFromUrl('runningFlowId')}&flowId=${getIdPartFromUrl('flowId')}`;
        }, remainingTime);
    }

    function updateTotalTimer() {
        if (totalTimer) {
            clearTimeout(totalTimer);
        }
        totalTimeRemaining = 300000;
        totalTimer = setTimeout(() => {
            console.log("Redirecting to the home page due to inactivity");
            window.location.href = `/SurveyHome/IndexSurveyHome?runningFlowId=${getIdPartFromUrl('runningFlowId')}&flowId=${getIdPartFromUrl('flowId')}`;
        }, 300000);
    }


    // General timer functions
    async function checkRunningFlowState(runningFlowId: string): Promise<number> {
        try {
            const response = await fetch(`/api/Detection/GetState?runningFlowId=${runningFlowId}`);
            const data = await response.json();
            return data.state;
        } catch (error) {
            console.error('Error fetching running flow state:', error);
            return -1;
        }
    }

    async function handlePauseCheck(runningFlowId: string): Promise<void> {
        const state = await checkRunningFlowState(runningFlowId);
        if (state === 1) { 
            console.log('The running flow is paused.');
            pauseTimers();

            pauseCheckInterval = setInterval(async () => {
                const currentState = await checkRunningFlowState(runningFlowId);
                if (currentState !== 1) { 
                    if (pauseCheckInterval !== null) {
                        clearInterval(pauseCheckInterval);
                    }
                    console.log('The running flow is resumed. Proceeding to the next step.');
                    resumeTimers(runningFlowId);
                }
            }, 10000); 
        } else {
            console.log('Proceeding to the next step.');
            moveToTheNextQuestion();
        }
    }

    function pauseTimers(): void {
        if (timerInterval !== null) {
            clearInterval(timerInterval);
        }
        if (totalTimer !== null) {
            clearTimeout(totalTimer);
        }
    }

    function resumeTimers(runningFlowId: string): void {
        timerInterval = setInterval(async () => {
            remainingTime--;
            displayTime(remainingTime);

            if (remainingTime <= 1) {
                if (timerInterval !== null) {
                    clearInterval(timerInterval);
                }
                await handlePauseCheck(runningFlowId);
            }
        }, 1000);

        if (totalTimeRemaining != null) {
            startTotalTimer(totalTimeRemaining);
        }
    }
    
    startTimer();

    navigator.mediaDevices.getUserMedia({video: true})
        .then(function (stream: MediaStream): void {
            if (video) {
                video.srcObject = stream;
            }
        })
        .catch(function (error): void {
            console.error("Cannot access camera: ", error);
        });

    // Handelen van de button presses van de user
    function handleCaptureButtonClick(): void {
        const surveyStartIndicator: HTMLElement | null = document.getElementById('surveyStartIndicator');
        if (context && video) {
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

    function handleCaptureButtonAnswerClick(answerValue: string): void {
        if (context && video) {
            context.drawImage(video, 0, 0, 640, 480);
        } else {
            console.log("The context (2D) could not be found!");
        }
        if (canvas) {
            sendAnswerData(canvas, answerValue);
        } else {
            console.log("There was a problem finding the canvas!");
        }
    }

    document.addEventListener('keydown', function (event: KeyboardEvent): void {
        let questionInput: string | undefined;
        let labelId: string | undefined;

        switch (event.code) {
            case 'KeyQ':
                console.log("You pressed Q!");
                questionInput = (document.getElementById('input_0') as HTMLInputElement).value;
                labelId = 'label_0';
                break;
            case 'KeyS':
                console.log("You pressed S!");
                questionInput = (document.getElementById('input_1') as HTMLInputElement).value;
                labelId = 'label_1';
                break;
            case 'KeyD':
                console.log("You pressed D!");
                questionInput = (document.getElementById('input_2') as HTMLInputElement).value;
                labelId = 'label_2';
                break;
            case 'KeyF':
                console.log("You pressed F!");
                questionInput = (document.getElementById('input_3') as HTMLInputElement).value;
                labelId = 'label_3';
                break;
            case 'KeyG':
                console.log("You pressed G!");
                questionInput = (document.getElementById('input_4') as HTMLInputElement).value;
                labelId = 'label_4';
                break;
            case 'KeyH':
                console.log("You pressed H!");
                handleCaptureButtonClick();
                return;
            default:
                return;
        }

        if (questionInput) {
            console.log(questionInput);
            updateTotalTimer();
            updateAnswerCounts(questionInput);
            handleCaptureButtonAnswerClick(questionInput);
            if (labelId) {
                bounceLabel(labelId);
            }
        }
    });

    function bounceLabel(labelId: string): void {
        const label: HTMLElement | null = document.getElementById(labelId);
        if (label) {
            label.classList.add('bounce');
            setTimeout((): void => {
                label.classList.remove('bounce');
            }, 1000);
        }
    }

function sendAnswerData(canvas : HTMLCanvasElement, answerValue : string)  : void {
    canvas.toBlob((blob : Blob | null): void => {
        if (blob) {
            const formData : FormData = new FormData();
            formData.append('image', blob);
            formData.append('answerInput', answerValue);

                const runningFlowId: string | undefined = getRunningFlowIdFromUrl();
                if (runningFlowId) {
                    formData.append('runningFlowId', runningFlowId);
                } else {
                    console.error("The RunningFlowId could not be found!");
                }

                fetch('/api/Detection/ReceivingChoiceAnswer', {
                    method: 'POST',
                    body: formData,
                })
                    .then((response: Response) => response.json())
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

    function createSessionAndStartSurvey(canvas: HTMLCanvasElement): void {
        canvas.toBlob((blob: Blob | null): void => {
            if (blob) {
                const formData: FormData = new FormData();
                formData.append('image', blob);

                const runningFlowId: string | undefined = getRunningFlowIdFromUrl();
                if (runningFlowId) {
                    formData.append('RunningFlowId', runningFlowId);
                } else {
                    console.error("The RunningFlowId could not be found!");
                }
                fetch('/api/Detection/StartingSession', {
                    method: 'POST',
                    body: formData,
                })
                    .then((response: Response): Promise<any> => response.json())
                    .then((data): void => {
                        window.location.href = `/Step/FirstStep?runningFlowId=${getIdPartFromUrl('runningFlowId')}&remainingTime=${180000}&flowId=${getIdPartFromUrl('flowId')}`;
                        console.log('Success:', data);
                    })
                    .catch((error): void => {
                        window.location.href = `/Step/FirstStep?runningFlowId=${getIdPartFromUrl('runningFlowId')}&remainingTime=${180000}&flowId=${getIdPartFromUrl('flowId')}`;
                        console.error('Error:', error);
                    });
            } else {
                console.log("Failed to get blob from canvas");
            }
        }, 'image/png');
    }

    function createSessionOnly(canvas: HTMLCanvasElement): void {
        canvas.toBlob((blob: Blob | null): void => {
            if (blob) {
                const formData: FormData = new FormData();
                formData.append('image', blob);

                const runningFlowId: string | undefined = getRunningFlowIdFromUrl();
                if (runningFlowId) {
                    formData.append('RunningFlowId', runningFlowId);
                } else {
                    console.error("The RunningFlowId could not be found!");
                }
                fetch('/api/Detection/StartingSession', {
                    method: 'POST',
                    body: formData,
                })
                    .then((response: Response): Promise<any> => response.json())
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
});

export function getRunningFlowIdFromUrl() : string | undefined{
    const urlParams : URLSearchParams = new URLSearchParams(window.location.search);
    const runningFlowId : string | null = urlParams.get('runningFlowId');
    if (runningFlowId){
        return runningFlowId
    } else {
        console.error("The RunningFlowId could not be found!")
        return undefined;
    }
}