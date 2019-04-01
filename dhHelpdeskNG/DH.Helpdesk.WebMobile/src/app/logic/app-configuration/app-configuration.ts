import { forkJoin, throwError } from 'rxjs'
import { TranslateService as NgxTranslateService, TranslateLoader } from '@ngx-translate/core'
import { UserSettingsApiService } from 'src/app/services/api/user/user-settings-api.service';
import { TranslationApiService } from 'src/app/services/api/translation/translation-api.service';
import { LocalStorageService } from 'src/app/services/local-storage';
import { LoggerService } from 'src/app/services/logging';
import { take, map, catchError } from 'rxjs/operators';

export function initApplication(
  ngxTranslateService: NgxTranslateService,
  userSettingsService: UserSettingsApiService,
  translationApiService: TranslationApiService,
  localStorage: LocalStorageService,
  logger: LoggerService) : Function {
    return () => {
      let userSettings$ = userSettingsService.applyUserSettings();
      let translation$ = translationApiService.getLanguages();

      return forkJoin(userSettings$, translation$).pipe(
          take(1),
          map(([userSettings, langs]) => {
            // add languages to the inner collection of supported languages to switch into
            ngxTranslateService.addLangs(langs.map(s => s.languageId.toLowerCase()));

            // save existing languages to the storage
            localStorage.saveLanguages(langs);
          }),
          catchError((e) => throwError(e))
        ).toPromise();
      };
 }