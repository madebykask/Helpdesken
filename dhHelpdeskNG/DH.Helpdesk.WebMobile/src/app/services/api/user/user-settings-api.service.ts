import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, finalize, take } from 'rxjs/operators';
import { UserData, Language, CurrentUser } from '../../../models';
import { LocalStorageService } from '../../local-storage';
import { TranslateService as NgxTranslateService } from '@ngx-translate/core';
import { LoggerService } from '../../logging';
import { Observable } from 'rxjs/Observable';
import { DateTime, Zone, Settings } from 'luxon';
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
    return this.getJson(this.buildResourseUrl('/api/currentuser/settings', undefined, false))
      .pipe(
        take(1),
        map((data: any) => {
          if (data) {
            const user = this.localStorageService.getCurrentUser();
            if (data.id) {
              user.currentData.id = data.id;
            }
            if (data.userRole) {
              user.currentData.userRole = +data.userRole;
            }
            if (data.userGuid) {
              user.currentData.userGuid = data.userGuid;
            }
            if (data.customerId) {
              user.currentData.selectedCustomerId = data.customerId;
            }// TODO: if no customer;
            if (data.languageId) {
              user.currentData.selectedLanguageId = data.languageId; // TODO: if no language
            }
            if (data.timeZone) {
              user.currentData.userTimeZone = data.timeZone;
            }
            user.currentData.createCasePermission = data.createCasePermission != null ? data.createCasePermission : false;
            user.currentData.canDeleteAttachedFiles = data.deleteAttachedFiles != null ? data.deleteAttachedFiles : false;
            user.currentData.caseOverviewRefreshInterval = data.caseOverviewRefreshInterval != null || data.caseOverviewRefreshInterval === 0 ?
              data.caseOverviewRefreshInterval :
              null;

            this.localStorageService.setCurrentUser(user);
            return user;
          } else {
            return null;
          }
        }),
        finalize(() => this.isLoadingUserSettings = false)
      );
  }

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

  applyUserSettings(): Observable<any> {
    this.tryApplyDateTimeSettings();
    return this.tryLoadTranslations();
  }

  private tryLoadTranslations(): Observable<any> {
    const currentLangId = this.getCurrentLanguage();
    const languages = this.localStorageService.getLanguages();
    let languageKey: string;
    if (languages && languages.length) {
      if (currentLangId) {
        const lang = languages.filter((l: Language) => l.id === currentLangId);
        languageKey = lang && lang.length ? lang[0].languageId : languages[0].languageId;
      } else {
        languageKey = languages[0].languageId;
      }
    }
    if (!languageKey) { 
      languageKey = 'en';
    }
    //change translations
    this._logger.log('>>> Settings translation language to: ' + languageKey);
    return this.ngxTranslationService.use(languageKey.toLowerCase());
  }

  private tryApplyDateTimeSettings(): boolean {
    const userTz = this.getUserTimezone();
    if (userTz == null) { return false; }

    this._logger.log('>>>>Setting date timezone: ' + userTz);
    const testTz = DateTime.local().setZone(userTz);

    if (!testTz.isValid) {
      this._logger.log('>>>>Error applying timezone: ' + testTz.invalidReason + '. Using browser default');
    } else {
      Settings.defaultZoneName = userTz;
    }

    const browserLang = this.ngxTranslationService.getBrowserLang(); //Returns the language code name from the browser, e.g. "de", 'sv'
    if (browserLang) {
      this._logger.log('>>>>Setting locale:  ' + navigator.language);
      Settings.defaultLocale = browserLang.toLowerCase();
    }
  }
}
