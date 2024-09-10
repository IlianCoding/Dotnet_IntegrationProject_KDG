const passwordInput: HTMLInputElement = document.getElementById("Input_Password") as HTMLInputElement;
const passwordStrength: HTMLProgressElement = document.getElementById("password-strength") as HTMLProgressElement;
const passwordStrengthLabel: HTMLElement = document.getElementById("password-strength-label") as HTMLElement;

passwordInput.addEventListener("input", () => {
    const password: string = passwordInput.value;
    const score: number = calculatePasswordStrength(password);
    passwordStrength.value = score;

    if (score <= 33) {
        passwordStrengthLabel.textContent = "Weak";
    } else if (score <= 66) {
        passwordStrengthLabel.textContent = "Medium";
    } else {
        passwordStrengthLabel.textContent = "Strong";
    }
});


function calculatePasswordStrength(password: string): number {
    let score: number = 0;
    if (password.length >= 8) score += 20;
    if (/[a-z]/.test(password)) score += 20;
    if (/[A-Z]/.test(password)) score += 20;
    if (/\d/.test(password)) score += 20;
    if (/[!@#$%^&*]/.test(password)) score += 20;
    return score;
}

