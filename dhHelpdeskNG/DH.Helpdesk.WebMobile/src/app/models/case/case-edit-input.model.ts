export class CaseEditInputModel { 
    id: number;
    caseNumber: number;
    fields: IBaseCaseField<any>[];

    
}

export class BaseCaseField<T> implements IBaseCaseField<T> {
    public name: string;
    public label: string;
    //public JsonType: string;
    public value?: T;
    public section?: string;
    public options: KeyValue[]
}

export class KeyValue {
    public key: string;
    public value: string;
}

export interface IBaseCaseField<T> {
    name: string;
    label: string;
    //JsonType: string;
    value?: T;
    section?: string;
    options: KeyValue[]
}