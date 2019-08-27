export class ClientLogEntryModel {
    UniqueId: string;
    Url: string;
    Level: ClientLogLevel;
    Message: string;
    Stack: string;
    Param1: string;
    Param2: string;
    Param3: string;
}

export enum ClientLogLevel {
    Debug = 0,
    Info = 1,
    Warning = 2,
    Error = 3
}