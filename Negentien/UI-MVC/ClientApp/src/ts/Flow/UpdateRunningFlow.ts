export function updateRunningFlow(runningFlowId: number, state: number) {
    if (state < 0 || state > 3) {
        return;
    }
    fetch(`/api/RunningFlows/${runningFlowId}/update`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            "State": state
        })
    }).then(response => {
        if (!response.ok) {
            throw new Error("Update runningFlow failed: " + response.status)
        }
        // window.location.reload();
    }).catch(function (error) {
        console.error(error)
    })
}