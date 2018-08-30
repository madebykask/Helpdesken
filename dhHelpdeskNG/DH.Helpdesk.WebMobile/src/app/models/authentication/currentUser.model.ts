export class CurrentUser {
    constructor() {
        this.authData = new UserAuthenticationData();
        this.currentData = new UserData();
    }

    id: number;
    authData: UserAuthenticationData;
    currentData: UserData;    

    static fromJSON(data: any) : CurrentUser {
        var obj = JSON.parse(data);
        var user = new CurrentUser()

        user.authData.access_token = obj.authData.access_token;
        user.authData.expires_in = Number(obj.authData.expires_in);
        user.authData.recievedAt = new Date(obj.authData.recievedAt);
        user.authData.refresh_token = obj.authData.refresh_token;

        user.currentData.selectedCustomerId = obj.currentData.selectedCustomerId;

        return user;
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

    static fromJSON(data: any) {
        return Object.assign(new this, data) as UserAuthenticationData;
    }
}

export class UserData {
    selectedCustomerId: number;
}