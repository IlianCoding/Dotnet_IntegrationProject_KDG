import { Alert } from 'bootstrap';

document.addEventListener('DOMContentLoaded', () => {
    const reassignButtons = document.querySelectorAll('.reassignButton');

    reassignButtons.forEach((button, index) => {
        button.addEventListener('click', async () => {
            const confirmReassignment = window.confirm('Are you sure you want to reassign this project?');
            if (!confirmReassignment) {
                return;
            }

            const formId = `reassignProjectForm-attendant-${index}`;
            const form = document.getElementById(formId) as HTMLFormElement;
            if (!form) {
                console.error(`Form with ID ${formId} not found`);
                return;
            }

            const emailInput = form.querySelector('#attendantEmail') as HTMLInputElement;
            const projectSelect = form.querySelector('#projectSelect') as HTMLSelectElement;

            const email = emailInput.value;
            const projectId = parseInt(projectSelect.value, 10);

            const dto: AssignProjectDto = {
                email,
                projectId,
            };

            try {
                await reassignProject(dto);
                alert('Project successfully reassigned.');
                location.reload();
            } catch (error) {
                console.error('Error reassigning project:', error);
                alert('Error reassigning project. Please try again.');
            }
        });
    });
});

interface AssignProjectDto {
    email: string;
    projectId: number;
}

async function reassignProject(dto: AssignProjectDto): Promise<void> {
    const response : Response = await fetch('/api/Organizations/ReassignProject', {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(dto),
    });
    if (!response.ok) {
        throw new Error(`Error reassigning project: ${await response.text()}`);
    }
}