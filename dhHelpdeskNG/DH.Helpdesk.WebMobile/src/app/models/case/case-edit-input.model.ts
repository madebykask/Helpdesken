export class CaseEditInputModel { 
    Id: number;
    CaseNumber: number;
    Fields: IBaseCaseField<any>[];

    static fromJSON(json: any) : CaseEditInputModel {
        if (typeof json === 'string') { json = JSON.parse(json); } 
        return Object.assign(new CaseEditInputModel(), json, {
            Fields: (json.Fields as any[] || new Array()).map(v => {
                if (v.JsonType === "string") return BaseCaseField.fromJSON<string>(v);
                if (v.JsonType === "date") return BaseCaseField.fromJSON<Date>(v);
                if (v.JsonType === "number") return BaseCaseField.fromJSON<number>(v);
                return BaseCaseField.fromJSON<any>(v);
            })
        }); 
    }
}

export class BaseCaseField<T> implements IBaseCaseField<T> {
    public Name: string;
    public Label: string;
    //public JsonType: string;
    public Value?: T;
    public Section?: string;
    public Options: KeyValue[]

    static fromJSON<T>(json: any) : BaseCaseField<T> {
        if (typeof json === 'string') { json = JSON.parse(json); } 
        return Object.assign(new BaseCaseField<T>(), json, {
            Value: json.Value,
            Options: (json.Options as any[] || new Array()).map(v => {
                return KeyValue.fromJSON(v);
            })
        });
    }
}

export class KeyValue {
    public Key: string;
    public Value: string;

    static fromJSON(json: any) : KeyValue {
        if (typeof json === 'string') { json = JSON.parse(json); } 
        return Object.assign(new KeyValue(), json, {})
    }
}

export interface IBaseCaseField<T> {
    Name: string;
    Label: string;
    //JsonType: string;
    Value?: T;
    Section?: string;
    Options: KeyValue[]
}