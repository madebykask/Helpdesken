import { UuidGenerator } from "../../helpers/uuid-generator";

export class CurrentUser {
    constructor() {
        this.authData = new UserAuthenticationData();
        this.currentData = new UserData();
    }

    id: number;
    version: string;
    authData: UserAuthenticationData;
    currentData: UserData;      

    static createAuthenticated(data: any) : CurrentUser {
        let authData = new UserAuthenticationData()
        authData.access_token = data.access_token;
        authData.refresh_token = data.refresh_token;
        authData.expires_in = data.expires_in;
        authData.recievedAt = new Date();
        authData.sessionId = UuidGenerator.createUuid();
        
        let user = new CurrentUser();
        user.authData = authData;

        return user;        
    }
}

export class UserAuthenticationData {
    access_token: string;
    refresh_token: string;
    expires_in: number; // in seconds - not updated once recieved
    recievedAt: Date;
    sessionId: string;   
}

export class UserData {
    selectedCustomerId: number;
    selectedLanguageId: number;
    userTimeZone?: string;
}