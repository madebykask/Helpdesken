import { Router } from "@angular/router";
import { Injectable } from "@angular/core";

@Injectable({providedIn: 'root'})
export class EnvironmentService {
  constructor(private _router: Router) {
  }

  getEnvInfo(): EnvInfo {
    let env = new EnvInfo();
    env.standalone = this.isInStandaloneMode();
    env.userAgent = this.getUserAgent();
    env.appVersion = this.getAppVersion();    
    return env;
  }

  getUserAgent(): string {
    return navigator.userAgent;
  }

  getAppVersion(): string {
    return navigator.appVersion;
  }

  getUrl(): string {
    return this._router.url;
  }

  // Detects if device is on iOS 
  isIos(): boolean {
    const userAgent = navigator.userAgent.toLowerCase();
    return /iphone|ipad|ipod/.test( userAgent );
  }

  // Detects if device is in standalone mode
  isInStandaloneMode()  {
    return (('standalone' in navigator) && (<any>navigator).standalone) || // safari
      window.matchMedia('(display-mode: standalone)').matches; // others
  };
}

export class EnvInfo {
  userAgent: string;
  appVersion: string;
  standalone: boolean;
}