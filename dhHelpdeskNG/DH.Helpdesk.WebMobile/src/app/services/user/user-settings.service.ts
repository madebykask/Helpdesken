import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, finalize} from 'rxjs/operators';
import { UserData } from '../../models'
import { LocalStorageService } from '../local-storage'
import { HttpApiServiceBase } from '../api'
import * as moment from 'moment-timezone/moment-timezone';
import { LoggerService } from '../logging';

@Injectable({ providedIn: 'root' })
export class UserSettingsService extends HttpApiServiceBase {
    isLoadingUserSettings = false; 
    
    protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService,
        private _logger: LoggerService) {
        super(http, localStorageService);
    }

    loadUserSettings() {
        this.isLoadingUserSettings = true;
        return this.getJson(this.buildResourseUrl('/api/currentuser/settings', undefined, false))//TODO: error handling
            .pipe(
                map((data: any) => {
                    if(data) {
                        let user = this.localStorageService.getCurrentUser();
                        if(data.customerId)
                            user.currentData.selectedCustomerId = data.customerId;//TODO: if no customer;
                        if(data.languageId)
                            user.currentData.selectedLanguageId = data.languageId;//TODO: if no language
                        if(data.timeZone)
                            user.currentData.userTimeZone = data.timeZone;
                        if(data.timeZoneMoment)
                            this.localStorageService.saveTimezoneInfo(data.timeZoneMoment);
                        // Other settings
                        this.localStorageService.setCurrentUser(user);

                        return user.currentData;
                    }
                }),
                finalize(() => this.isLoadingUserSettings = false )
            )
    };
    
    getUserData(): UserData {
        let user = this.localStorageService.getCurrentUser();
        if (!user) return null;

        return user.currentData
    }

    getCurrentLanguage(): number {
        return this.getUserData().selectedLanguageId;
    }

    setCurrentLanguage(languageId: number) {
        let user = this.localStorageService.getCurrentUser();
        user.currentData.selectedLanguageId = languageId;
        this.localStorageService.setCurrentUser(user);
    }

    getUserTimezone(): string {
        return this.getUserData().userTimeZone;
    }

    tryApplyTimezone(): boolean {
        const timezoneInfo = this.localStorageService.getTimezoneInfo();
        if (timezoneInfo == null) return false;
        const userTz = this.getUserTimezone();
        if (userTz == null) return false;

        moment.tz.add(timezoneInfo);
        this._logger.log('>>>>Setting date timezone: ' +userTz)
        moment.tz.setDefault(userTz);
        this._logger.log('>>>>Setting datetime L LT format')
        moment.defaultFormat = 'L LT';
    }

}