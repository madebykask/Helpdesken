import { Component, OnInit } from '@angular/core';
import { mobiscroll } from '@mobiscroll/angular';
import { config } from '@env/environment';
import { AuthenticationService } from './services/authentication';
import { LoggerService } from './services/logging';
import { Router } from '@angular/router';
import '../../node_modules/moment-timezone/moment-timezone-utils';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  pageSettings = {};

  bottomMenuSettings = {
    type: 'bottom',
  };
  
  version = config.version;

  constructor(private _authenticationService: AuthenticationService, 
    private _logger: LoggerService,    
    private _router: Router) {
    mobiscroll.settings = { theme: 'ios', lang: 'en', labelStyle: 'stacked' };
  }

  ngOnInit(): void {    
    const version = this._authenticationService.getVersion();
    const isAuthenticated = this._authenticationService.isAuthenticated();    
    
    if (isAuthenticated && config.version !== version) {
      this._logger.log('>>>>>>>>>>>>>>>>Logout: version changed');
      this._router.navigate(['/login']);
    }
  }
}
