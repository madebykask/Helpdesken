import { Component, OnInit } from '@angular/core';
import { take, filter, distinctUntilChanged, takeUntil, delay, switchMap, map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { MbscListviewOptions } from '@mobiscroll/angular';
import { SearchFilterService } from 'src/app/modules/case-overview-module/services/cases-overview/search-filter.service';
import { AppStore, AppStoreKeys } from 'src/app/store';
import { FavoriteFilterModel } from 'src/app/modules/case-overview-module/models/cases-overview/favorite-filter.model';
import { BehaviorSubject, Observable, Subject, of, from } from 'rxjs';
import { CaseStandardSearchFilters, CaseProgressFilter } from 'src/app/modules/case-overview-module/models/cases-overview/enums';
import { CasesOverviewFilter } from 'src/app/modules/case-overview-module/models/cases-overview/cases-overview-filter.model';
import { CasesOverviewService } from 'src/app/modules/case-overview-module/services/cases-overview';
import { CustomerApiService } from 'src/app/services/api/customer-api.service';
import { LocalStorageService } from 'src/app/services/local-storage';
import { CustomerModel } from 'src/app/models/customer/customer.model';

export class SearchFilterViewData {
  isLoading$ = new BehaviorSubject(true);

  constructor(public id: number, public name: string, public count= 0) {
  }

  setCountResult(val: number) {
    this.count = val;
    this.isLoading$.next(false);
  }
}

@Component({
  selector: 'cases-status',
  templateUrl: './cases-status.component.html',
  styleUrls: ['./cases-status.component.scss']
})
export class CasesStatusComponent implements OnInit {
  private destroy$ = new Subject<any>();

  userFilters: SearchFilterViewData[] = [];

  options: MbscListviewOptions = {
    fillAnimation: false,
    animateIcons: false,
    animateAddRemove: false,
    swipe: false,
    select: 'off'
  };

  customer$: Observable<CustomerModel>;

  constructor(private searchFilterService: SearchFilterService,
              private casesOverviewService: CasesOverviewService,
              private customerApiService: CustomerApiService,
              private localStorageService: LocalStorageService,
              private appStore: AppStore,
              private router: Router) {
  }

  ngOnInit() {
    const selectedCustomerId = +this.localStorageService.getCurrentUser().currentData.selectedCustomerId;
    if (selectedCustomerId) {
      this.customer$ = this.customerApiService.getCustomer(selectedCustomerId);
    }

    //load search filters
    this.loadSearchFilters();
  }

  private loadSearchFilters() {
    this.appStore.select<FavoriteFilterModel[]>(AppStoreKeys.FavoriteFilters).pipe(
      takeUntil(this.destroy$),
      distinctUntilChanged(),
      filter(Boolean), // aka new Boolean(val) to filter null values
      switchMap((filters: FavoriteFilterModel[]) => {
        // build menu items
        this.userFilters = filters.map(f => new SearchFilterViewData(f.id, f.name));
        // return filters separately to run one by one
        return from(filters);
      }),
      //filter(f => f.id === -1),!! For DEBUG
      delay(150)
    ).subscribe((filterModel: FavoriteFilterModel) => {
        //run search
        this.runFilterSearch(filterModel);
    });
  }

  private runFilterSearch(filterModel: FavoriteFilterModel) {
    //console.log(`>>> run filter '${filterModel.id}' search`);
    const searchFilter = this.createSearchFilter(filterModel);

    this.casesOverviewService.searchCasesCount(searchFilter).pipe(
      take(1)
    ).subscribe(res => {
      //console.log(`>>> search results for filter '${filterModel.id}' arrived: ${res}`);
      this.updateSearchFilterCount({ id: filterModel.id, itemsCount: res});
    });
  }

  private createSearchFilter(filterModel: FavoriteFilterModel) {
    const filterData = new CasesOverviewFilter();
    filterData.SearchInMyCasesOnly = filterModel.id === +CaseStandardSearchFilters.MyCases;
    filterData.CaseProgress = CaseProgressFilter.CasesInProgress;
    // set filter params to run count search query only
    filterData.CountOnly = true;
    filterData.Page = 0;
    filterData.PageSize = 0;

    if (filterModel && filterModel.fields) {
      this.searchFilterService.applyFavoriteFilter(filterData, filterModel);
    }
    return filterData;
  }

  private updateSearchFilterCount(res: any) {
    if (res && res.id) {
      const filterId = res.id;
      const ff = this.userFilters.filter(f => f.id === filterId);
      if (ff && ff.length) {
        ff[0].setCountResult(res.itemsCount || 0);
      }
    }
  }

  onItemTap(e: any) {
    const userFilter = this.userFilters[e.index];
    if (userFilter) {
      this.searchFilterService.saveFilterIdToState(userFilter.id);
      this.router.navigateByUrl('/casesoverview');
    }
  }

  goTo(url: string = null) {
    this.router.navigateByUrl(url);
  }

  trackByFn(index, item: SearchFilterViewData) {
    return item.id;
  }
}
