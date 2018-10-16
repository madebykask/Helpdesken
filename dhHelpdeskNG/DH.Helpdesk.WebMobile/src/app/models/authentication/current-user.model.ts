export class CurrentUser {
    constructor() {
        this.authData = new UserAuthenticationData();
        this.currentData = new UserData();
    }

    id: number;
    version: string;
    authData: UserAuthenticationData;
    currentData: UserData;      
}

export class UserAuthenticationData {
    access_token: string;
    refresh_token: string;
    expires_in: number; // in seconds - not updated once recieved
    recievedAt: Date;

    static setData(dest: UserAuthenticationData, data: any) {
        dest.access_token = data.access_token;
        dest.expires_in = data.expires_in;
        dest.refresh_token = data.refresh_token;
        dest.recievedAt = new Date();
    }
}

export class UserData {
    selectedCustomerId: number;
    selectedLanguageId: number;
    userTimeZone?: string;
}