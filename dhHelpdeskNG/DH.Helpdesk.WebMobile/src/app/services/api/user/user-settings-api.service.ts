import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, finalize, take} from 'rxjs/operators';
import { UserData, Language, CurrentUser } from '../../../models'
import { LocalStorageService } from '../../local-storage'
import { TranslateService as NgxTranslateService } from '@ngx-translate/core'
import { LoggerService } from '../../logging';
import { Observable } from 'rxjs/Observable';
import * as moment from 'moment-timezone';
import { HttpApiServiceBase } from 'src/app/modules/shared-module/services/api/httpServiceBase';

@Injectable({ providedIn: 'root' })
export class UserSettingsApiService extends HttpApiServiceBase {
    isLoadingUserSettings = false; 
    
    protected constructor(
        protected http: HttpClient, 
        protected localStorageService: LocalStorageService,
        private ngxTranslationService: NgxTranslateService,
        private _logger: LoggerService) {
        super(http, localStorageService);
    }

    loadUserSettings(): Observable<CurrentUser> {
        this.isLoadingUserSettings = true;
        return this.getJson(this.buildResourseUrl('/api/currentuser/settings', undefined, false))//TODO: error handling
            .pipe(
                take(1),
                map((data: any) => {
                    if (data) {
                        let user = this.localStorageService.getCurrentUser();
                        if (data.id){
                          user.currentData.id = data.id;
                        }
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
                        return user;
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

    applyUserSettings() : Observable<any>{
        this.tryApplyDateTimeSettings();
        return this.tryLoadTranslations();
    }
    
    private tryLoadTranslations(): Observable<any> {
        const currentLangId = this.getCurrentLanguage();
        const languages = this.localStorageService.getLanguages();
        let languageKey = 'en'; // TODO: use config for default language?
        if (currentLangId && languages && languages.length )
        {
            const lang = languages.filter((l:Language) => l.id === currentLangId);
            languageKey = lang && lang.length ? lang[0].languageId : languageKey;
        }
        
        //change translations
        this._logger.log('>>> Settings translation language to: ' + languageKey);
        return this.ngxTranslationService.use(languageKey.toLowerCase());
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

        const browserLang = this.ngxTranslationService.getBrowserLang(); //Returns the language code name from the browser, e.g. "de", 'sv' 
        if (browserLang) {
/*             const availableLocales = moment.locales();
            this._logger.log(availableLocales); */
/*             if (availableLocales.filter(l => l == browserLang).length <= 0) {
                this._logger.log('>>>>Locale is not supported: ' + browserLang);
            } else {
 */             this._logger.log('>>>>Setting locale:  ' + navigator.language);
                moment.locale(browserLang.toLowerCase());
//            }
        }
    }
}
