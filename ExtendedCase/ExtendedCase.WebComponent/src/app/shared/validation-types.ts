
export enum ValidateOn {
    OnSave,
    OnNext
}

export enum Trigger {
    Normal,
    OnCaseClose
}

export enum MinMax {
    Min,
    Max,
    Range
}

export class ValidatorError {
    constructor(public id: string,
        public type: string,
        public label: string,
        public mode: ValidateOn,
        public message: string,
        public trigger: Trigger) { }
}
