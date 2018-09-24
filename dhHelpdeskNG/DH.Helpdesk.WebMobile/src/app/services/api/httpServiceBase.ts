import { Observable } from "rxjs/Observable";
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { config } from "../../../environments/environment";
import { LocalStorageService } from "../local-storage";

export abstract class HttpApiServiceBase {
    private baseApiUrl: string;

    protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService) { 
        this.baseApiUrl = config.apiUrl;
    }

    protected buildResourseUrl(resourceName: string, params: object = undefined, addCustomerId = true) {
        let urlParams: string = null;
        if(addCustomerId === true) {
            let userData = this.localStorageService.getCurrentUser();
            if(userData !== null) {
                params = Object.assign({}, params || {}, {cid: userData.currentData.selectedCustomerId})
            }
        }
        if (params) {
            let str = Object.keys(params).map(function(key) {
                return key + '=' + encodeURIComponent(params[key]);
              }).join('&');

              urlParams = (str.length > 0 && resourceName.indexOf("?") < 0) ? "?" + str : "&" + str;
        }
        return `${this.baseApiUrl}${resourceName}${urlParams || ''}`;
    }
    
    protected getJson<TResponse>(url: string, headers?:any): Observable<TResponse> {
        return this.http
            .get<TResponse>(url, { headers: this.getHeaders(headers)})
            .pipe(
                catchError((error: any) => {
                    return throwError(error);
            }));
    }

    protected postJson<TResponse>(url: string, data: any, headers?:any): Observable<TResponse> {
        return this.http
            .post<TResponse>(url, JSON.stringify(data), { headers: this.getHeaders(headers) })
            .pipe(
                catchError((error: any) => {
                    return throwError(error);
            }));
    }

    protected postJsonNoContent(url: string, data: any, headers?: any): Observable<Object> { // fixed issue https://github.com/angular/angular/issues/18680 - remove after fix

        return this.http
            .post(url, JSON.stringify(data), { headers: this.getHeaders(headers), responseType: 'text' })
            .pipe(
                catchError((error: any) => {
                    return throwError(error);
            }));
    }
    
    private getHeaders(headers?: any): HttpHeaders {
        let options = new HttpHeaders();
        if (headers != null) {
            Object.getOwnPropertyNames(headers).forEach((v: string) => {
                options.set(v, headers[v]);
            });
        }
        
        options = options.set('Accept', 'application/json');
        options = options.set('Content-Type', 'application/json; charset=utf-8');
        options = options.set("Access-Control-Allow-Origin", "*");        

        return options;
    }

    protected addQsParam(qsParamMap:any, paramName:string, customerId:number) : any{        
        let newQsParams = {...qsParamMap, [paramName]: customerId };
        return newQsParams;
    }
}
