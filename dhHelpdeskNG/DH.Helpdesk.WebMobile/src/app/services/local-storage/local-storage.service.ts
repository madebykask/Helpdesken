import { CurrentUser, UserAuthenticationData, UserData, TimeZoneInfo } from '../../models'
import { StorageNameConstants } from '../../helpers/constants'
import * as moment from 'moment-timezone';

export class LocalStorageService {
    constructor() {}

    getCurrentUser(): CurrentUser {
        let currentUser = localStorage.getItem(StorageNameConstants.userDataStorageName);
        if(currentUser) {
            return this.fromJSONCurrentUser(currentUser);
        }

        return null;
    }

    setCurrentUser(user: CurrentUser) {
        localStorage.setItem(StorageNameConstants.userDataStorageName, JSON.stringify(user));
    }

    removeCurrentUser() {
        localStorage.removeItem(StorageNameConstants.userDataStorageName);
    }

    saveTimezoneInfo(data: any) {
        const zone = <moment.MomentZone>data;
        const packed = moment.tz.pack(zone);
        localStorage.setItem(StorageNameConstants.timezoneName, packed);
    }

    getTimezoneInfo(): string {
        let data = localStorage.getItem(StorageNameConstants.timezoneName);
        return data;
    }

    private fromJSONCurrentUser(json: any) : CurrentUser { 
        if (typeof json === 'string') { json = JSON.parse(json); } 
        return <CurrentUser>Object.assign(new CurrentUser(), json, {
            authData: this.fromJSONUserAuthenticationData(json.authData),
            currentData: this.fromJSONUserData(json.currentData)
        });  
    }

    private fromJSONUserAuthenticationData(json: any) : UserAuthenticationData {
        if (typeof json === 'string') { json = JSON.parse(json); } 
        return <UserAuthenticationData>Object.assign(new UserAuthenticationData(), json, {
            recievedAt: new Date(json.recievedAt)
        });
    }

    private fromJSONUserData(json: any) : UserData {
        if (typeof json === 'string') { json = JSON.parse(json); } 
        return <UserData>Object.assign(new UserData(), json, {}); 
    }
}