import { FormFieldPathModel } from '../models/form-field-path.model';

export interface IMap<T> {
    [key: string]: T;
}

export class ChangedFieldItem {
    constructor(
        public oldValue: any,
        public newValue: any,
        public fieldPath: FormFieldPathModel) {
    }
}

export enum LogLevel {
    Debug = 0,
    Info = 1,
    Warning = 2,
    Error = 3
}
