
import { throwError,  Observable } from 'rxjs';

import {catchError} from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpHandler, HttpRequest, HttpEvent, HttpInterceptor } from '@angular/common/http';



//@Injectable()
//export class HttpBase extends HttpClient {


//    constructor(handler: HttpHandler) {
//        super(handler);
//    }

//    //request(url: string | Request, options?: RequestOptionsArgs): Observable<Response> {
//    //    return super.request(url, options).catch((error: Response) => {
//    //        switch (error.status) {
//    //        case 404:
//    //            console.log('404 error.');
//    //        default:
//    //            return Observable.throw(error);
//    //        }
//    //    });
//    //}

//    request(first: string | HttpRequest<any>, url?: string, options: any = {}): Observable<any> {
//        return super.request(first, url, options)
//            .catch((error: HttpErrorResponse) => {
//                switch (error.status) {
//                    case 404:
//                        console.log('404 error.');
//                    default:
//                        return Observable.throw(error);
//                }
//            });
//    }
    
    
//}

@Injectable()
export class HttpInterceptorService implements HttpInterceptor {
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(catchError(err => {
            if (err instanceof HttpErrorResponse) {
                switch (err.status) {
                    case 404:
                        console.log('404 error.');
                }
            }
            return throwError(err);
        }));
    }
}
