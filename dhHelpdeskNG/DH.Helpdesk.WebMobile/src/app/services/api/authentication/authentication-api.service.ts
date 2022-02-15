import { HttpApiServiceBase } from '../../../modules/shared-module/services/api/httpServiceBase';
import { Injectable } from '@angular/core';
import { config } from '@env/environment';
import { take, map, switchMap } from 'rxjs/operators';
import { CurrentUser } from 'src/app/models';
import { HttpClient } from '@angular/common/http';
import { LocalStorageService } from '../../local-storage';
import { Observable, of } from 'rxjs';
import { ErrorHandlingService } from '../../logging/error-handling.service';
import { MsalService } from '@azure/msal-angular';
import { AuthenticationResult } from '@azure/msal-browser';


@Injectable({ providedIn: 'root' })
export class AuthenticationApiService extends HttpApiServiceBase {

  constructor(protected http: HttpClient,
    protected localStorageService: LocalStorageService,
    protected errorHandlingService: ErrorHandlingService,
    private msalService: MsalService) {
    super(http, localStorageService);
  }

  login(username: string, password: string): Observable<boolean> {
    const clientId = config.clientId;
    const postData = { username, password, clientId };
    return this.postJson<any>(this.buildResourseUrl('/api/account/login', undefined, false), postData)
      .pipe(
        take(1),
        map(data => {
          let isSuccess = false;
          // login successful if there's a token in the response
          if (data && data.access_token) {
            const user = CurrentUser.createAuthenticated(data);
            user.currentData.name = username;
            user.version = config.version;

            // store user details and token in local storage to keep user logged in between page refreshes
            this.localStorageService.setCurrentUser(user);
            isSuccess = true;
          }
          return isSuccess;
        }));
  }

  microsoftLogin(): Observable<boolean> {
    return this.msalService.loginPopup().pipe(
      take(1),
      switchMap(response => {
        //login successful if there's a token in the response
        
        if (response && response.idToken) {
          const postData = { Email: response.account.username, ClientId: config.clientId, IdToken: response.idToken };

          return this.postJson<any>(this.buildResourseUrl('/api/account/SignInWithMicrosoft', undefined, false), postData)
            .pipe(
              take(1),
              map(data => {
                let isSuccess = false;
                // login successful if there's a token in the response
                if (data && data.access_token) {
                  const user = CurrentUser.createAuthenticated(data);
                  user.currentData.name = response.account.username;
                  user.version = config.version;

                  // store user details and token in local storage to keep user logged in between page refreshes
                  this.localStorageService.setCurrentUser(user);
                  isSuccess = true;
                }
                return isSuccess;
              }))
        }
        return of(false);
      }))
  }

  microsoftLogout() {
    if(this.msalService.instance.getAllAccounts().length) {
      this.msalService.logout();
      return true;
    };
    return false;
  }

  refreshToken(refreshToken: string): Observable<any> {
    const params = {
      refreshToken: refreshToken,
      clientId: config.clientId
    };
    return this.postJson<any>(this.buildResourseUrl('/api/account/refresh', null, false), params);
  }
}
