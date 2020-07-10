
import { throwError, Observable } from 'rxjs';
import {catchError} from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';



@Injectable()
export class ExtendedHttpService {
    constructor(protected http: HttpClient) { }

    getJson<TResponse>(url: string, headers?: any): Observable<TResponse> {
        return this.http
            .get<TResponse>(url, { headers: this.getHeaders(headers)}).pipe(
                catchError((error: any) => throwError(error))
            );
    }

    postJson<TResponse>(url: string, data: any, headers?: any): Observable<TResponse> {
        return this.http
            .post<TResponse>(url, JSON.stringify(data), { headers: this.getHeaders(headers) }).pipe(
                catchError((error: any) => throwError(error))
            );
    }

    postJsonNoContent(url: string, data: any, headers?: any): Observable<Object> {
        // fixed issue https://github.com/angular/angular/issues/18680 - remove after fix
        return this.http
            .post(url, JSON.stringify(data), { headers: this.getHeaders(headers), responseType: 'text' }).pipe(
                catchError((error: any) => throwError(error))
            );
    }

    deleteJson<TResponse>(url: string, headers?: any): Observable<TResponse> {
      return this.http
          .delete<TResponse>(url, { headers: this.getHeaders(headers) }).pipe(
              catchError((error: any) => throwError(error))
          );
    }

    private getHeaders(headers?: any): HttpHeaders {
        let options = new HttpHeaders();
        if (headers) {
            Object.keys(headers).forEach((v: string) => {
                options.set(v, headers[v]);
            });
        }
        options = options.set('Accept', 'application/json');
        options = options.set('Content-Type', 'application/json; charset=utf-8');
        return options;
    }
}
