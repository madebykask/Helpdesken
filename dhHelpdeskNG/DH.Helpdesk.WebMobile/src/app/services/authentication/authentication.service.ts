import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { config } from '../../../environments/environment';
import { CurrentUser, UserAuthenticationData } from '../../models'
import { LocalStorageService } from '../../services/localStorage'
import * as moment from 'moment'

@Injectable({ providedIn: 'root' })
export class AuthenticationService {

    constructor(private http: HttpClient, private localStorageService: LocalStorageService) { }    

    login(username: string, password: string) {
        let clientId = config.clientId;
        return this.http.post<any>(`${config.apiUrl}/api/account/login`, { username, password, clientId})
            .pipe(map(data => {                
                // login successful if there's a token in the response
                if (data && data.access_token) {
                    let user = new CurrentUser();
                    UserAuthenticationData.setData(user.authData, data);
                    user.authData.recievedAt = new Date();
                    // store user details and token in local storage to keep user logged in between page refreshes
                    this.localStorageService.setCurrentUser(user);                    
                }

                return data;
            }));
    }

    refreshToken() {
        var user = this.getUser();
        if(user.authData.refresh_token) {
            var refreshToken = user.authData.refresh_token;
            let clientId = config.clientId;
            return this.http.post<any>(`${config.apiUrl}/api/account/refresh`, { refreshToken, clientId })
                .pipe(map(data => {
                    user.authData.access_token = data.refresh_token;
                    user.authData.expires_in = Number(data.expires_in);
                    user.authData.recievedAt = new Date();
                }));
        }
        
    }

    logout() {
        // remove user from local storage to log user out
        this.localStorageService.removeCurrentUser();
    }

    getAccessToken(): string {
        var user = this.getUser();
        return user ? user.authData.access_token : null;
    }

    setAuthHeader(request: HttpRequest<any>): HttpRequest<any> {
        // Get access token 
        const accessToken = this.getAccessToken();

        // If access token is null this means that user is not logged in
        // And we return the original request
        if (!accessToken) {
            return request;
        }

        // We clone the request, because the original request is immutable
        return request.clone({
            setHeaders: {
                Authorization: this.getAccessToken()
            }
        });
    }

    isTokenExpired(): boolean {
        let user = this.getUser();
        if(!user.authData.recievedAt || !user.authData.expires_in) return true;//TODO: throw error

        let expiresAt = user.authData.recievedAt.getTime() + user.authData.expires_in * 1000;
        let now = new Date();
        return now.getTime() > expiresAt;
    }


    public getUser(): CurrentUser {
        return this.localStorageService.getCurrentUser();
    }
}