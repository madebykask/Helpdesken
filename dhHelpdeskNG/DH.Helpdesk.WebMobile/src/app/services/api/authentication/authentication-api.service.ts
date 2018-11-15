import { HttpApiServiceBase } from '../httpServiceBase';
import { Injectable } from '@angular/core';
import { config } from '@env/environment';
import { take, map, catchError } from 'rxjs/operators';
import { CurrentUser } from 'src/app/models';
import { HttpClient } from '@angular/common/http';
import { LocalStorageService } from '../../local-storage';
import { of } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthenticationApiService extends HttpApiServiceBase {

  constructor(protected http: HttpClient, protected localStorageService: LocalStorageService) {
    super(http, localStorageService);
  }

  login(username: string, password: string) {
    const clientId = config.clientId;
    return this.postJson<any>(this.buildResourseUrl('/api/account/login', undefined, false), { username, password, clientId })
        .pipe(
            take(1),
            map(data => {
            let isSuccess = false;
            // login successful if there's a token in the response
            if (data && data.access_token) {
                const user = CurrentUser.createAuthenticated(data);
                user.version = config.version;

                // store user details and token in local storage to keep user logged in between page refreshes
                this.localStorageService.setCurrentUser(user);
                isSuccess = true;
            }

            return isSuccess;
        }));
  }

  refreshToken(user: CurrentUser) {
    if (user.authData && user.authData.refresh_token) {
        const refreshToken = user.authData.refresh_token;
        const clientId = config.clientId;
        return this.postJson<any>(this.buildResourseUrl('/api/account/refresh', undefined, false), { refreshToken, clientId })
            .pipe(
                map(data => {
                    user.authData.access_token = data.access_token;
                    user.authData.expires_in = Number(data.expires_in);
                    user.authData.recievedAt = new Date();
                    this.localStorageService.setCurrentUser(user);

                    return true;
                }),
                catchError(err => {
                    return of(false);
                }),
            );
    }
  }
}
