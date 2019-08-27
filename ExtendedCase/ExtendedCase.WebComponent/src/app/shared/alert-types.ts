
export class Alert {
    constructor(public type: AlertType, public message: string) {
    }
}

export enum AlertType {
    Success = 0,
    Info = 1,
    Warning = 2,
    Error = 3
}