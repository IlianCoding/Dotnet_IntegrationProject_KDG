export interface ValidationRule {
    minLength: number;
    errorMessage: string;
}

const minLengthDefault: number = 2;

export function isInputRequiredMinLength(input: HTMLInputElement, minLength: number = minLengthDefault) {
    const value: string = input.value;
    return value.length >= minLength;
}

export function spanMessageByInput(input: HTMLInputElement, span: HTMLElement, minLength: number = minLengthDefault, errorTextField: string = "") {
    if (!isInputRequiredMinLength(input, minLength)) {
        span.textContent = errorTextField.charAt(0).toUpperCase() + errorTextField.charAt(1).toLowerCase() + " must be at least " + minLength + " characters long!";
    } else {
        span.textContent = "";

    }
}

