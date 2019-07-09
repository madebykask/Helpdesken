import { Injectable } from '@angular/core';
import { HttpRequest } from '@angular/common/http';
import { finalize, switchMap, take } from 'rxjs/operators';
import { LocalStorageService } from '../../services/local-storage';
import { AuthenticationApiService } from '../api/authentication/authentication-api.service';
import { UserSettingsApiService } from 'src/app/services/api/user/user-settings-api.service';
import { InfoLoggerService } from '../logging/info-logger.service';
import { CommunicationService, Channels } from '../communication';
import { AuthenticationStateService } from './authentication-state.service';
import { throwError, Observable } from 'rxjs';
import { CurrentUser } from 'src/app/models';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {

    //ctor()
    constructor(protected localStorageService: LocalStorageService,
      private logger: InfoLoggerService,
      private authStateService: AuthenticationStateService,
      private authApiService: AuthenticationApiService,
      private userSettingsApiService: UserSettingsApiService,
      private commService: CommunicationService) {
     }

    login(username: string, password: string): Observable<CurrentUser> {
        return this.authApiService.login(username, password)
          .pipe(
              take(1),
              switchMap(isSuccess => {
                  if (!isSuccess) { throwError('Something wrong.'); }
                  return this.userSettingsApiService.loadUserSettings();
              }),
              // tap(() => this._logger.log(`Log in action.`)),
              finalize(() => this.raiseAuthenticationChanged())
          );
    }

    refreshToken() {
        const user = this.authStateService.getUser();
        return this.authApiService.refreshToken(user)
          .pipe(
            // tap(() => this._logger.log(`Refresh token action.`)),
            finalize(() => this.raiseAuthenticationChanged())
          );
    }

    logout() {
        // remove user from local storage to log user out
        this.localStorageService.removeCurrentUser();
        // this._logger.log(`Log out action.`);
        this.raiseAuthenticationChanged();
    }

    setAuthHeader(request: HttpRequest<any>): HttpRequest<any> {
        // Get access token
        const accessToken = this.authStateService.getAccessToken();

        // If access token is null this means that user is not logged in
        // And we return the original request
        if (!accessToken) {
            return request;
        }

        // We clone the request, because the original request is immutable
        return request.clone({
            setHeaders: {
                Authorization: this.getAuthorizationHeaderValue()
            }
        });
    }

    getAuthorizationHeaderValue(): string {
      const accessToken = this.authStateService.getAccessToken();
      return `Bearer ${accessToken}`;
    }

    private raiseAuthenticationChanged() {
        this.commService.publish(Channels.AuthenticationChange, {});
    }
}
