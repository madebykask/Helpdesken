export class CurrentUser {
    constructor() {
        this.authData = new UserAuthenticationData();
        this.currentData = new UserData();
    }

    id: number;
    authData: UserAuthenticationData;
    currentData: UserData;    

    static fromJSON(json: any) : CurrentUser {
        if (typeof json === 'string') { json = JSON.parse(json); } 
        return Object.assign(new CurrentUser(), json, {
            authData: UserAuthenticationData.fromJSON(json.authData),
            currentData: UserData.fromJSON(json.currentData)
        });  
    }
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

    static fromJSON(json: any) : UserAuthenticationData {
        if (typeof json === 'string') { json = JSON.parse(json); } 
        return Object.assign(new UserAuthenticationData(), json, {
            recievedAt: new Date(json.recievedAt)
        });
    }
}

export class UserData {
    selectedCustomerId: number;
    selectedLanguageId: number;

    static fromJSON(json: any) : UserData {
        if (typeof json === 'string') { json = JSON.parse(json); } 
        return Object.assign(new UserData(), json, {}); 
    }
}