import {TranslateService} from '@ngx-translate/core';
import { Component } from '@angular/core';
import { mobiscroll } from '@mobiscroll/angular';
import { config } from '../environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  pageSettings = {
  };
  version = config.version;

  constructor() { 
    mobiscroll.settings = { theme: 'ios', lang: 'en', labelStyle: 'stacked' };   
  }
}
