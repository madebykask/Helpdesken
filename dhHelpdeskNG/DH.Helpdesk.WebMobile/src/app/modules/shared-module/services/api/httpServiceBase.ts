import { Observable } from 'rxjs/Observable';
import { catchError, map } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { config } from '@env/environment';
import { LocalStorageService } from '../../../../services/local-storage';
import { AuthConstants } from '../../constants';
import { QueryParamsOptions } from './query-params-options';

export abstract class HttpApiServiceBase {
  protected baseApiUrl: string;

  protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService) {
    this.baseApiUrl = config.apiUrl;
  }

  protected getJsonWithParams<TResponse>(resourcePath: string, paramsObj: {}, headers: any = null, noAuth = false): Observable<TResponse> {
    // prepare request options
    const requestOptions = {
      headers: this.getHeaders(headers, false, noAuth),
      params: new HttpParams({ fromObject: paramsObj }) // uses HttpUrlEncodingCodec to encode key/values.
    };

    const requestUrl = this.getRequestUrl(resourcePath);
    return this.http
        .get<TResponse>(requestUrl, requestOptions)
        .pipe(
            catchError((error: any) => {
              return throwError(error);
        }));
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

  
  protected postJsonWithSubscription<TResponse>(url: string, data: any, headers: any = null, noAuth = false): Observable<TResponse> {
    headers = this.getHeaders(headers, true, noAuth);
 
    console.log(headers)
    console.log(JSON.stringify(data))
    // debugger;
    let req =  this.http
        .post<TResponse>(url, JSON.stringify(data), { headers: headers })
        .pipe(
            catchError((error: any) => {
                return throwError(error);
        }));

        req.subscribe();
        return req;
  }

  protected sendOptions(url: string, headers: any, noAuth: boolean): Observable<any> {
    return this.http
      .options(url, {
        observe: 'response',
        headers: this.getHeaders(headers, true, noAuth)
      }).pipe(
        map(res => {
            return {
                body : res.body,
                headers: res.headers
            };
      }),
      catchError((error: any) => {
            return throwError(error);
      }));
  }

  protected deleteWithResult<TResponse>(url: string, headers: any = null, noAuth = false, withCredentials: boolean = false): Observable<TResponse> {

    return this.http.delete<TResponse>(url, {
                headers: this.getHeaders(headers, true, noAuth),
                withCredentials: withCredentials
             }).pipe(
                catchError((error: any) => {
                  return throwError(error);
            })
        );
  }

  // TODO: review files delete
  protected deleteResource(url: string, headers: any = null, noAuth = false): Observable<any> {
    // fixed issue https://github.com/angular/angular/issues/18680 - remove after fix
    return this.http
        .delete(url, { headers: this.getHeaders(headers, true, noAuth), responseType: 'text' })
        .pipe(
            catchError((error: any) => {
              return throwError(error);
            })
        );
  }

  protected postJsonNoContent(url: string, data: any, headers: any = null, noAuth = false): Observable<Object> {
    // fixed issue https://github.com/angular/angular/issues/18680 - remove after fix
    return this.http
        .post(url, JSON.stringify(data), { headers: this.getHeaders(headers, true, noAuth), responseType: 'text' })
        .pipe(
            catchError((error: any) => {
              return throwError(error);
            })
        );
  }

  protected getFileBody(url: string, headers?: any): Observable<Blob> {
    return this.http.get(url, {
        responseType: 'blob',
        observe: 'body',
        headers: this.getHeaders(headers, false)
    }).pipe(
        catchError((error: any) => throwError(error))
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

  protected createQueryParams(customParams: {}, opt: QueryParamsOptions): any {
    opt = opt || new QueryParamsOptions();

    let params = customParams ? Object.assign({}, customParams) : {};

    const userData = this.localStorageService.getCurrentUser();

    if (opt.addCustomerId === true) {
        if (userData) {
            params = Object.assign({}, params, {cid: userData.currentData.selectedCustomerId});
        }
    }

    if (opt.addLanguage === true) {
        if (userData) {
            params = Object.assign({}, params, {langid: userData.currentData.selectedLanguageId});
        }
    }
    return params;
  }

  //* NOTE: USE createQueryParams and getJsonWithParams instead!
  buildResourseUrl(resourceName: string, params: {} = null, addCustomerId: boolean = true, addLanguage: boolean = false) {
      let urlParams: string = null;
      const paramsOptions = new QueryParamsOptions();
      paramsOptions.addCustomerId = addCustomerId;
      paramsOptions.addLanguage = addLanguage;

      params = this.createQueryParams(params, paramsOptions);

      // build resource url with params
      if (params) {
        const str = Object.keys(params).map(key => `${key}=${encodeURIComponent(params[key])}`).join('&');
        urlParams = (str.length && resourceName.indexOf('?') < 0) ? '?' + str : str;
      }

      if (!resourceName.startsWith('/')) {
        resourceName = '/' + resourceName;
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

  private getRequestUrl(resourcePath: string) {
    if (!resourcePath.startsWith('/')) {
      resourcePath = '/' + resourcePath;
    }
    return `${this.baseApiUrl}${resourcePath}`;
  }

  protected addQsParam(qsParamMap: any, paramName: string, customerId: number): any {
      const newQsParams = {...qsParamMap, [paramName]: customerId };
      return newQsParams;
  }
  /*
  protected handleError(error: Response) {
    console.error(error);
    return Observable.throw(error.json().error || 'Server Error');
  }
  */
}


