import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MbscSelect, MbscSelectOptions, MbscNavOptions } from '@mobiscroll/angular';
import { take, finalize, map } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { LanguagesApiService } from 'src/app/services/api/language/languages-api.service';
import { CasesSearchType } from 'src/app/modules/shared-module/constants';
import { UserSettingsApiService } from "src/app/services/api/user/user-settings-api.service";
import { CaseTemplateService } from 'src/app/services/case-organization/case-template.service';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit  {
  searchType = CasesSearchType;
  languageId: number = 0;
  isLoadingLanguage: boolean = true;
  isVisible = true;

  canCreateCases$ = new BehaviorSubject<boolean>(false);
  
  @ViewChild('languages') languagesCtrl: MbscSelect;
  
  languagesSettings: MbscSelectOptions = {
    cssClass: 'languages-list',
    showOnTap: false,
    display: 'bottom',
    data: [],
    onSet: (event, inst) => this.onLanguageChange(event, inst)
  };

  bottomMenuSettings: MbscNavOptions = {
    type: 'bottom',
    moreText: null,
    moreIcon: 'fa-ellipsis-h',
    menuIcon: null,
    menuText: null,
  };
 
  constructor(private router: Router,
              private userSettingsService : UserSettingsApiService,
              private languagesService: LanguagesApiService,
              private caseTemplateService: CaseTemplateService,
              private ngxTranslateService: TranslateService) {
  }

  ngOnInit() {    
    this.loadLanguages();
    if (this.userSettingsService.getUserData().createCasePermission) {
       this.caseTemplateService.loadTemplates().pipe(
        take(1)
      ).subscribe(templates => 
        this.canCreateCases$.next(templates && templates.length > 0));
    }

    // apply translations
    this.languagesCtrl.setText = this.ngxTranslateService.instant("Välj");
    this.languagesCtrl.cancelText  = this.ngxTranslateService.instant("Avbryt");    
  }

  openLanguages() {
    if (!this.isLoadingLanguage) {
        this.languagesCtrl.instance.show();
    }
  } 

  setLanguage(languageId: number) {
    if (languageId) {
      this.userSettingsService.setCurrentLanguage(languageId);

      // reload will reopen the app
      window.location.reload(true);
    }
  }

  logout() {
    this.goTo('/login');
  } 

  goTo(url: string = null) {  
    this.router.navigateByUrl(url);
  } 

  private onLanguageChange(event, inst) {
    let val = inst.getVal();
    this.setLanguage(val ? +val : null);
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

}