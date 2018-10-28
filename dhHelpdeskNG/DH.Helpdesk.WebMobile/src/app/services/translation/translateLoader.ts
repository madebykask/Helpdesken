import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs'
import { TranslateService as NgxTranslateService, TranslateLoader } from '@ngx-translate/core'
import { TranslationApiService } from './translationApi.service';
import { LoggerService } from '../logging';
import { LocalStorageService } from '../local-storage';
import { Language } from 'src/app/models';
import { take, switchMap } from 'rxjs/operators';

export function initTranslation(
     ngxTranslateService: NgxTranslateService,
     translationApiService: TranslationApiService,
     localStorage: LocalStorageService,
     logger: LoggerService) : Function {
    return () =>               
       translationApiService.getLanguages().pipe(
            take(1),
            switchMap((langs: Language[]) => {

                //add languages to the inner collection of supported languages to switch into
                ngxTranslateService.addLangs(langs.map(s => s.languageId.toLowerCase()));  
        
                //save existing languages to the storage
                localStorage.saveLanguages(langs);
        
                let languageKey = 'en'; //default language
                let curUser = localStorage.getCurrentUser();
                if (curUser && curUser.currentData && langs.length) {
                    let currentLangId = curUser.currentData.selectedLanguageId;
                    let lang = langs.filter((l:Language) => l.id === currentLangId);
                    languageKey = lang.length ? lang[0].languageId : languageKey;
                }
            
                // load translations for language
                return ngxTranslateService.use(languageKey);        
            })
        ).toPromise();    
 }

@Injectable()
export class CustomTranslateLoader implements TranslateLoader  {   
    
    constructor(private translationApiService : TranslationApiService) {        
        //this.logger.log('CustomTranslateLoader created.')
    }

    getTranslation(lang: string): Observable<Object>{
        console.log('>>>Loading translations for: ' + lang);
        return this.translationApiService.getTranslations(lang);         
    }
}

export function HttpLoaderFactory(translationApiService : TranslationApiService) {
    return new CustomTranslateLoader(translationApiService);
    //return new TranslateHttpLoader(http);  
}