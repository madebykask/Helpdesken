export class ClientLogEntryModel {
    uniqueId: string;
    isAuthenticated: boolean;
    sessionId: string;
    url: string;
    level: ClientLogLevel;
    message: string;
    stack: string;
    param1: string;
    param2: string;
    param3: string;
}

export enum ClientLogLevel {
    Debug = 0,
    Info = 1,
    Warning = 2,
    Error = 3
}