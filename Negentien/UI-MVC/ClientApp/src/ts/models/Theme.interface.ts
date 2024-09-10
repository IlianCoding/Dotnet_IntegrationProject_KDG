export interface Theme {
    id: number;
    subThemes: Theme[];
    shortInformation: string;
    themeName: string;
    isHeadTheme: boolean;
}