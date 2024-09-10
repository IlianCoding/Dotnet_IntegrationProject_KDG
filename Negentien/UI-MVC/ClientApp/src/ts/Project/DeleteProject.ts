document.addEventListener('DOMContentLoaded', () => {
    const deleteButtons = document.querySelectorAll('.delete-button');

    deleteButtons.forEach(button => {
        button.addEventListener('click', (event) => {
            const target = event.target as HTMLButtonElement;
            const projectId = target.getAttribute('data-id');

            if (projectId) {
                deleteProject(parseInt(projectId));
            }
        });
    });
});

function deleteProject(projectId: number): void {
    if (confirm('Are you sure you want to delete this project?')) {
        fetch(`/api/Projects/Delete/${projectId}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(response => {
                if (response.ok) {
                    alert('Project deleted successfully.');
                    location.reload();
                } else {
                    return response.json().then(error => {
                        alert('Failed to delete project: ' + error.message);
                    });
                }
            })
            .catch(error => {
                console.error('Error deleting project:', error);
                alert('Failed to delete project. Please try again.');
            });
    }
}
