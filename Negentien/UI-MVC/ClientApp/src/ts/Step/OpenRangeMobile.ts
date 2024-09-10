const colorSelect = document.getElementById('colorSelect') as HTMLSelectElement;
const openQuestionAnswer = document.getElementById('openQuestionAnswer') as HTMLInputElement;
const runningFlowIdOpenRangeMobile = document.getElementById('runningFlowIdOpenRangeMobile') as HTMLInputElement;
const QuestionId = document.getElementById('QuestionId') as HTMLInputElement;
const submitOpenAnswer = document.getElementById('submitOpenAnswer') as HTMLButtonElement;

function fetchSubmitOpenAnswer() {
    fetch('/api/Detection/ReceivingOpenAnswer', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            "AnswerInput": openQuestionAnswer.value,
            "RunningFlowId": runningFlowIdOpenRangeMobile.value,
            "Color": Number(colorSelect.value),
            "QuestionId": QuestionId.value,
        })
    })
        .then(() => {
            window.location.href = `/Step/ThankYouMobile?runningFlowId=${Number(runningFlowIdOpenRangeMobile.value)}`;
        }).catch(error => {
        console.error(error)
    })
}

submitOpenAnswer.addEventListener("click", () => {
    fetchSubmitOpenAnswer();
})