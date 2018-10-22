import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { AuthenticationService } from '../../services/authentication';
import { ErrorHandlingService } from 'src/app/services/errorhandling/error-handling.service';

import { Observable, throwError, BehaviorSubject, of } from 'rxjs';
import { catchError, switchMap, filter, take, map } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    private refreshTokenInProgress = false;
    
    // Refresh Token Subject tracks the current token, or is null if no token is currently
    // available (e.g. refresh pending).
    private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);
    
    constructor(private authenticationService: AuthenticationService, private errorHandlingService: ErrorHandlingService) {}

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(
            catchError(err => {
                const errorMsg = err.error.message || err.error.error_description || err.statusText;//TODO: unify errors messages

                if (request.url.includes("account/refresh")) {
                    this.authenticationService.logout();
                    location.reload(true);
                }
                if (request.url.includes("account/login")) {
                    return throwError(errorMsg);
                }

                if (err.status !== 401) {
                    //todo:
                    //this.errorHandlingService.handleError(err, 'Unknown error occured.');
                    return throwError(errorMsg);
                }

                if (this.refreshTokenInProgress) {
                    // If refreshTokenInProgress is true, we will wait until refreshTokenSubject has a non-null value
                    // – which means the new token is ready and we can retry the request again
                    return this.refreshTokenSubject
                        .pipe( 
                            filter(result => result !== null),
                            take(1),
                            switchMap(() => { return next.handle(this.authenticationService.setAuthHeader(request)) })
                        )
                        //.subscribe(()=> { return next.handle(this.authenticationService.setAuthHeader(request)) });
                } else {
                    this.refreshTokenInProgress = true;

                    // Set the refreshTokenSubject to null so that subsequent API calls will wait until the new token has been retrieved
                    this.refreshTokenSubject.next(null);

                    // Call authenticationService.refreshToken(this is an Observable that will be returned)
                    return this.authenticationService.refreshToken().pipe(
                            switchMap((r: boolean) => {
                                //When the call to refreshToken completes we reset the refreshTokenInProgress to false
                                // for the next time the token needs to be refreshed
                                this.refreshTokenInProgress = false;
                                this.refreshTokenSubject.next(true);

                                return next.handle(this.authenticationService.setAuthHeader(request));
                            }),
                            catchError((err: any) =>  {
                                this.refreshTokenInProgress = false;

                                this.authenticationService.logout();
                                return throwError(errorMsg);
                            }
                        ) //finalize(() => this.stopLoading())
                    );
                }
            }
        ))
    }
}