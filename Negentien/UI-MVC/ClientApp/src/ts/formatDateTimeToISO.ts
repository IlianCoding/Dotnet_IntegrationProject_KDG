export function formatDateTimeToISO(date: Date): string {
    const year = date.getFullYear();
    const month = ('0' + (date.getMonth() + 1)).slice(-2); // Voeg een nul toe aan het begin en pak de laatste 2 cijfers
    const day = ('0' + date.getDate()).slice(-2); // Voeg een nul toe aan het begin en pak de laatste 2 cijfers
    const hours = ('0' + date.getHours()).slice(-2); // Voeg een nul toe aan het begin en pak de laatste 2 cijfers
    const minutes = ('0' + date.getMinutes()).slice(-2); // Voeg een nul toe aan het begin en pak de laatste 2 cijfers
    const seconds = ('0' + date.getSeconds()).slice(-2); // Voeg een nul toe aan het begin en pak de laatste 2 cijfers

    return `${year}-${month}-${day}T${hours}:${minutes}:${seconds}`;
}
