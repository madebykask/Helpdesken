import {TranslateService} from '@ngx-translate/core';
import { Component } from '@angular/core';
import { mobiscroll } from '@mobiscroll/angular';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  pageSettings = {
  };

  constructor() { 
    mobiscroll.settings = { theme: 'ios', lang: 'en' };   
  }
}
