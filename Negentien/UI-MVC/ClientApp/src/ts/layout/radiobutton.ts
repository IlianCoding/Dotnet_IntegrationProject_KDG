
document.getElementById("flexSwitchCheckDefault")!.addEventListener("change", event => {
    changeLabelContent(event)
});

function changeLabelContent(event: Event) {
    const label = <HTMLElement>document.getElementById('label-status');
    
    let newContent: string;
    if ((event.target as HTMLInputElement).checked) {
        newContent = "Flow Enabled"
    } else {
        newContent = "Flow Disabled"
    }
    
    label.innerHTML = newContent;
}

document.getElementById("isLinear")!.addEventListener("change", event => {
    changeLabelLinear(event)
});

function changeLabelLinear(event: Event) {
    const label = <HTMLElement>document.getElementById('label-is-linear');
    
    let newContent: string;
    if ((event.target as HTMLInputElement).checked) {
        newContent = "Linear Flow"
    } else {
        newContent = "Circular Flow"
    }

    label.innerHTML = newContent;
}