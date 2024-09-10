export function getStatus(trueRadio: HTMLInputElement, falseRadio: HTMLInputElement): boolean {
    if (trueRadio.checked) {
        return true;
    } else if (falseRadio.checked) {
        return false;
    }
    return false;
}
export function setRadioIsActiveByInputs(input: boolean, radioTrueInput: HTMLInputElement, radioFalseInput: HTMLInputElement) {

    if (!input) {
        radioFalseInput.checked = true;
        radioTrueInput.checked = false;
    } else {
        radioTrueInput.checked = true;
        radioFalseInput.checked = false;
    }
}

// export function setRadioText(radioTrueInput: HTMLInputElement, radioFalseInput: HTMLInputElement,
//                              stringInputTrue: string = "", stringInputFalse: string = "") {
//     radioTrueInput.textContent = stringInputTrue;
//     radioFalseInput.textContent = stringInputFalse;
// }

export function setSpanByIsactive(status: boolean, span: HTMLSpanElement,
                                  spanTrue: string = "Active", spanFalse: string= "Inactive") {
    if (status) {
        span.textContent = spanTrue;
    } else {
        span.textContent = spanFalse;
    }
}

export function getStatusInString(isActive: boolean) {
    if (isActive) {
        return "Active"
    } else {
        return "InActive"
    }
}