import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Language } from '../../../models';
import { Observable } from 'rxjs/Observable';
import { HttpApiServiceBase } from '../../../modules/shared-module/services/api/httpServiceBase';
import { LocalStorageService } from '../../local-storage';
import { take, map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class TranslationApiService  extends HttpApiServiceBase {
    
    constructor(protected http: HttpClient, protected localStorageService: LocalStorageService){
        super(http, localStorageService);
    }       

    getLanguages(): Observable<Language[]> {
        // console.log(">>>loading languages");
        var methodUrl = this.buildResourseUrl('/api/Translation/Languages');
        return this.getJson<object[]>(methodUrl, null, true)
                .pipe(    
                    map((res:any[]) => res.map((lang:any) => new Language(lang.id, lang.languageId, lang.name)))
                );        
    }

    getTranslations(lang: string): Observable<any> {
        //console.log(">>>loading translation for language: " + lang);
        var methodUrl = this.buildResourseUrl(`/api/Translation/mobile/${lang}`);
        return this.getJson<any>(methodUrl, null, true);
    }
}
