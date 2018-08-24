import { Observable } from "rxjs/Observable";
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { HttpClient, HttpHeaders } from "@angular/common/http";

export abstract class HttpApiServiceBase {

    protected constructor(private http: HttpClient,private baseApiUrl: string) { 
    }

    protected buildResourseUrl(resourceName: string){
        return this.baseApiUrl + resourceName;
    }

    protected getJson<TResponse>(url: string, headers?:any): Observable<TResponse> {
        return this.http
            .get<TResponse>(url, { headers: this.getHeaders(headers)})
            .catch((error: any) => {
                return Observable.throw(error);
            });
    }

    protected postJson<TResponse>(url: string, data: any, headers?:any): Observable<TResponse> {

        return this.http
            .post<TResponse>(url, JSON.stringify(data), { headers: this.getHeaders(headers) })
            .catch((error: any) => {
                return Observable.throw(error);
            });
    }

    protected postJsonNoContent(url: string, data: any, headers?: any): Observable<Object> { // fixed issue https://github.com/angular/angular/issues/18680 - remove after fix

        return this.http
            .post(url, JSON.stringify(data), { headers: this.getHeaders(headers), responseType: 'text' })
            .catch((error: any) => {
                return Observable.throw(error);
            });
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
}
