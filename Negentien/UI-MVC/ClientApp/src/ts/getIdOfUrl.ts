export function getIdOfUrl(url: string | null): number {
    if (url !== null) {
        let projectId = url.substring(url.lastIndexOf("/") + 1)
        return parseInt(projectId, 10) || 0;
    }
    return -1
}