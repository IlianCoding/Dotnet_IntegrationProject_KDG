import { getIdPartFromUrl } from "../getIdPartFromUrl";

document.addEventListener("DOMContentLoaded", () : void => {
    const submitBtn: HTMLButtonElement = document.getElementById("submitBtn") as HTMLButtonElement;
    const userEmailInput: HTMLInputElement = document.getElementById("userEmail") as HTMLInputElement;
    const colorSelect: HTMLSelectElement = document.getElementById("colorSelect") as HTMLSelectElement;

    const colorMap: { [key: string]: number } = {
        "Red": 0,
        "Green": 1,
        "Blue": 2,
        "Yellow": 3,
        "Orange": 4,
        "Pink": 5,
        "None": 6
    };

    submitBtn.addEventListener("click", async (event: MouseEvent) : Promise<void> => {
        event.preventDefault();

        const runningFlowId: number | null = getIdPartFromUrl('runningFlowId');
        const userEmail: string = userEmailInput.value;
        const colorName: string = colorSelect.value;

        if (!userEmail || !colorName) {
            alert("Please fill out all fields.");
            return;
        }

        const color : number = colorMap[colorName];

        const data = {
            userEmail,
            color
        };

        try {
            const response : Response = await fetch(`/api/EndUsers/LinkFinishedSessionToExistingUser?runningFlowId=${runningFlowId}`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(data)
            });

            if (response.ok) {
                alert("Successfully linked session.");
            } else {
                alert("Failed to link session. Please try again.");
            }
        } catch (error) {
            console.error("Error:", error);
            alert("An error occurred. Please try again.");
        }
    });
});
