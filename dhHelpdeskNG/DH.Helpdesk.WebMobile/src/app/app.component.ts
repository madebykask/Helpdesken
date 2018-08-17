import {TranslateService} from '@ngx-translate/core';
import { Component } from '@angular/core';
import {Languages} from './shared/translation/translateLoader';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
  testString:string  ="";
  testString2:string  ="";

  constructor(private translate: TranslateService) {    
  }

  test(){    
     this.testString = this.translate.instant("Test");
     this.translate.get("Test").subscribe((res:string) => {
        this.testString2 = res; 
        console.log("Translation2: " + res);
      });
  }

  switchLanguage(lang: string){
    this.translate.use(lang);    
  }  

  getDisplayName(langCode:string){
    return Languages[langCode];
  }
  
  getCurrentUser(){
    return {
      name: 'Peter'
    }
  }
}
