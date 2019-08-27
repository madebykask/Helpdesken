
export enum ValidateOn {
    OnSave,
    OnNext
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
        public message: string) { }
}
