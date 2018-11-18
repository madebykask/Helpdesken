import { Injectable } from '@angular/core';
import { HttpRequest } from '@angular/common/http';
import { finalize, tap } from 'rxjs/operators';
import { LocalStorageService } from '../../services/local-storage'
import { AuthenticationApiService } from '../api';
import { InfoLoggerService } from '../logging/info-logger.service';
import { CommunicationService, Channels } from '../communication';
import { AuthenticationStateService } from './authentication-state.service';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {

    constructor(protected localStorageService: LocalStorageService,
      private _logger: InfoLoggerService,
      private _authStateService: AuthenticationStateService,
      private _authApiService: AuthenticationApiService,
      private _commService: CommunicationService) {
     }

    login(username: string, password: string) {
        return this._authApiService.login(username, password)
          .pipe(
            tap(() => this._logger.log(`Log in action.`)),
            finalize(() => this.raiseAuthenticationChanged())
          );
    }

    refreshToken() {
        var user = this._authStateService.getUser();
        return this._authApiService.refreshToken(user)
          .pipe(
            tap(() => this._logger.log(`Refresh token action.`)),
            finalize(() => this.raiseAuthenticationChanged())
          );
    }

    logout() {
        // remove user from local storage to log user out
        this.localStorageService.removeCurrentUser();
        this._logger.log(`Log out action.`);
        this.raiseAuthenticationChanged();
    }

    setAuthHeader(request: HttpRequest<any>): HttpRequest<any> {
        // Get access token 
        const accessToken = this._authStateService.getAccessToken();

        // If access token is null this means that user is not logged in
        // And we return the original request
        if (!accessToken) {
            return request;
        }

        // We clone the request, because the original request is immutable
        return request.clone({
            setHeaders: {
                Authorization: `Bearer ${accessToken}`
            }
        });
    }   

    private raiseAuthenticationChanged() {
        this._commService.publish(Channels.AuthenticationChange, {});
    }
}
