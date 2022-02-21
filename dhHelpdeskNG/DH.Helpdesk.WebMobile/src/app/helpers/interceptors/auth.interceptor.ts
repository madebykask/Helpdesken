import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../../services/authentication';
import { AuthConstants } from 'src/app/modules/shared-module/constants';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private authService: AuthenticationService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      // add authorization header with token if available

      let newRequest: HttpRequest<any> = null;
      if (request.headers.has(AuthConstants.NoAuthHeader)) {
        newRequest = request.clone({ headers: request.headers.delete(AuthConstants.NoAuthHeader)});
      } else {
        newRequest = this.authService.setAuthHeader(request);
      }
      return next.handle(newRequest ? newRequest : request);
  }
}
