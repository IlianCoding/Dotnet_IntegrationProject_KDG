import "../../scss/AttendantFlowDetail.scss"
import { Tab } from 'bootstrap';

document.addEventListener("DOMContentLoaded", () => {
    // Function to get the stored active tab from sessionStorage
    const getStoredActiveTab = (): string | null => {
        return sessionStorage.getItem("activeTab");
    };

    // Function to set the stored active tab in sessionStorage
    const setStoredActiveTab = (tabId: string): void => {
        sessionStorage.setItem("activeTab", tabId);
    };

    // Retrieve the stored active tab and activate it
    const activeTab = getStoredActiveTab();
    if (activeTab) {
        const tabElement = document.querySelector(`#pills-tab button[data-bs-target="${activeTab}"]`);
        if (tabElement) {
            const tabInstance = new Tab(tabElement);
            tabInstance.show();
        }
    }

    // Add event listeners to store the active tab on tab show event
    document.querySelectorAll('#pills-tab button[data-bs-toggle="pill"]').forEach(tabButton => {
        tabButton.addEventListener("shown.bs.tab", (event: Event) => {
            const target = event.target as HTMLElement;
            const targetTab = target.getAttribute("data-bs-target");
            if (targetTab) {
                setStoredActiveTab(targetTab);
            }
        });
    });
});

