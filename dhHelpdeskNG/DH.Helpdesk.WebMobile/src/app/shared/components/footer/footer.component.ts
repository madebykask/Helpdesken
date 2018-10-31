import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { MbscSelect, MbscSelectOptions, MbscNavOptions } from '@mobiscroll/angular';
import { take, takeUntil, finalize } from 'rxjs/operators';
import { UserSettingsService } from 'src/app/services/user';
import { LanguagesService } from 'src/app/services/language/languages.service';
import { TranslateService, LangChangeEvent } from '@ngx-translate/core';
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
      
      //handles user login - footer component will be created before user settings are loaded.
      this._userSettingsService.userSettingsLoaded$.pipe(
          takeUntil(this._destroy$)
      ).subscribe(_ => {        
        this.loadLanguages()
      });      

      this._ngxTranslateService.onLangChange.pipe(
          takeUntil(this._destroy$)
      ).subscribe((e:LangChangeEvent) => this.applyTranslations());
  }

  ngOnDestroy(): void {
    this._destroy$.next();
  }

  private onLanguageChange(event, inst) {       
    let val = inst.getVal();    
    this.setLanguage(val ? +val : null);
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
    
    this.isLoadingLanguage = true;
    this._languagesService.getLanguages().pipe(
        take(1),
        finalize(() => this.isLoadingLanguage = false)        
    ).subscribe((data) => {        
        this.languagesCtrl.refreshData(data);
    });
  }
  
  private applyTranslations() {          
      let options = this.languagesCtrl.options;
      if (this.languagesCtrl.instance) {
          this.languagesCtrl.instance.buttons.set.text = this._ngxTranslateService.instant("Välj");
          this.languagesCtrl.instance.buttons.cancel.text = this._ngxTranslateService.instant("Avbryt");
      } else{
          this.languagesCtrl.setText = this._ngxTranslateService.instant("Välj");
          this.languagesCtrl.cancelText  = this._ngxTranslateService.instant("Avbryt");
      }        
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


