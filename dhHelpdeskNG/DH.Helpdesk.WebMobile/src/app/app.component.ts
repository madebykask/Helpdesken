import { Component, OnInit, OnDestroy } from '@angular/core';
import { mobiscroll } from '@mobiscroll/angular';
import { config } from '@env/environment';
import { AuthenticationStateService } from './services/authentication';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { CommunicationService, Channels } from './services/communication';
import { UserSettingsApiService } from './services/api/user/user-settings-api.service';
import { CaseTemplateService } from './services/case-organization/case-template.service';
import { SearchFilterService } from './modules/case-overview-module/services/cases-overview/search-filter.service';
import { AppStore, AppStoreKeys } from './store';
import { CurrentUser } from './models';
import { takeUntil, take } from 'rxjs/operators';
import { CaseTemplateModel, CustomerCaseTemplateModel } from './models/caseTemplate/case-template.model';
import { CustomerFavoriteFilterModel } from './modules/case-overview-module/models/cases-overview/favorite-filter.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject();
  pageSettings = {};

  bottomMenuSettings = {
    type: 'bottom',
  };

  version = config.version;
  // navStart: Observable<NavigationStart>;

  constructor(private authenticationService: AuthenticationStateService,
    private communicationService: CommunicationService,
    private userSettingsService: UserSettingsApiService,
    private caseTemplateService: CaseTemplateService,
    private searchFilterService: SearchFilterService,
    private appStore: AppStore,
    private router: Router) {

    mobiscroll.settings = { theme: 'ios', lang: 'en', labelStyle: 'stacked' };

    // if user is already logged in - run app state init immediately
    if (this.authenticationService.isAuthenticated()) {
      this.initAppStoreState();
    }

    // also subscribe to the login event to reload state on new login
    this.communicationService.listen<CurrentUser>(Channels.UserLoggedIn)
      .pipe(
        takeUntil(this.destroy$)
      )
      .subscribe(e => {
        this.initAppStoreState();
      });
  }

  ngOnInit(): void {
    const version = this.authenticationService.getVersion();
    const isAuthenticated = this.authenticationService.isAuthenticated();

    if (isAuthenticated && config.version !== version) {
      //this.logger.log('>>> Logout: version changed');
      this.router.navigate(['/login']);
    }
  }

  private initAppStoreState() {
    this.loadTemplates();
    this.loadSearchFilters();
  }

  private loadTemplates() {
    if (this.userSettingsService.getUserData().createCasePermission) {
      this.caseTemplateService.loadTemplates().pipe(
       take(1),
       takeUntil(this.destroy$)
     ).subscribe((templates: CustomerCaseTemplateModel[]) => {
       this.appStore.set(AppStoreKeys.Templates, templates);
     });
   }
  }

  // load search filters
  private loadSearchFilters() {
    this.searchFilterService.loadFavoriteFilters().pipe(
      take(1),
      takeUntil(this.destroy$)
    ).subscribe((filters: CustomerFavoriteFilterModel[]) => {
      this.appStore.set(AppStoreKeys.FavoriteFilters, filters);
    });
  }

  ngOnDestroy(): void {
      this.destroy$.next();
      this.destroy$.complete();
  }
}
