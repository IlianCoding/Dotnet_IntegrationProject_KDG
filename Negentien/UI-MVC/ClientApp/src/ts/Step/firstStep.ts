import {getStepId} from "../ConditionalPoint/addConditionalPoint";
import {getIdPartFromUrl} from "../getIdPartFromUrl";
import {getRunningFlowIdFromUrl} from "./videoInput";

const loadSubmitButton: HTMLElement | null = document.getElementById("submitAnswers");
let inputs : NodeListOf<HTMLInputElement> = document.querySelectorAll('.form-check-input') as NodeListOf<HTMLInputElement>;
let selectedOptionIds: any[] = [];
let stepId: string = document.getElementsByTagName("input")[0].name;

if (loadSubmitButton) {
    loadSubmitButton.addEventListener("click", (event : MouseEvent) => submitAndLoadNextStep());
}

function submitAndLoadNextStep() {
    let openAnswerInput = document.querySelector('.form-text') as HTMLInputElement;

    inputs.forEach(input => {
        if (input.checked) {
            selectedOptionIds.push(input.value);
        }
    });

    if (openAnswerInput !== null && openAnswerInput.value.trim() !== "") {
        var newUserAnswerDto = {
            "AnswerString": openAnswerInput.value,
            "RunningFlowId": getRunningFlowIdFromUrl(),
            "QuestionId": openAnswerInput.id,
            "StepId": Number.parseInt(stepId)
        }
        postUserAnswer(newUserAnswerDto);
    } else if (selectedOptionIds.length > 0) {
        selectedOptionIds.forEach(selectedOptionId => {
            var newUserAnswerDto = {
                "AnswerId": selectedOptionId,
                "RunningFlowId": getRunningFlowIdFromUrl(),
                "StepId": Number.parseInt(stepId)
            }
            postUserAnswer(newUserAnswerDto);
        });
    } else {
        window.location.href = `/Step/NextStep?runningFlowId=${getRunningFlowIdFromUrl()}&stepId=${stepId}&answerOptionId=${selectedOptionIds[0]}`;
    }
}

function postUserAnswer(newUserAnswerDto: { AnswerId?: string, AnswerString?: string, RunningFlowId: string | undefined, StepId: number }) : void {
    fetch('/api/UserAnswers', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(newUserAnswerDto)
    })
        .then(function (response: Response) {
            if (!response.ok) {
                throw new Error("Error submitting answers " + response.statusText);
            }
            return response.json();
        })
        .then(function (userAnswerDto): void {
            const nextStepId: string | null = loadSubmitButton!.getAttribute("data-next-step-id");
            console.log(selectedOptionIds[0])
            window.location.href = `/Step/NextStep?runningFlowId=${getRunningFlowIdFromUrl()}&stepId=${stepId}&answerOptionId=${selectedOptionIds[0]}`;
        })
        .catch(function (error): void {
            console.log(error);
        });
}





