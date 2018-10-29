import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Subject, interval } from 'rxjs';
import { MbscSelect, MbscSelectOptions, MbscNavOptions } from '@mobiscroll/angular';
import { take, takeUntil, filter, map, finalize } from 'rxjs/operators';
import { UserSettingsService } from 'src/app/services/user';
import { OptionItem } from 'src/app/models';
import { LanguagesService } from 'src/app/services/language/languages.service';
import { TranslateService } from '@ngx-translate/core';
import { AuthenticationService } from 'src/app/services/authentication';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit, OnDestroy {
  private _destroy$ = new Subject();
  
  @ViewChild('languages') 
  languagesCtrl: MbscSelect;
  
  languagesSettings: MbscSelectOptions = {
    cssClass: 'languages-list',
    showOnTap: false,
    display: 'bottom',
    data: [],
    onSet: (event, inst) => this.onLanguageChange(event, inst)
  }

  bottomMenuSettings: MbscNavOptions = {
    //layout: 'fixed',
    type: 'bottom',
    moreText: null,
    moreIcon: 'fa-ellipsis-h',
    menuIcon: null,
    menuText: null,    
  }

  languageId: number = 0;
  isLoadingLanguage: boolean = true;
  
  constructor(private _router: Router, 
              private _userSettingsService : UserSettingsService, 
              private _authenticationService: AuthenticationService,
              private _languagesService: LanguagesService, 
              private _ngxTranslateService: TranslateService) {           
  }

  ngOnInit() {

      this.applyTranslations();
      this.loadLanguages();
      
      //subsribe on changes
      this._userSettingsService.userSettingsLoaded$.pipe(        
        takeUntil(this._destroy$)
      ).subscribe(_ => {
        this.loadLanguages();
      });
  }

  ngOnDestroy(): void {
    this._destroy$.next();
  }

  private onLanguageChange(event, inst) {
    const item = (<OptionItem[]>inst.settings.data).find(item => item.text == event.valueText);
    let selectedLanguage = item.value;
    //console.log('>>> change language to: ' + selectedLanguage);
    this.setLanguage(item ? +item.value : null);
  }

  openLanguages() {
    if (!this.isLoadingLanguage) {
      this.languagesCtrl.instance.show();
    }
  }

  private loadLanguages() {
    this.languageId = this._userSettingsService.getCurrentLanguage() || 0;
    if (this.languageId === 0)
       return;

    console.log('>>> footer: loading languages')
    this.isLoadingLanguage = true;
    this._languagesService.getLanguages()
      .pipe(
        take(1),
        finalize(() => this.isLoadingLanguage = false)        
      )
      .subscribe((data) => {
          console.log('>>> footer: languages loaded')
          this.applyTranslations();
          this.languagesCtrl.instance.refresh(data);

      });
  }
  
  private applyTranslations() {    
      this.languagesCtrl.setText = this._ngxTranslateService.instant("Välj");
      this.languagesCtrl.cancelText  = this._ngxTranslateService.instant("Avbryt");    
  }

  logout() {    
    this._authenticationService.logout();
    this.goTo('/login');
  }

  goTo(url: string = null) {
    if (url == null) return;
    this._router.navigate([url]);
  }

  setLanguage(languageId: number) {
    if (languageId) {
      this._userSettingsService.setCurrentLanguage(languageId);

      // reload will reopen the app
      window.location.reload(true);    
    }
  }
}


