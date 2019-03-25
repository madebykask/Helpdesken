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
import { routerNgProbeToken } from '@angular/router/src/router_module';

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
    if (this.userSettingsService.getUserData().createCasePermission) {
       this.caseTemplateService.loadTemplates().pipe(
        take(1)
      ).subscribe(templates => 
        this.canCreateCases$.next(templates && templates.length > 0));
    }

    // apply translations
    //this.languagesCtrl.setText = this.ngxTranslateService.instant("Välj");
    //this.languagesCtrl.cancelText  = this.ngxTranslateService.instant("Avbryt");    
  }

  openLanguages() {
    this.router.navigate(['language']);
  } 

  logout() {
    this.goTo('/login');
  } 

  goTo(url: string = null) {  
    this.router.navigateByUrl(url);
  } 

}