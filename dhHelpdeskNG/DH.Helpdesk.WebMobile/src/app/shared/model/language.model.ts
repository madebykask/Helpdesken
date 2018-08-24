
export interface ILanguage {
    languageId: string;
    name: string;
}
export class Language implements ILanguage {
    
    constructor(languageId: string, name: string) {            
        this.languageId = languageId;
        this.name = name;
    }    

    languageId: string;
    name: string;    
}