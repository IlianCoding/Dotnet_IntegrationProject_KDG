import * as validate from "./validateStepInput"

function addEventListeners() {
    let addButton: HTMLElement | null = document.getElementById(`addStep`);
    if (addButton) {
        addButton.addEventListener("click", (event) => addStep());
    } else {
        throw new Error("Couldn't find add button");
    }
}

function addStep() {
    let activeTab: HTMLElement | null = document.querySelector('.tab-pane.active');
    if (activeTab === null) {
        throw new Error("Couldnt read active tab");
    }

    let tabName = activeTab.id.replace('pills-', "").charAt(0);

    if (activeTab?.id == "pills-multi" || activeTab?.id == "pills-single" || activeTab?.id == "pills-range") {
        const questionInputSpan: HTMLSpanElement | null = activeTab.querySelector(`#vraagInputSpan_${tabName}`) as HTMLSpanElement;
        const questionInput: HTMLInputElement | null = activeTab!.querySelector(`#vraagInput_${tabName}`) as HTMLInputElement;
        questionInput.addEventListener("blur", () => validate.updateQuestionSpan(questionInput, questionInputSpan, validate.questionRule));
        if (!validate.validateQuestionInput(questionInput!, validate.questionRule)) {
            validate.updateQuestionSpan(questionInput, questionInputSpan, validate.questionRule);
            return;
        }
        const answerInputContainer: HTMLElement | null = activeTab!.querySelector(`#antwoordInput_${tabName}`);
        let answerInputsList: string[] = [];

        if (answerInputContainer) {
            let answerInputs: NodeListOf<HTMLInputElement> = answerInputContainer.querySelectorAll('input') as NodeListOf<HTMLInputElement>;
            answerInputs.forEach(answerinput => {
                if (answerinput.value !== "") {
                    answerInputsList.push(answerinput.value);
                }
            });
        } else {
            throw new Error("Couldnt read answerinput container");
        }

        const newQuestionWithOptionsDto = {
            "QuestionText": questionInput?.value,
            "AnswerOptions": answerInputsList,
            "QuestionType": activeTab?.id.replace('pills-', "")
        };

        fetch('/api/Steps/AddQuestionWithOptions', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(newQuestionWithOptionsDto)
        }).then(r => {
            if (!r.ok) {
                return r.json().then(error => {
                    let errorMessage = "";
                    error.forEach((e: any) => errorMessage += e.errorMessage + "\n");
                    alert(errorMessage);
                });
            }
            r.json().then(data => postStep(data.id, tabName));
        }).catch(error => {console.log(error.message)});

    } else if (activeTab?.id == "pills-open") {
        const questionInput: HTMLInputElement | null = activeTab!.querySelector(`#vraagInput_${tabName}`) as HTMLInputElement;
        const questionInputSpan: HTMLSpanElement | null = activeTab!.querySelector(`#vraagInputSpan_${tabName}`) as HTMLSpanElement;
        questionInput.addEventListener("blur", () => validate.updateQuestionSpan(questionInput, questionInputSpan, validate.questionRule));
        if (!validate.validateQuestionInput(questionInput!, validate.questionRule)){
            validate.updateQuestionSpan(questionInput, questionInputSpan, validate.questionRule);
            return;
        }

        const newQuestionDto = {
            "QuestionText": questionInput?.value
        };

        fetch('/api/Steps/AddQuestionOpen', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(newQuestionDto)
        }).then(r => {
            if (!r.ok) {
                return r.json().then(error => {
                    let errorMessage = "";
                    error.forEach((e: any) => errorMessage += e.errorMessage + "\n");
                    alert(errorMessage);
                });
            }
            r.json().then(data => postStep(data.id, tabName));
        }).catch(error => {console.log(error.message)});

    } else if (activeTab?.id == "pills-info") {
        const titleInput: HTMLInputElement | null = activeTab!.querySelector('#titleInput') as HTMLInputElement;
        const infoInput: HTMLInputElement | null = activeTab!.querySelector('#infoInput') as HTMLInputElement;
        const titleInputSpan: HTMLSpanElement | null = activeTab!.querySelector(`#titleInputSpan_${tabName}`) as HTMLSpanElement;
        const infoInputSpan: HTMLSpanElement | null = activeTab!.querySelector(`#infoInputSpan_${tabName}`) as HTMLSpanElement;
        titleInput.addEventListener("blur", () => validate.updateTitleSpan(titleInput, titleInputSpan, validate.titleRule));
        if (!validate.validateTitleInput(titleInput!, validate.titleRule)){
            validate.updateTitleSpan(titleInput, titleInputSpan, validate.titleRule)
            return;
        }
        infoInput.addEventListener("blur", () => validate.updateInfoSpan(infoInput, infoInputSpan, validate.infoRule));
        if (!validate.validateInfoInput(infoInput!, validate.infoRule)){
            validate.updateInfoSpan(infoInput, infoInputSpan, validate.infoRule)
            return;
        }

        const newInfoDto = {
            "Title": titleInput?.value,
            "TextInformation": infoInput?.value
        };

        fetch('/api/Steps/AddInformation', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(newInfoDto)
        }).then(r => {
            if (!r.ok) {
                return r.json().then(error => {
                    let errorMessage = "";
                    error.forEach((e: any) => errorMessage += e.errorMessage + "\n");
                    alert(errorMessage);
                });
            }
            r.json().then(data => postStep(data.id, tabName));
        }).catch(error => {console.log(error.message)});
    }
}

function postStep(contentID: number, tabName: string): void {
    const themaInput: HTMLInputElement | null = document.querySelector(`#themes_${tabName}`) as HTMLInputElement;
    const flowId: HTMLInputElement | null = document.querySelector(`.flow-id`) as HTMLInputElement;
    const nameInput: HTMLInputElement | null = document.querySelector(`#nameInput_${tabName}`) as HTMLInputElement;
    const nameInputSpan: HTMLSpanElement | null = document.querySelector(`#nameInputSpan_${tabName}`) as HTMLSpanElement;
    nameInput.addEventListener("blur", () => validate.updateNameSpan(nameInput, nameInputSpan, validate.nameRule));
    if (!validate.validateNameInput(nameInput!, validate.nameRule)){
        validate.updateNameSpan(nameInput, nameInputSpan, validate.nameRule);
        return;
    }

    const newStepDto = {
        ThemeId: themaInput!.value,
        ContentId: contentID.toString(),
        FlowId: flowId.innerText,
        Name: nameInput!.value,
        IsConditioneel: true
    }

    fetch('/api/Steps/AddStep', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(newStepDto)
    }).then(r => {
        if (!r.ok) {
            throw new Error("Error submitting answers " + r.statusText);
        }
        return r.json();
    }).then(function () {
        console.log("Step added - redirecting to flow detail");
        window.location.href = `/Flow/FlowDetail/${flowId?.innerText}`;
    }).catch(function (error) {
        console.log(error);
    });
}

addEventListeners();