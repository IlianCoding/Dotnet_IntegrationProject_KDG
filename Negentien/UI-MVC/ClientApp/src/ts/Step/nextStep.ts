import { getIdPartFromUrl } from "../getIdPartFromUrl";

const loadSubmitButton = document.getElementById("submitAnswers");
const inputs = document.querySelectorAll('.form-check-input') as NodeListOf<HTMLInputElement>;
let selectedOptionIds: any[] = [];
const stepId: string = document.getElementById("stepId")!.textContent!;

if (loadSubmitButton) {
    loadSubmitButton.addEventListener("click", submitAndLoadNextStep);
}

function getRunningFlowIdFromUrl() {
    const runningFlowId = getIdPartFromUrl('runningFlowId')?.toString();
    const flowId = getIdPartFromUrl('flowId')?.toString();

    if (runningFlowId) {
        return Promise.resolve(runningFlowId);
    } else if (flowId) {
        return fetch(`/api/RunningFlows/GetTestingFlow?flowId=${flowId}`, {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            }
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to fetch running flow ID');
                }
                return response.json();
            })
            .then(data => data.toString())
            .catch(error => {
                console.error('Error fetching running flow ID:', error);
                return undefined;
            });
    } else {
        return Promise.reject('Flow ID not found in URL');
    }
}

function submitAndLoadNextStep() {
    let openAnswerInput = document.querySelector('.form-text') as HTMLInputElement;

    getRunningFlowIdFromUrl().then(runningFlowId => {
        inputs.forEach(input => {
            if (input.checked) {
                selectedOptionIds.push(input.value);
            }
        });

        let promises = [];

        if (openAnswerInput !== null && openAnswerInput.value.trim() !== "") {
            var newUserAnswerDto = {
                "AnswerString": openAnswerInput.value,
                "RunningFlowId": runningFlowId,
                "QuestionId": openAnswerInput.id,
                "StepId": Number.parseInt(stepId)
            }
            promises.push(postUserAnswer(newUserAnswerDto));
        } else if (selectedOptionIds.length > 0) {
            selectedOptionIds.forEach(selectedOptionId => {
                var newUserAnswerDto = {
                    "AnswerId": selectedOptionId,
                    "RunningFlowId": runningFlowId,
                    "StepId": Number.parseInt(stepId)
                }
                promises.push(postUserAnswer(newUserAnswerDto));
            });
        }

        Promise.all(promises).then(() => {
            window.location.href = `/Step/NextStep?runningFlowId=${runningFlowId}&stepId=${stepId}&answerOptionId=${selectedOptionIds[0]}`;
        }).catch(error => {
            console.error('Error submitting answers:', error);
        });
    }).catch(error => {
        console.error('Error getting running flow ID:', error);
    });
}

function postUserAnswer(newUserAnswerDto: { AnswerId?: string, AnswerString?: string, RunningFlowId: string | undefined, StepId: number }) {
    return fetch('/api/UserAnswers', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(newUserAnswerDto)
    })
        .then(function (response) {
            if (!response.ok) {
                throw new Error("Error submitting answers " + response.statusText);
            }
            return response.json();
        });
}
