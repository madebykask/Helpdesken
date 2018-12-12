import { Injectable } from "@angular/core";
import { CurrentUser } from "src/app/models";
import { LocalStorageService } from "../local-storage";

@Injectable({ providedIn: 'root' })
export class AuthenticationStateService {

  constructor(private _localStorageService: LocalStorageService) {}

  isAuthenticated() : boolean {
    if (this.isTokenExpired()) return false;
   
       let token = this.getAccessToken();
       return token && token.length > 0;
   }

  isTokenExpired(): boolean {
      let user = this.getUser();
      
      if (user == null || user.authData == null)
          return true;
          
      if (!user.authData.recievedAt || !user.authData.expires_in)
          return true; // TODO: throw error

      let expiresAt = user.authData.recievedAt.getTime() + user.authData.expires_in * 1000;
      let now = new Date();
      return now.getTime() > expiresAt;
  }

  hasToken(): boolean {
      return this.getAccessToken() != null;
  }

  getVersion(): string {
      let user = this.getUser();
      if (user == null) return null;

      return user.version;
  }
  
  getUser(): CurrentUser {
      return this._localStorageService.getCurrentUser();
  }

  getAccessToken(): string {
    let user = this.getUser();
    return user && user.authData && user.authData.access_token ? user.authData.access_token : null;
  }
}
