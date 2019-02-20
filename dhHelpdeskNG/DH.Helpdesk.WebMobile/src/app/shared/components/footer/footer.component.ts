import { Component, OnInit, OnDestroy, ViewChild, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { MbscPopup, MbscPopupOptions, MbscSelect, MbscSelectOptions, MbscNavOptions, MbscListviewOptions } from '@mobiscroll/angular';
import { take, finalize, map } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { AuthenticationService } from 'src/app/services/authentication';
import { LanguagesApiService } from 'src/app/services/api/language/languages-api.service';
import { CasesSearchType } from 'src/app/modules/shared-module/constants';
import { UserSettingsApiService } from "src/app/services/api/user/user-settings-api.service";
import { CaseApiService } from 'src/app/modules/case-edit-module/services/api/case/case-api.service';
import { CaseTemplateModel } from 'src/app/modules/case-edit-module/models/case/case-template.model';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit, AfterViewInit, OnDestroy {
  private destroy$ = new Subject();
  SearchType = CasesSearchType; // this allows to use enum values in the view

  @ViewChild('caseSearchPopup') caseSearchPopup: MbscPopup;
  @ViewChild('languages') languagesCtrl: MbscSelect;
  
  languagesSettings: MbscSelectOptions = {
    cssClass: 'languages-list',
    showOnTap: false,
    display: 'bottom',
    data: [],
    onSet: (event, inst) => this.onLanguageChange(event, inst)
  };

  bottomMenuSettings: MbscNavOptions = {
    //layout: 'fixed',
    type: 'bottom',
    moreText: null,
    moreIcon: 'fa-ellipsis-h',
    menuIcon: null,
    menuText: null,
  };

  languageId: number = 0;
  isLoadingLanguage: boolean = true;
  isVisible = true;
    
  popupMenuSettings: MbscPopupOptions = {
    buttons: [],
    closeOnOverlayTap: true,
    display: 'bottom',
    cssClass: 'mbsc-no-padding'
  };
 
  menuListSettings: MbscListviewOptions = {
    enhance: true,
    swipe: false
  };

  templates: CaseTemplateModel[] = [];

  constructor(private router: Router,
              private userSettingsService : UserSettingsApiService,
              private authenticationService: AuthenticationService,
              private languagesService: LanguagesApiService,
              private caseApiService: CaseApiService,
              private ngxTranslateService: TranslateService) {
  }

  ngOnInit() {
    this.loadLanguages();
    this.loadTemplates();

    //apply translations
    this.languagesCtrl.setText = this.ngxTranslateService.instant("Välj");
    this.languagesCtrl.cancelText  = this.ngxTranslateService.instant("Avbryt");
  }

  ngAfterViewInit() {
  }

  ngOnDestroy(): void {
    this.destroy$.next();
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
    this.languageId = this.userSettingsService.getCurrentLanguage() || 0;
    if (this.languageId === 0)
       return;
    
    this.isLoadingLanguage = true;
    this.languagesService.getLanguages().pipe(
        take(1),
        finalize(() => this.isLoadingLanguage = false)
    ).subscribe((data) => {
        this.languagesCtrl.refreshData(data);
    });
  }
  
  private loadTemplates() {
    this.caseApiService.getCaseTemplates().pipe(
      take(1),
      map(data => {
        return data.map(x => Object.assign(new CaseTemplateModel(), x));
      })
    ).subscribe((items:CaseTemplateModel[]) => {
        if (items && items.length) {
          items.forEach(x => this.templates.push(x));
        }
    });
  } 

  logout() {
    this.authenticationService.logout();
    this.goTo('/login');
  }

  goToCases(searchType: CasesSearchType) {
    this.caseSearchPopup.instance.hide();
    this.router.navigate(['/casesoverview', CasesSearchType[searchType]]);    
  }

  goTo(url: string = null) {
    if (url == null) return;
    this.router.navigate([url]);
  }

  setLanguage(languageId: number) {
    if (languageId) {
      this.userSettingsService.setCurrentLanguage(languageId);

      // reload will reopen the app
      window.location.reload(true);
    }
  }
}