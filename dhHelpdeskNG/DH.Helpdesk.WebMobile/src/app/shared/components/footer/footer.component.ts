import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MbscSelect, MbscNavOptions } from '@mobiscroll/angular';
import { take  } from 'rxjs/operators';
import { CasesSearchType } from 'src/app/modules/shared-module/constants';
import { UserSettingsApiService } from 'src/app/services/api/user/user-settings-api.service';
import { CaseTemplateService } from 'src/app/services/case-organization/case-template.service';
import { BehaviorSubject } from 'rxjs';
import { AppStore, AppStoreKeys } from 'src/app/store/app-store';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit  {
  searchType = CasesSearchType;
  languageId = 0;
  isLoadingLanguage = true;
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
              private appSore: AppStore,
              private userSettingsService : UserSettingsApiService,
              private caseTemplateService: CaseTemplateService) {
  }

  ngOnInit() {
    if (this.userSettingsService.getUserData().createCasePermission) {
       this.caseTemplateService.loadTemplates().pipe(
        take(1)
      ).subscribe(templates => {
        this.appSore.set(AppStoreKeys.Templates, templates);
        this.canCreateCases$.next(templates && templates.length > 0);
      });
    }

    // apply translations
    // this.languagesCtrl.setText = this.ngxTranslateService.instant("Välj");
    // this.languagesCtrl.cancelText  = this.ngxTranslateService.instant("Avbryt");
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
