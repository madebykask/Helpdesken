import { Component, OnInit } from '@angular/core';
import { take, filter, distinctUntilChanged, takeUntil, delay, switchMap, map, share } from 'rxjs/operators';
import { Router } from '@angular/router';
import { MbscListviewOptions } from '@mobiscroll/angular';
import { SearchFilterService } from 'src/app/modules/case-overview-module/services/cases-overview/search-filter.service';
import { AppStore, AppStoreKeys } from 'src/app/store';
import { FavoriteFilterModel, CustomerFavoriteFilterModel } from 'src/app/modules/case-overview-module/models/cases-overview/favorite-filter.model';
import { BehaviorSubject, Subject, from } from 'rxjs';
import { CaseProgressFilter } from 'src/app/modules/case-overview-module/models/cases-overview/enums';
import { CasesOverviewFilter } from 'src/app/modules/case-overview-module/models/cases-overview/cases-overview-filter.model';
import { CasesOverviewService } from 'src/app/modules/case-overview-module/services/cases-overview';
import { CustomerApiService } from 'src/app/services/api/customer-api.service';
import { LocalStorageService } from 'src/app/services/local-storage';
import { CaseOverviewConstants } from 'src/app/modules/shared-module/constants';

export class SearchFilterViewData {
  isLoading$ = new BehaviorSubject(true);

  constructor(public id: string, public name: string, public customerId: number, public count= 0) {
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
  private selectedCustomerId: number;

  userFilters: SearchFilterViewData[] = [];
  filters: CustomerFavoriteFilterModel[] = [];

  options: MbscListviewOptions = {
    fillAnimation: false,
    animateIcons: false,
    animateAddRemove: false,
    swipe: false,
    select: 'off'
  };

  constructor(private searchFilterService: SearchFilterService,
              private casesOverviewService: CasesOverviewService,
              private customerApiService: CustomerApiService,
              private localStorageService: LocalStorageService,
              private appStore: AppStore,
              private router: Router) {
  }

  ngOnInit() {
    this.selectedCustomerId = this.localStorageService.getCurrentUser().currentData.selectedCustomerId;

    //load search filters
    this.loadSearchFilters();
  }

  private loadSearchFilters() {
    this.appStore.select<CustomerFavoriteFilterModel[]>(AppStoreKeys.FavoriteFilters).pipe(
      takeUntil(this.destroy$),
      distinctUntilChanged(),
      filter(Boolean), // aka new Boolean(val) to filter null values
      switchMap((filters: CustomerFavoriteFilterModel[]) => {
        if (!filters.length) {
          return from([]);
        }
        // build menu items
        let tempFilters = new Array<SearchFilterViewData>();
        // show first default customer
        this.filters = filters.filter(cf => cf.customerId > 0 && cf.customerId === this.selectedCustomerId);
        this.filters = this.filters.concat(filters.filter(cf => cf.customerId > 0 && cf.customerId !== this.selectedCustomerId));
        this.filters.forEach(cf => {
         tempFilters = tempFilters.concat(cf.favorites.map(f => new SearchFilterViewData(f.id, f.name, cf.customerId)));
        });
        this.userFilters = tempFilters;

        // return filters separately to run one by one
        return from(this.filters);
      }),
      switchMap((cf: CustomerFavoriteFilterModel) => {
        return from(cf.favorites);
      }),
      //filter(f => f.id === -1),!! For DEBUG
      delay(150)
    ).subscribe((ff: FavoriteFilterModel) => {
        //run search
        this.runFilterSearch(ff);
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
    const isMyCases = filterModel.id.startsWith(CaseOverviewConstants.CaseOverviewCustomerPrefix);
    filterData.SearchInMyCasesOnly = isMyCases;
    filterData.CaseProgress = CaseProgressFilter.CasesInProgress;
    // set filter params to run count search query only
    filterData.CountOnly = true;
    filterData.Page = 0;
    filterData.PageSize = 0;
    filterData.CustomersIds = this.appStore.state.favFilters
                                .filter(cf => cf.customerId > 0 && cf.favorites.some(f => f.id === filterModel.id))
                                .map(f => f.customerId);

    if (!isMyCases && filterModel && filterModel.fields) {
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

  onItemTap(e: any, customerId: number) {
    const customerFilters = this.customerFilters(customerId);
    const userFilter = customerFilters[e.index];
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

  trackByFiltersFn(index, item: CustomerFavoriteFilterModel) {
    return item.customerId;
  }

  customerFilters(customerId: number) {
    return this.userFilters.filter(f => f.customerId === customerId);
  }

}
