import { Injectable, Inject, InjectionToken } from '@angular/core';
import { Observable } from 'rxjs'
import { TranslateService as NgxTranslateService, TranslateLoader } from '@ngx-translate/core'
import { TranslationApiService } from './translationApi.service';
import { LoggerService } from '../logging';
import { config } from '../../../environments/environment';

export function initTranslation(ngxTranslateService: NgxTranslateService,
     translationApiService: TranslationApiService,
     logger: LoggerService) : Function {
    return () =>       
        new Observable(observer => {
            var languages = translationApiService.Languages;
            translationApiService.getLanguages().subscribe(langs => {
                langs.map(lang => {
                    languages[lang.languageId.toLowerCase()] = lang.name;
                });

                //adds languages to the inner collection - we later can use  it to show it on UI: translationService.lang    
                ngxTranslateService.addLangs(Object.keys(languages).map(s => s.toLowerCase())); 

                ngxTranslateService.use('en'); //todo:replace with constant/config value
                logger.log(`>> translation has been initialised. Langs: ${JSON.stringify(ngxTranslateService.langs)}` );
                observer.next(null); 
                observer.complete();
        });
    }).toPromise();
 }

@Injectable()
export class CustomTranslateLoader implements TranslateLoader  {   
    
    constructor(private translationApiService : TranslationApiService) {        
        //this.logger.log('CustomTranslateLoader created.')
    }

    getTranslation(lang: string): Observable<Object>{
        //console.log('>>Loading translations for: ' + lang);
        return this.translationApiService.getTranslations(lang);         
    }
}

export function HttpLoaderFactory(translationApiService : TranslationApiService) {
    return new CustomTranslateLoader(translationApiService);
    //return new TranslateHttpLoader(http);  
}