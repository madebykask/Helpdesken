import { CurrentUser, UserAuthenticationData, UserData, Language } from '../../models'
import * as moment from 'moment-timezone';
import { Injectable } from '@angular/core';
import { StorageNameConstants } from 'src/app/modules/shared-module/constants';
import { CaseSearchStateModel } from 'src/app/modules/shared-module/models/cases-overview/case-search-state.model';

@Injectable({providedIn: 'root'})
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

    getLanguages(): Language[] {
        let json = localStorage.getItem(StorageNameConstants.languages);
        let data= JSON.parse(json);
        return data;
    }

    saveLanguages(languages: Language[]) {
        let json = JSON.stringify(languages);
        localStorage.setItem(StorageNameConstants.languages, json);
    }

    saveTimezoneInfo(data: any) {
        const zone = <moment.MomentZone>data;
        const packed = (<any>moment.tz).pack(zone);
        localStorage.setItem(StorageNameConstants.timezoneName, packed);
    }

    getTimezoneInfo(): string {
        let data = localStorage.getItem(StorageNameConstants.timezoneName);
        return data;
    }

    getCaseSearchState(): CaseSearchStateModel { // TODO: move it to session store 
      let json = localStorage.getItem(StorageNameConstants.caseSearchState);
      if (json) {
        let data = JSON.parse(json);
        return data;
      };
      return null;
    }

    setCaseSearchState(data: CaseSearchStateModel ) { // TODO: move it to session store
        let json = JSON.stringify(data);
        localStorage.setItem(StorageNameConstants.caseSearchState, json);
    }

    private fromJSONCurrentUser(json: any): CurrentUser {
        if (typeof json === 'string') { json = JSON.parse(json); }
        return <CurrentUser>Object.assign(new CurrentUser(), json, {
            authData: this.fromJSONUserAuthenticationData(json.authData),
            currentData: this.fromJSONUserData(json.currentData)
        });
    }

    private fromJSONUserAuthenticationData(json: any): UserAuthenticationData {
        if (typeof json === 'string') { json = JSON.parse(json); }
        return <UserAuthenticationData>Object.assign(new UserAuthenticationData(), json, {
            recievedAt: new Date(json.recievedAt)
        });
    }

    private fromJSONUserData(json: any): UserData {
        if (typeof json === 'string') { json = JSON.parse(json); }
        return <UserData>Object.assign(new UserData(), json, {});
    }
}
