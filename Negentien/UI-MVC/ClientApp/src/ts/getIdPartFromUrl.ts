export function getIdPartFromUrl(param: string): number | null {
    try {
        const params = new URLSearchParams(window.location.search);
        const paramLower = param.toLowerCase();

        for (const [key, value] of params.entries()) {
            if (key.toLowerCase() === paramLower) {
                const parsedValue = parseInt(value, 10);
                if (!isNaN(parsedValue)) {
                    return parsedValue;
                }
            }
        }

        return null;
    } catch (error) {
        console.error('Error parsing URL parameter:', error);
        return null;
    }
}
