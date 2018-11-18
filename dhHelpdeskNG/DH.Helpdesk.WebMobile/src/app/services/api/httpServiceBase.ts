import { Observable } from 'rxjs/Observable';
import { catchError, map } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { config } from '@env/environment';
import { LocalStorageService } from '../local-storage';
import { AuthConstants } from 'src/app/helpers/constants';

export abstract class HttpApiServiceBase {
  private baseApiUrl: string;

  protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService) {
    this.baseApiUrl = config.apiUrl;
  }

  protected getJson<TResponse>(url: string, headers: any = null, noAuth = false): Observable<TResponse> {
    return this.http
        .get<TResponse>(url, { headers: this.getHeaders(headers, false, noAuth)})
        .pipe(
            catchError((error: any) => {
                return throwError(error);
        }));
  }

  protected postJson<TResponse>(url: string, data: any, headers: any = null, noAuth = false): Observable<TResponse> {
    return this.http
        .post<TResponse>(url, JSON.stringify(data), { headers: this.getHeaders(headers, true, noAuth) })
        .pipe(
            catchError((error: any) => {
                return throwError(error);
        }));
  }

  protected postJsonNoContent(url: string, data: any, headers: any = null, noAuth = false): Observable<Object> {
    // fixed issue https://github.com/angular/angular/issues/18680 - remove after fix
    return this.http
        .post(url, JSON.stringify(data), { headers: this.getHeaders(headers, true, noAuth), responseType: 'text' })
        .pipe(
            catchError((error: any) => {
                return throwError(error);
        }));
  }

  protected getFileBody(url: string, headers?: any): Observable<Blob> {
    return this.http.get(url, {
        responseType: 'blob',
        observe: 'body',
        headers: this.getHeaders(headers, false)
    }).pipe(
        catchError((error:any) => throwError(error))
        );
  }

  protected getFileResponse(url: string, headers?: any): Observable<any> {
    return this.http.get(url, {
        responseType: 'blob',
        observe: 'response',
        headers: this.getHeaders(headers, false)
    }).pipe(
        map(res => {
                return {
                    body : res.body,
                    fileName: res.headers.get('content-filename'),
                    contentType: res.headers.get('content-type')
                };
        }),
        catchError((error: any) => {
            return throwError(error);
        }));
  }

    protected buildResourseUrl(resourceName: string, params: object = null, addCustomerId = true, addLanguage = false, addSessionId = false) {
      let urlParams: string = null;
      const userData = this.localStorageService.getCurrentUser();
        
      if (addCustomerId === true) {
          if (userData !== null) {
              params = Object.assign({}, params || {}, {cid: userData.currentData.selectedCustomerId});
          }
      }

      if (addLanguage === true) {
          if (userData !== null) {
              params = Object.assign({}, params || {}, {langid: userData.currentData.selectedLanguageId});
          }
      }

        if (addSessionId === true) {
            if (userData !== null) {
                params = Object.assign({}, params || {}, { sessionId: userData.authData.sessionId })
            }
        }

      if (params) {
          const str = Object.keys(params).map(function(key) {
              return key + '=' + encodeURIComponent(params[key]);
            }).join('&');

            urlParams = (str.length > 0 && resourceName.indexOf('?') < 0) ? '?' + str : '&' + str;
      }
      return `${this.baseApiUrl}${resourceName}${urlParams || ''}`;
  }

  private getHeaders(headers?: any, addContentType = true, noAuth = false): HttpHeaders {
      let options = new HttpHeaders();
      if (headers != null) {
          Object.getOwnPropertyNames(headers).forEach((v: string) => {
              options.set(v, headers[v]);
          });
      }

      options = options.set('Accept', 'application/json');
      if (addContentType) {
        options = options.set('Content-Type', 'application/json; charset=utf-8');
      }
      if (noAuth) {
        options = options.set(AuthConstants.NoAuthHeader, '');
      }
      // options = options.set('Access-Control-Allow-Origin', '*');

      return options;
  }

  protected addQsParam(qsParamMap: any, paramName: string, customerId: number): any {
      const newQsParams = {...qsParamMap, [paramName]: customerId };
      return newQsParams;
  }
}

