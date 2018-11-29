import { Component, OnInit, OnDestroy, ViewChild, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { MbscSelect, MbscSelectOptions, MbscNavOptions } from '@mobiscroll/angular';
import { take, finalize } from 'rxjs/operators';
import { UserSettingsService } from 'src/app/services/user';
import { TranslateService } from '@ngx-translate/core';
import { AuthenticationService } from 'src/app/services/authentication';
import { LanguagesApiService } from 'src/app/services/api/language/languages-api.service';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit, AfterViewInit, OnDestroy {
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
  isVisible = true;
  
  constructor(private _router: Router, 
              private _userSettingsService : UserSettingsService, 
              private _authenticationService: AuthenticationService,
              private _languagesService: LanguagesApiService, 
              private _ngxTranslateService: TranslateService) {
  }

  ngOnInit() {
    this.loadLanguages();
    //apply translations
    this.languagesCtrl.setText = this._ngxTranslateService.instant("Välj");
    this.languagesCtrl.cancelText  = this._ngxTranslateService.instant("Avbryt");
  }

  ngAfterViewInit() {
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



