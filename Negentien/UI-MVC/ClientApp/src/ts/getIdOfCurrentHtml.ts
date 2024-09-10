export function getIdOfCurrentHtml() {
    const urlSearchParams = new URLSearchParams(window.location.search)
    let projectId = urlSearchParams.get('id')
    if (!projectId) {
        const path = window.location.pathname
        projectId = path.substring(path.lastIndexOf("/") + 1)
    }
    return parseInt(projectId, 10) || 0;
}