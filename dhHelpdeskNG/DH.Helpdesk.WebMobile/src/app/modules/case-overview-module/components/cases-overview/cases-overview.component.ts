import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { finalize, take, distinctUntilChanged, takeUntil, filter, catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { Subject, forkJoin, throwError } from 'rxjs';
import { PagingConstants, SortOrder, CaseFieldsNames, CaseOverviewConstants } from 'src/app/modules/shared-module/constants';
import { CasesOverviewFilter } from '../../models/cases-overview/cases-overview-filter.model';
import { CaseOverviewItem } from '../../models/cases-overview/cases-overview-item.model';
import { CasesOverviewService } from '../../services/cases-overview';
import { CaseProgressFilter, CaseStandardSearchFilters, InitiatorSearchScope } from '../../models/cases-overview/enums';
import { DateTime } from 'luxon';
import { TranslateService } from '@ngx-translate/core';
import { CaseRouteReuseStrategy } from 'src/app/helpers/case-route-resolver.stategy';
import { SearchFilterService } from '../../services/cases-overview/search-filter.service';
import { AppStore, AppStoreKeys } from 'src/app/store';
import { CustomerFavoriteFilterModel } from '../../models/cases-overview/favorite-filter.model';
import { CaseSortFieldModel } from 'src/app/modules/case-edit-module/services/model/case-sort-field.model';
import { LocalStorageService } from 'src/app/services/local-storage';
import { CaseSearchStateModel } from 'src/app/modules/shared-module/models/cases-overview/case-search-state.model';

@Component({
  selector: 'app-cases-overview',
  templateUrl: './cases-overview.component.html',
  styleUrls: ['./cases-overview.component.scss']
})
export class CasesOverviewComponent implements OnInit, OnDestroy {
  @ViewChild('searchInput', { static: false }) searchInput: any;

  private selectedFilterId: string;
  private filter: CasesOverviewFilter;
  private destroy$ = new Subject();
  private defaultHeaderText = this.ngxTranslateService.instant('Ärendeöversikt');
  private defaultCustomerText = '';
  private pageSize = 10;
  private DefaultSortField = 'RegTime';
  private DefaultSortOrder = SortOrder.SortDesc;

  headerText: string;
  customerText: string;
  DateTime: DateTime;
  showSearchPanel = false;
  filtersForm: FormGroup;
  cases: CaseOverviewItem[] = [];
  isLoading = false;
  favoriteFilters: CustomerFavoriteFilterModel[] = null;
  selectedSortFieldId: string;
  selectedSortFieldOrder: string;
  caseSortFields: CaseSortFieldModel[];
  caseFieldsNames = CaseFieldsNames;

  listviewSettings: any = {
    enhance: false,
    swipe: false,
    animateAddRemove: false,
    // an event to handle load on demand when scrolling results to the end
    onListEnd: (event, inst) => {
      if (!this.isLoading) {
        if (this.filter) {
          this.filter.Page += 1;
          this.search();
        }
      }
    }
  };

  constructor(private casesOverviewService: CasesOverviewService,
              private formBuilder: FormBuilder,
              private router: Router,
              private appStore: AppStore,
              private localStorageService: LocalStorageService,
              private searchFilterService: SearchFilterService,
              private ngxTranslateService: TranslateService) {
  }

  ngOnInit() {
    //create form
    this.filtersForm = this.formBuilder.group({
      freeSearch: ['']
    });

    //set default values
    this.headerText = this.defaultHeaderText;
    this.customerText = this.defaultCustomerText;
    this.selectedFilterId = CaseStandardSearchFilters.AllCases;
    this.pageSize = this.caclucatePageSize();
    this.selectedSortFieldId = this.DefaultSortField;
    this.selectedSortFieldOrder = this.DefaultSortOrder;

    const filtersLoaded$ = new Subject<CustomerFavoriteFilterModel[]>();

    // get case sort fields
    const sortFields$ = this.casesOverviewService.getCaseSortingFields();

    // run initial search after filter state (sorting, filters) is fully loaded
    forkJoin([sortFields$, filtersLoaded$]).pipe(
      take(1),
      catchError(err => throwError(err))
    ).subscribe(([sortFields, favFilters]: [CaseSortFieldModel[], CustomerFavoriteFilterModel[]]) => {
        // load sort state
        const searchState = this.localStorageService.getCaseSearchState();
        if (searchState) {
          if (searchState.sortField) {
            this.selectedSortFieldId = searchState.sortField || this.DefaultSortField;
            this.selectedSortFieldOrder = searchState.sortOrder || this.DefaultSortOrder;
          }
          // filter state if any
          const stateFilterId = searchState && searchState.filterId ? searchState.filterId : CaseStandardSearchFilters.AllCases;
          if (stateFilterId != null && stateFilterId !== CaseStandardSearchFilters.AllCases) {
            const ff = favFilters.find(cf => cf.favorites.find(f => f.id === stateFilterId));
            if (ff) {
              this.headerText = ff.favorites.find(f => f.id === stateFilterId).name;
              this.updateCustomerText(stateFilterId);
              this.selectedFilterId = stateFilterId;
            } else {
              this.selectedFilterId = favFilters[0].favorites[0].id;
            }
          }
        }

        //set binding properties
        this.caseSortFields = sortFields;
        this.favoriteFilters = favFilters;

        // run cases search
        this.runNewSearch();
    });

     // get search filters from app store (loaded on app.component init)
     this.appStore.select<CustomerFavoriteFilterModel[]>(AppStoreKeys.FavoriteFilters).pipe(
      takeUntil(this.destroy$),
      distinctUntilChanged(),
      filter(m => m && m.length > 0)
    ).subscribe((favFilters: CustomerFavoriteFilterModel[]) => {
        // trigger filters load is complete
        filtersLoaded$.next(favFilters);
        filtersLoaded$.complete();
    });

    //clear case page snapshots in reuse strategy
    CaseRouteReuseStrategy.deleteSnaphots();
  }

  processFilterChanged(filterChangeArg) {
    this.selectedFilterId = filterChangeArg.filterId;
    this.headerText = filterChangeArg.filterName ? this.ngxTranslateService.instant(filterChangeArg.filterName) : this.defaultHeaderText;
    this.updateCustomerText(this.selectedFilterId);
    this.saveSearchState();

    // run new search
    this.runNewSearch();
  }

  private updateCustomerText(id: string) {
    const isOnlyOneCustomer = this.appStore.state.favFilters.filter(cf => cf.customerId !== -1).length === 1;
    const ff = this.appStore.state.favFilters.find(cf => cf.favorites.find(f => f.id === id));
    if (!isOnlyOneCustomer && ff && ff.customerId > 0) {
      this.customerText = ff.customerName;
    } else {
      this.customerText = this.defaultCustomerText;
    }
  }

  processSortingChanged(sortArgs) {
    if (sortArgs && sortArgs.sortField) {
      this.selectedSortFieldId = sortArgs.sortField;
      this.selectedSortFieldOrder = sortArgs.sortOrder;
      this.saveSearchState();

      // run new search
      this.runNewSearch();
    }
  }

  applyFilterAndSearch() {
    if (this.searchInput.element.blur != null) { // on android/ios removing focus from field hides keyboard
      this.searchInput.element.blur();
    }
    this.runNewSearch();
  }

  cancelSearch() {
    if (this.isLoading) { return; }
    const defaultValue = '';

    if (this.filtersForm.controls.freeSearch.value != defaultValue) {
       this.filtersForm.controls.freeSearch.setValue(defaultValue);
       this.applyFilterAndSearch();
    }

    this.showSearchPanel = false;
  }

  onItemTap(event) {
    const selectedCase = this.cases[event.index];
    this.goToCase(selectedCase.id);
  }

  trackByFn(index, item: CaseOverviewItem) {
    return item.id;
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private runNewSearch() {
    this.initSearchFilter();
    this.resetCases();
    this.search();
  }

  private saveSearchState() {
    let state = this.localStorageService.getCaseSearchState();
    if (state === null) {
      state = new CaseSearchStateModel();
    }
    state.filterId = this.selectedFilterId;
    state.sortField = this.selectedSortFieldId;
    state.sortOrder = this.selectedSortFieldOrder;
    this.localStorageService.setCaseSearchState(state);
  }

  private search() {
    this.isLoading = true;
    this.casesOverviewService.searchCases(this.filter).pipe(
        take(1),
        finalize(() => this.isLoading = false),
        // catchError(err => {}) // TODO:
      ).subscribe(
        data => {
          if (data != null && data.length > 0) {
            this.cases = this.cases.concat(data);
          }
        });
  }

  private initSearchFilter() {
    const isMyCasesOnly = this.selectedFilterId === CaseStandardSearchFilters.MyCases ||
                           this.selectedFilterId.startsWith(CaseOverviewConstants.CaseOverviewCustomerPrefix);
    this.filter = new CasesOverviewFilter();
    this.filter.FreeTextSearch = this.filtersForm.controls.freeSearch.value;
    this.filter.InitiatorSearchScope = +InitiatorSearchScope.UserAndIsAbout;
    this.filter.PageSize = this.pageSize || PagingConstants.pageSize;
    this.filter.Page = PagingConstants.page;
    this.filter.Ascending = this.selectedSortFieldOrder === SortOrder.SortAsc;
    this.filter.OrderBy = this.selectedSortFieldId;
    this.filter.SearchInMyCasesOnly = isMyCasesOnly;
    this.filter.CaseProgress = CaseProgressFilter.CasesInProgress;
    // default customers - all available
    this.filter.CustomersIds = this.appStore.state.favFilters.filter(f => f.customerId > 0).map(f => f.customerId);

    // Apply case search favorite filter values
    if (this.selectedFilterId !== null && this.selectedFilterId !== CaseStandardSearchFilters.MyCases) {
      //get selected filter info from app store state
      const ff = this.appStore.state.favFilters.find(cf => cf.favorites.some(f => f.id === this.selectedFilterId));
      if (ff) {
        this.filter.CustomersIds = [ ff.customerId ];
        if (!isMyCasesOnly) {
          this.searchFilterService.applyFavoriteFilter(this.filter, ff.favorites.find(f => f.id === this.selectedFilterId));
        }
      }
    }
  }

  private caclucatePageSize(): number {
    const headerSize = 53;
    const caseElemSize = 85; // TODO: get real height from UI
    const windowHeight = window.innerHeight;
    const defaultPageSize = 2;
    const size = ((windowHeight - headerSize) / caseElemSize) + 1 || defaultPageSize;
    return Math.floor(size > defaultPageSize ? size : defaultPageSize);
  }

  private goToCase(caseId: number) {
    if (caseId <= 0) { return; }
    this.router.navigate(['/case', caseId ]);
  }

  private resetCases() {
    this.cases = [];
  }
}
