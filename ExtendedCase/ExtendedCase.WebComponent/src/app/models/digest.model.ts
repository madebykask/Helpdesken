import { FormFieldPathModel } from './form-field-path.model';
import { ChangedFieldItem } from '../shared/common-types';

export class DigestUpdateLog {
    logs: DigestUpdateLogItem[];

    constructor() {
        this.logs = [];
    }

    add(fieldPath: FormFieldPathModel, oldValue: any, newValue: any): void {
        this.logs.push(new DigestUpdateLogItem(fieldPath, oldValue, newValue));
    }
     
    contains(id: string): boolean {
        return this.logs.findIndex((x) => {
            return x.fieldPath.fieldId === id;
        }) > -1;
    }

    containsPath(fieldPath: FormFieldPathModel): boolean {                
        return this.logs.findIndex(x => x.fieldPath.equals(fieldPath)) > -1;
    }

    clear() {
        this.logs = [];
    }
}

export class DigestUpdateLogItem {

    constructor(public fieldPath: FormFieldPathModel, public oldValue: any, public newValue: any) {
    }

    get id():string {
        return this.fieldPath.buildFormFieldPath(true);
    }

    get multiId(): string {
        return this.fieldPath.buildFormFieldPath(false);
    }

    getTabId() : string {
        return this.fieldPath.tabId;
    }

    getSectionId(): string {
        return this.fieldPath.sectionId;
    }

    getSectionInstanceIndex(): number {
        return this.fieldPath.sectionInstanceIndex;
    }

    getControlId(): string {
        return this.fieldPath.fieldId;
    }
}

export class DigestResult {
    constructor(public result: boolean, public digestUpdateLog: DigestUpdateLog) {

    }
}

export class DigestResultContext {
    constructor(
        public isInitial: boolean,
        public result: DigestResult,
        public changedFields: ChangedFieldItem[] = [],
        public isNewFields: boolean) {
        
    }

    static create(isInitial: boolean, result: DigestResult, changedFields: ChangedFieldItem[] = null, isNewFields: boolean = false) {
        return new DigestResultContext(isInitial, result, changedFields, isNewFields);
    }
}