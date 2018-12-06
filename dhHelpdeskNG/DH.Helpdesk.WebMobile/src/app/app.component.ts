import { Component, OnInit, OnDestroy } from '@angular/core';
import { mobiscroll } from '@mobiscroll/angular';
import { config } from '@env/environment';
import { AuthenticationStateService } from './services/authentication';
import { LoggerService } from './services/logging';
import { Router, NavigationStart } from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { filter, takeUntil } from 'rxjs/operators';
import { InfoLoggerService } from './services/logging/info-logger.service';
import '../../node_modules/moment-timezone/moment-timezone-utils';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  private _destroy$ = new Subject();
  pageSettings = {};
  
  bottomMenuSettings = {
    type: 'bottom',
  };
  
  version = config.version;
  navStart: Observable<NavigationStart>;

  constructor(private _authenticationService: AuthenticationStateService, 
    private _logger: LoggerService,
    private _infoLogger: InfoLoggerService,
    private _router: Router) {
    mobiscroll.settings = { theme: 'ios', lang: 'en', labelStyle: 'stacked' };
    this.navStart = _router.events.pipe(
      filter(evt => evt instanceof NavigationStart),
      takeUntil(this._destroy$)
    ) as Observable<NavigationStart>;
  }

  ngOnInit(): void { 
    // this.navStart.subscribe(evt => this._infoLogger.log(`Navigation started: ${evt.url}`));

    const version = this._authenticationService.getVersion();
    const isAuthenticated = this._authenticationService.isAuthenticated();
    
    if (isAuthenticated && config.version !== version) {
      this._logger.log('>>>>>>>>>>>>>>>>Logout: version changed');
      this._router.navigate(['/login']);
    }   
    
    // Checks if should display install popup notification:
    /*
    if (this.isIos() && this.isInStandaloneMode()) {
      // logout or refresh token on open.
    } 
    */
  }

  ngOnDestroy(): void {
      this._destroy$.next();
  }
}