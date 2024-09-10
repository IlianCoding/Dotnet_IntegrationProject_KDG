import '../../scss/stepDetail.scss';
function getStepId2() {
    const urlSearchParams = new URLSearchParams(window.location.search)
    let stepId = urlSearchParams.get("stepId")
    if (!stepId) {
        const path = window.location.pathname
        stepId = path.substring(path.lastIndexOf("=") + 1)
    }
    return parseInt(stepId)
}

const deleteCPButton = document.getElementById("delete-cp");

deleteCPButton!.addEventListener('click', async function () {
        const cpId = parseInt(this.getAttribute("cp-id")!);
        const response = await fetch(`/api/ConditionalPoints/RemoveCP/${cpId}`, {
            method: "DELETE",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            }
        });

        // Redirect upon successful deletion
        if (response.ok) {
            window.location.href = `/Step/StepDetail?stepId=${getStepId2()}`;
        } else {
            console.error('Failed to delete Conditional Point');
        }
    });
      