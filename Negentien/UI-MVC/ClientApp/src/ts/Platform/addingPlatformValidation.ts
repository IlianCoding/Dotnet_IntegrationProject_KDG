document.addEventListener('DOMContentLoaded', () => {
    const birthdayInput : HTMLInputElement = document.getElementById('Birthday') as HTMLInputElement;
    const birthdayError : HTMLElement | null = document.getElementById('birthday-error');

    if (birthdayInput) {
        birthdayInput.addEventListener('change', () => {
            const today : Date = new Date();
            const birthday : Date = new Date(birthdayInput.value);
            var age : number = today.getFullYear() - birthday.getFullYear();
            const monthDifference : number = today.getMonth() - birthday.getMonth();
            
            if (monthDifference < 0 || (monthDifference === 0 && today.getDate() < birthday.getDate())) {
                age--;
            }
            
            if (birthdayError!= null){
                if (age < 16) {
                    birthdayError.textContent = 'The person must be at least 16 years old.';
                    birthdayInput.classList.add('is-invalid');
                } else {
                    birthdayError.textContent = '';
                    birthdayInput.classList.remove('is-invalid');
                }
            }
        });
    }
});