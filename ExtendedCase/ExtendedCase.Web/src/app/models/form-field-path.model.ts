import * as commonMethods from '../utils/common-methods'

export class FormFieldPathModel {
    constructor(public tabId: string,
        public sectionId: string,
        public sectionInstanceIndex: number = 0,
        public fieldId: string) {

    }

    equals(item: FormFieldPathModel): boolean {
        if (!item)
            return false;

        return item.tabId === this.tabId &&
               item.sectionId === this.sectionId &&
               item.sectionInstanceIndex === this.sectionInstanceIndex && 
               item.fieldId === this.fieldId;
    }
    
    // two versions are supported at the moment:
    //ex1: tabs.tab1.sections.sec1.controls.fieldId
    //ex2: tabs.tab1.sections.sec1.instances[0].controls.fieldId
    static parse(path: string) {

        let regex = /tabs.(\w+).sections.(\w+)(.instances\[(\d+)\])?.controls.(\w+)/g;
        let match = regex.exec(path);
        
        if (match) {
            let tabId = match[1],
                sectionId = match[2],
                sectionInstanceIndex = 0,
                fieldId = match.length > 5 ? match[5] : '';

            if (match.length > 4) {
                let sectionInstanceIndexVal = commonMethods.isUndefinedNullOrEmpty(match[4]) ? 0 : parseInt(match[4]);
                sectionInstanceIndex = isNaN(sectionInstanceIndexVal) ? 0 : sectionInstanceIndexVal;
            }

            let formFieldPathModel = new FormFieldPathModel(tabId, sectionId, sectionInstanceIndex, fieldId);
            return formFieldPathModel;
        } else {
            throw `Failed to parse field path. Path: '${path}'`;
        }
    }

    buildFormFieldPath(singleSection: boolean = false): string {
        let path = singleSection
            ? `tabs.${this.tabId}.sections.${this.sectionId}.controls.${this.fieldId}`
            : `tabs.${this.tabId}.sections.${this.sectionId}.instances[${this.sectionInstanceIndex}].controls.${this.fieldId}`;
        return path;
    }

    buildSectionInstancePath(): string {
        let path = `tabs.${this.tabId}.sections.${this.sectionId}.instances[${this.sectionInstanceIndex}]`;
        return path;
    }
}
