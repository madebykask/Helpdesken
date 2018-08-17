import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { TranslateService, TranslateLoader } from '@ngx-translate/core'
import { Observable } from 'rxjs'

export const Languages : { [key: string]: string } = {};

export function initTranslation(translationService: TranslateService) : Function {
    return () => {  
    
     //todo: should be loaded from the database and availiable via shared config
     Languages['en'] = "English";
     Languages['swd'] = 'Swedish';
     
     translationService.addLangs(Object.keys(Languages)); //add languages to the inner collection - we later can use  it to show it on UI: translationService.lang    
     translationService.use('en').toPromise(); //todo:replace with constant/config
   };
 } 

@Injectable()
export class CustomTranslateLoader implements TranslateLoader  {
    
    contentHeader = new HttpHeaders({"Content-Type": "application/json","Access-Control-Allow-Origin":"*"});

    constructor(private http: HttpClient) {        
        console.log('CustomTranslateLoader created.')
    }

    getTranslation(lang: string): Observable<Object>{
        console.log('>>Loading translations for: ' + lang);

        //var apiAddress = AppConfig.API_URL+"/static/i18n/"+ lang+".json";
        var apiAddress = "http://localhost:8049/api/Test/Language/" + lang;
        
        return Observable.create(observer => {
            return this.http.get(apiAddress, { headers: this.contentHeader }).subscribe(                
                res => {
                    console.log('>>Translations have been loaded successfully for: ' + lang);
                    console.log(res);
                    observer.next(res);
                    observer.complete();
                },
                error => {
                    //  failed to retrieve from api, switch to local
                    console.error("Failed to load translation for lang: " + lang);

                    this.http.get("/assets/i18n/en.json").subscribe((res: Object) => {
                        observer.next(res);
                        observer.complete();
                })
            });
        }); 
    }
}

export function HttpLoaderFactory(http: HttpClient) {
    return new CustomTranslateLoader(http);
    //return new TranslateHttpLoader(http);  
}