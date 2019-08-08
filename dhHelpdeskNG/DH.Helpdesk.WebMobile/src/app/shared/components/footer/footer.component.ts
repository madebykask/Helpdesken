import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MbscSelect, MbscNavOptions } from '@mobiscroll/angular';
import { takeUntil, distinctUntilChanged, filter  } from 'rxjs/operators';
import { UserSettingsApiService } from 'src/app/services/api/user/user-settings-api.service';
import { BehaviorSubject, Subject } from 'rxjs';
import { AppStore, AppStoreKeys } from 'src/app/store/app-store';
import { CaseTemplateModel } from 'src/app/models/caseTemplate/case-template.model';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit  {
  languageId = 0;
  isLoadingLanguage = true;
  isVisible = true;

  canCreateCases$ = new BehaviorSubject<boolean>(false);

  @ViewChild('languages', { static: false }) languagesCtrl: MbscSelect;

  private destroy$ = new Subject<any>();

  bottomMenuSettings: MbscNavOptions = {
    type: 'bottom',
    moreText: null,
    moreIcon: 'fa-ellipsis-h',
    menuIcon: null,
    menuText: null,
  };

  constructor(private router: Router,
              private appStore: AppStore,
              private userSettingsService: UserSettingsApiService) {
  }

  ngOnInit() {
    // load templates from appStore state
    if (this.userSettingsService.getUserData().createCasePermission) {
       this.appStore.select<CaseTemplateModel[]>(AppStoreKeys.Templates).pipe(
         distinctUntilChanged(),
         filter(Boolean), // aka new Boolean(val) to filter null values
         takeUntil(this.destroy$)
      ).subscribe((templates: CaseTemplateModel[]) => {
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

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
