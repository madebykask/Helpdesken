import { Injectable, ModuleWithComponentFactories } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, finalize, take} from 'rxjs/operators';
import { UserData, Language } from '../../models'
import { LocalStorageService } from '../local-storage'
import { HttpApiServiceBase } from '../api'
import { TranslateService as NgxTranslateService, TranslateLoader } from '@ngx-translate/core'
import { LoggerService } from '../logging';
import * as moment from 'moment-timezone';
import { Subject } from 'rxjs/Subject';



@Injectable({ providedIn: 'root' })
export class UserSettingsService extends HttpApiServiceBase {
    isLoadingUserSettings = false; 
    
    private userSettingsLoadedSubject = new Subject();
    userSettingsLoaded$ = this.userSettingsLoadedSubject.asObservable();

    protected constructor(
        protected http: HttpClient, 
        protected localStorageService: LocalStorageService,
        private ngxTranslationService: NgxTranslateService,
        private _logger: LoggerService) {
        super(http, localStorageService);
    }

    loadUserSettings() {
        this.isLoadingUserSettings = true;
        return this.getJson(this.buildResourseUrl('/api/currentuser/settings', undefined, false))//TODO: error handling
            .pipe(
                take(1),
                map((data: any) => {
                    if (data) {
                        let user = this.localStorageService.getCurrentUser();
                        if (data.customerId) {
                            user.currentData.selectedCustomerId = data.customerId;
                        }//TODO: if no customer; 
                        if (data.languageId) {
                            user.currentData.selectedLanguageId = data.languageId;//TODO: if no language
                        }
                        if (data.timeZone) {
                            user.currentData.userTimeZone = data.timeZone; 
                        }
                        if (data.timeZoneMoment) {
                            this.localStorageService.saveTimezoneInfo(data.timeZoneMoment);
                        }
                        // Other settings
                        //console.log('>>> user data loaded sucessfully');
                        this.localStorageService.setCurrentUser(user);
                        
                        //raise load event
                        this.userSettingsLoadedSubject.next();

                        return user.currentData;
                    }
                    else {
                        return null;
                    }
                }),
                finalize(() => this.isLoadingUserSettings = false )
            )
    };

    getUserData(): UserData {
        const user = this.localStorageService.getCurrentUser();
        return user && user.currentData ? user.currentData : null;
    }

    getCurrentLanguage(): number {
        const user = this.getUserData();
        return user != null ? user.selectedLanguageId : null;
    }

    setCurrentLanguage(languageId: number) {
        const user = this.localStorageService.getCurrentUser();
        user.currentData.selectedLanguageId = languageId;
        this.localStorageService.setCurrentUser(user);
    }

    getUserTimezone(): string {
        const user = this.getUserData();
        return user != null ? user.userTimeZone : null;
    }

    applyUserSettings(){
        this.tryApplyDateTimeSettings();
        this.tryLoadTranslations();
    }
    
    private tryLoadTranslations() {
        const currentLangId = this.getCurrentLanguage();
        const languages = this.localStorageService.getLanguages();
        const lang = languages.filter((l:Language) => l.id === currentLangId);

        let languageKey  = lang.length ? lang[0].languageId : 'en';

        //change translations        
        //console.log('>>> Settings translation language to: ' + languageKey);
        this.ngxTranslationService.use(languageKey);         
    }

    private tryApplyDateTimeSettings(): boolean {
        //console.log('>>> tryApplyDateTimeSettings: called');
        const timezoneInfo = this.localStorageService.getTimezoneInfo();
        if (timezoneInfo == null) { return false };
        const userTz = this.getUserTimezone();
        if (userTz == null) { return false };

        moment.tz.add(timezoneInfo);
        this._logger.log('>>>>Setting date timezone: ' + userTz);
        moment.tz.setDefault(userTz);
        this._logger.log('>>>>Setting datetime L LT format');
        (<any>moment).defaultFormat = 'L LT';// <any> hack to avoid warning about constant
        if (navigator.language != null) {
/*             const availableLocales = moment.locales();
            this._logger.log(availableLocales); */
/*             if (availableLocales.filter(l => l == navigator.language).length <= 0) {
                this._logger.log('>>>>Locale is not supported: ' + navigator.language);
            } else {
 */             this._logger.log('>>>>Setting locale ' + navigator.language);
                moment.locale(navigator.language);
//            }
        }
    }
}
