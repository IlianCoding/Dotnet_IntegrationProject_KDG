// document.addEventListener('DOMContentLoaded', function() {
//     const deleteButtons = document.querySelectorAll('.btn-primary');
//     deleteButtons.forEach(button => {
//         button.addEventListener('click', function(event) {
//             const deleteButton = event.target as HTMLButtonElement;
//             const row = deleteButton.closest('tr');
//             if (row) {
//                 const projectId = row.dataset.projectId;
//                 const themeName = row.dataset.themeName;
//                 deleteTheme(Number(projectId), String(themeName));
//                 row.remove(); // Verwijder de rij direct van de tabel
//             } else {
//                 console.error('Row not found.');
//             }
//         });
//     });
// });
const projectIdInput = document.getElementById("projectId") as HTMLInputElement;
document.addEventListener('DOMContentLoaded', function () {
    const deleteButtons = document.querySelectorAll('.btn-primary');
    deleteButtons.forEach(button => {
        button.addEventListener('click', function (event) {
            const deleteButton = event.target as HTMLButtonElement;
            const row = deleteButton.closest('tr');
            if (row) {
                const themeName = row.querySelector('td:first-child')?.textContent; // Selecteer de tekst van de eerste <td> in de rij
                if (themeName) {
                    console.log("Theme Name:", themeName.trim());
                    deleteTheme(Number(projectIdInput.value), themeName.trim())
                    row.remove();
                } else {
                    console.error('Theme Name not found.');
                }
            } else {
                console.error('Row not found.');
            }
        });
    });
});


function deleteTheme(projectId: number, themeName: string) {
    fetch("/api/Themes", {
        method: "DELETE",
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            "ProjectId": projectId,
            "ThemeName": themeName
        })
    }).then(response => {
        if (!response.ok) {
            throw new Error(`Delete failed: ${response.status}`);
        }
        return response.json();
    })
}

