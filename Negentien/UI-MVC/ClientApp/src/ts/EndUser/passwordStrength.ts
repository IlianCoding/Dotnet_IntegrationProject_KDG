// wwwroot/js/passwordStrength.ts
document.addEventListener('DOMContentLoaded', (): void => {
    const passwordInput : HTMLInputElement = document.getElementById('password') as HTMLInputElement;
    const passwordStrength : HTMLElement = document.getElementById('password-strength') as HTMLElement;
    const lengthRequirement : HTMLElement = document.getElementById('length-requirement') as HTMLElement;
    const uppercaseRequirement : HTMLElement = document.getElementById('uppercase-requirement') as HTMLElement;
    const specialRequirement : HTMLElement = document.getElementById('special-requirement') as HTMLElement;

    passwordInput.addEventListener('input', () : void => {
        const password : string = passwordInput.value;

        if (password.length >= 6) {
            lengthRequirement.classList.add('valid');
        } else {
            lengthRequirement.classList.remove('valid');
        }

        if (/[A-Z]/.test(password)) {
            uppercaseRequirement.classList.add('valid');
        } else {
            uppercaseRequirement.classList.remove('valid');
        }

        if (/[\#]/.test(password)) {
            specialRequirement.classList.add('valid');
        } else {
            specialRequirement.classList.remove('valid');
        }

        const validLength = password.length >= 6;
        const hasUppercase = /[A-Z]/.test(password);
        const hasSpecialChar = /[\#]/.test(password);

        if (validLength && hasUppercase && hasSpecialChar) {
            passwordStrength.textContent = 'Strong';
            passwordStrength.style.color = 'green';
        } else if (validLength && (hasUppercase || hasSpecialChar)) {
            passwordStrength.textContent = 'Medium';
            passwordStrength.style.color = 'orange';
        } else {
            passwordStrength.textContent = 'Weak';
            passwordStrength.style.color = 'red';
        }
    });
});
