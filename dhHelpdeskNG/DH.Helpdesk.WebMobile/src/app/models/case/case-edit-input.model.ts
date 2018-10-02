export class CaseEditInputModel { 
    id: number;
    caseNumber: number;
    fields: IBaseCaseField<any>[];

    static fromJSON(json: any) : CaseEditInputModel {
        if (typeof json === 'string') { json = JSON.parse(json); } 
        return Object.assign(new CaseEditInputModel(), json, {
            Fields: (json.fields as any[] || new Array()).map(v => {
                if (v.JsonType === "string") return BaseCaseField.fromJSON<string>(v);
                if (v.JsonType === "date") return BaseCaseField.fromJSON<Date>(v);
                if (v.JsonType === "number") return BaseCaseField.fromJSON<number>(v);
                return BaseCaseField.fromJSON<any>(v);
            })
        }); 
    }
}

export class BaseCaseField<T> implements IBaseCaseField<T> {
    public name: string;
    public label: string;
    //public JsonType: string;
    public value?: T;
    public section?: string;
    public options: KeyValue[]

    static fromJSON<T>(json: any) : BaseCaseField<T> {
        if (typeof json === 'string') { json = JSON.parse(json); } 
        return Object.assign(new BaseCaseField<T>(), json, {
            value: json.value,
            options: (json.options as any[] || new Array()).map(v => {
                return KeyValue.fromJSON(v);
            })
        });
    }
}

export class KeyValue {
    public key: string;
    public value: string;

    static fromJSON(json: any) : KeyValue {
        if (typeof json === 'string') { json = JSON.parse(json); } 
        return Object.assign(new KeyValue(), json, {})
    }
}

export interface IBaseCaseField<T> {
    name: string;
    label: string;
    //JsonType: string;
    value?: T;
    section?: string;
    options: KeyValue[]
}