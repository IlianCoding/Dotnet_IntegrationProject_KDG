const checkbox: HTMLInputElement | null = document.getElementById('myCheckbox') as HTMLInputElement;
const form: HTMLElement | null = document.getElementById('myForm');
const submittingButton: HTMLElement | null = document.getElementById('submitButton') as HTMLElement;

if (checkbox && form && submittingButton) {
    checkbox.addEventListener('change', function() {
        if (checkbox.checked) {
            form.style.display = 'block';
            submittingButton.style.display = 'none';
        } else {
            form.style.display = 'none';
            submittingButton.style.display = 'none';
        }
    });
} else {
    console.error("One of the elements (checkbox, form, or submitButton) is not found.");
}
