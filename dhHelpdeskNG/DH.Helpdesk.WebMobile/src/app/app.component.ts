import {TranslateService as NgxTranslateService } from '@ngx-translate/core';
import { Component, OnInit } from '@angular/core';
import { TranslationApiService } from './shared/services/api/translationApiService';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'app';
  testString:string  ="";
  testString2:string  ="";

  get CurrentLanguage(){
    return this.ngxTranslateService.currentLang;
  }

  get Languages() {
    return this.ngxTranslateService.langs;
  }

  constructor(private ngxTranslateService: NgxTranslateService, private translationApiService: TranslationApiService) {        
    console.log('>> AppComponent ctor.');
  }

  ngOnInit(){
    console.log('>> AppComponent started.');
  }

  test(){    
     this.testString = this.ngxTranslateService.instant("Alla ärenden");
     this.ngxTranslateService.get("Nytt ärende").subscribe((res:string) => {
        this.testString2 = res; 
        console.log("Translation2: " + res);
      });
  }

  switchLanguage(lang: string){    
    this.ngxTranslateService.use(lang.toLowerCase());    
  }  

  getDisplayName(langCode:string){
    return this.translationApiService.Languages[langCode.toLowerCase()];
  }
  
  getCurrentUser(){
    return {
      name: 'Peter'
    }
  }
}
