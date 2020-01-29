import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MbscNavOptions } from '@mobiscroll/angular';
import { takeUntil, distinctUntilChanged, filter  } from 'rxjs/operators';
import { UserSettingsApiService } from 'src/app/services/api/user/user-settings-api.service';
import { BehaviorSubject, Subject } from 'rxjs';
import { AppStore, AppStoreKeys } from 'src/app/store/app-store';
import { CustomerCaseTemplateModel } from 'src/app/models/caseTemplate/case-template.model';
import { untilDestroyed } from 'ngx-take-until-destroy';

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
       this.appStore.select<CustomerCaseTemplateModel[]>(AppStoreKeys.Templates).pipe(
         distinctUntilChanged(),
         filter(Boolean), // aka new Boolean(val) to filter null values
         untilDestroyed(this)
      ).subscribe((templates: CustomerCaseTemplateModel[]) => {
        this.canCreateCases$.next(templates && templates.length > 0);
      });
    }
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
  }
}
