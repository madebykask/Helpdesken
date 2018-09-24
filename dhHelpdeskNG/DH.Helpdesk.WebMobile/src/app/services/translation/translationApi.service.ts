import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Language } from '../../models';
import { Observable } from 'rxjs/Observable';
import { HttpApiServiceBase } from '../api/httpServiceBase';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/observable/of';
import { LocalStorageService } from '../local-storage';

@Injectable()
export class TranslationApiService  extends HttpApiServiceBase {
    
    readonly Languages : { [key: string]: string } = {};

    constructor(protected http: HttpClient, protected localStorageService: LocalStorageService){
        super(http, localStorageService);
    }       

    getLanguages(): Observable<Language[]> {
        var methodUrl = this.buildResourseUrl('/api/Translation/Languages');
        return this.getJson<object[]>(methodUrl).mergeMap((res:any[]) => Observable.of(res.map((lang:any) => new Language(lang.LanguageId, lang.Name))));
    }

    getTranslations(lang: string): Observable<any> {
        console.log(">>loading translation for language: " + lang);
        var methodUrl = this.buildResourseUrl('/api/Translation/' + lang);        
        return this.getJson<any>(methodUrl);
    }
}
