export interface ValidationRule {
    minLength: number;
    maxlenght: number;
    errorMessage: string;
}

export const nameRule: ValidationRule = {minLength: 2, maxlenght: 100,  errorMessage: "must be between 2 and 100 characters long"};
export const questionRule: ValidationRule = {minLength: 3, maxlenght: 1000,  errorMessage: "must be between 3 and 1000 characters long"};
export const titleRule: ValidationRule = {minLength: 3, maxlenght: 100,  errorMessage: "must be between 3 and 100 characters long"};
export const infoRule: ValidationRule = {minLength: 3, maxlenght: 1000,  errorMessage: "must be between 3 and 1000 characters long"};

export function validateNameInput(input: HTMLInputElement, rule: ValidationRule = nameRule): boolean {
    const value: string = input.value;
    return value.length >= rule.minLength && value.length <= rule.maxlenght;
}

export function updateNameSpan(input: HTMLInputElement, span: HTMLElement, rule: ValidationRule = nameRule): void {
    if (!validateNameInput(input, rule)) {
        span.textContent = rule.errorMessage;
    } else {
        span.textContent = "";
    }
}

export function validateQuestionInput(input: HTMLInputElement, rule: ValidationRule = questionRule): boolean {
    const value: string = input.value;
    return value.length >= rule.minLength && value.length <= rule.maxlenght;
}

export function updateQuestionSpan(input: HTMLInputElement, span: HTMLElement, rule: ValidationRule = questionRule): void {
    if (!validateQuestionInput(input, rule)) {
        span.textContent = rule.errorMessage;
    } else {
        span.textContent = "";
    }
}

export function validateTitleInput(input: HTMLInputElement, rule: ValidationRule = titleRule): boolean {
    const value: string = input.value;
    return value.length >= rule.minLength && value.length <= rule.maxlenght;
}

export function updateTitleSpan(input: HTMLInputElement, span: HTMLElement, rule: ValidationRule = titleRule): void {
    if (!validateTitleInput(input, rule)) {
        span.textContent = rule.errorMessage;
    } else {
        span.textContent = "";
    }
}

export function validateInfoInput(input: HTMLInputElement, rule: ValidationRule = infoRule): boolean {
    const value: string = input.value;
    return value.length >= rule.minLength && value.length <= rule.maxlenght;
}

export function updateInfoSpan(input: HTMLInputElement, span: HTMLElement, rule: ValidationRule = infoRule): void {
    if (!validateInfoInput(input, rule)) {
        span.textContent = rule.errorMessage;
    } else {
        span.textContent = "";
    }
}