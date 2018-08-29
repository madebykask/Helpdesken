import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { config } from '../../../environments/environment';
import { User } from '../../models'

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    private currentUserStorageName: string = 'currentUser';

    constructor(private http: HttpClient) { }    

    login(username: string, password: string) {
        let clientId = config.clientId;
        return this.http.post<any>(`${config.apiUrl}/api/account/login`, { username, password, clientId})
            .pipe(map(data => {                
                // login successful if there's a token in the response
                if (data && data.access_token) {
                    // store user details and token in local storage to keep user logged in between page refreshes
                    localStorage.setItem(this.currentUserStorageName, JSON.stringify(data));
                }

                return data;
            }));
    }

    refreshToken() {
        var user = this.getUser();
        if(user.refresh_token) {
            var refreshToken = user.refresh_token;
            let clientId = config.clientId;
            return this.http.post<any>(`${config.apiUrl}/api/account/refresh`, { refreshToken, clientId })
                .pipe(map(data => {
                    user.access_token = data.refresh_token;
                    user.expires_in = Number(data.expires_in);
                }));
        }
        
    }

    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem(this.currentUserStorageName);
    }

    getAccessToken(): string {
        var user = this.getUser();
        return user ? user.access_token : null;
    }

    private getUser(): User {
        let currentUser = localStorage.getItem(this.currentUserStorageName);
        if(currentUser) {
            return User.fromJSON(currentUser);
        }

        return null;
    }
}