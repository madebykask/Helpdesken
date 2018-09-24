import { Observable } from "rxjs/Observable";
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { config } from "../../../environments/environment";

export abstract class HttpApiServiceBase {
    private baseApiUrl: string;

    protected constructor(private http: HttpClient,) { 
        this.baseApiUrl = config.apiUrl;
    }

    protected buildResourseUrl(resourceName: string, params: object = undefined) {
        let urlParams: string = null;
        if (params) {
            let str = Object.keys(params).map(function(key) {
                return key + '=' + encodeURIComponent(params[key]);
              }).join('&');

              if(str.length > 0) {
                  urlParams = "?" + str;
              }
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
