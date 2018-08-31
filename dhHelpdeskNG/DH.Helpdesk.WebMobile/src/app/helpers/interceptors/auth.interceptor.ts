import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../../services/authentication'

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor(private authService: AuthenticationService, ) {}
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // add authorization header with token if available
        if(request.url.includes('account/refresh') ||
            request.url.includes('account/login'))
            return next.handle(request);

        let newRequest = this.authService.setAuthHeader(request);

        return next.handle(newRequest ? newRequest : request);
    }
}