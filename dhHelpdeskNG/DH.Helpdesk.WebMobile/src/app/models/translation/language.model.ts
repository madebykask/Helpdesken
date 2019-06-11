
export interface ILanguage {
    id: number;
    languageId: string;
    name: string;
}
export class Language implements ILanguage {

    constructor(id: number, languageId: string, name: string) {
        this.id = id;
        this.languageId = languageId;
        this.name = name;
    }
    id: number;
    languageId: string;
    name: string;
}
