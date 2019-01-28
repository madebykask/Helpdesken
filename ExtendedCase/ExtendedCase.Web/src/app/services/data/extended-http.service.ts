import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

@Injectable()
export class ExtendedHttpService {
    constructor(protected http: HttpClient) { }

    getJson<TResponse>(url: string, headers?:any): Observable<TResponse> {

        return this.http
            .get<TResponse>(url, { headers: this.getHeaders(headers)})
            .catch((error: any) => {
                return Observable.throw(error);
            });
    }

    postJson<TResponse>(url: string, data: any, headers?:any): Observable<TResponse> {

        return this.http
            .post<TResponse>(url, JSON.stringify(data), { headers: this.getHeaders(headers) })
            .catch((error: any) => {
                return Observable.throw(error);
            });
    }

    postJsonNoContent(url: string, data: any, headers?: any): Observable<Object> { // fixed issue https://github.com/angular/angular/issues/18680 - remove after fix

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

        return options;
    }
}
