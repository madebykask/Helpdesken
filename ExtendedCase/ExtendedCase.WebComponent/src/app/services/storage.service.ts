import { Injectable } from '@angular/core';
import { WindowWrapper } from '../shared/window-wrapper'

@Injectable()
export class StorageService {
    private nativeWindow: any;
    private tokenDataKey: string = 'TokenData';
    private globalDataKey: string = 'GlobalData';

    constructor(private windowWrapper: WindowWrapper) {
        this.nativeWindow = windowWrapper.nativeWindow;
    }

    getAuthToken(): string {
        let token = '';
        let tokenData = this.getTokenData();
        if (tokenData && tokenData.hasOwnProperty('AccessToken')) {
            token = tokenData.AccessToken;
        }
        return token;
    }

    getRefreshToken(): string {
        let token = '';
        let tokenData = this.getTokenData();
        if (tokenData && tokenData.hasOwnProperty('RefreshToken')) {
            token = tokenData.RefreshToken;
        }
        return token;
    }

    getCaseId(): number {
        let caseId = 0;
        let globalData = this.getGlobalData();
        if (globalData && globalData.hasOwnProperty('CaseId')) {
            caseId = parseInt(globalData.CaseId);
            if (isNaN(caseId))
                caseId = 0;
        }
        return caseId;
    }

    // token data
    private getTokenData():any {
        let tokenData: any = null;
        var val = this.getLocalItem('TokenData');
        if (val) {
            try {
                tokenData = JSON.parse(val);
            }
            catch(err) {
            }
        }
        return tokenData;
    }

    private saveTokenData(data:any) {
        let val = JSON.stringify(data);
        this.setLocalItem('TokenData', val);
    }

    //  global data
    private getGlobalData():any {
        let globalData: any = null;
        let val = this.getSessionItem('GlobalData');
        if (val) {
            try {
                globalData = JSON.parse(val);
            } catch (err) {

            }
         }

        return globalData;
    }

    private setGlobalData(data:any) {
        var val = JSON.stringify(data);
        this.setSessionItem('GlobalData', val);
    }
    

    // Local storage
    private setLocalItem(key: string, value: string) {
        this.nativeWindow.localStorage.setItem(key, value);
    }

    private getLocalItem(key: string): string {
        return this.nativeWindow.localStorage.getItem(key);
    }


    // Session storage
    private setSessionItem(key: string, value: string) {
       this.nativeWindow.sessionStorage.setItem(key, value);
    }

    private getSessionItem(key: string): string {
        return this.nativeWindow.sessionStorage.getItem(key);
    }
}