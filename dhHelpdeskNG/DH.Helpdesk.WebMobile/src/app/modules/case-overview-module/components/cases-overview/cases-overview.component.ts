import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { finalize, take, distinctUntilChanged, takeUntil, filter, catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { Subject, forkJoin, throwError } from 'rxjs';
import { PagingConstants, SortOrder } from 'src/app/modules/shared-module/constants';
import { CasesOverviewFilter } from '../../models/cases-overview/cases-overview-filter.model';
import { CaseOverviewItem } from '../../models/cases-overview/cases-overview-item.model';
import { CasesOverviewService } from '../../services/cases-overview';
import { CaseProgressFilter, CaseStandardSearchFilters } from '../../models/cases-overview/enums';
import { DateTime } from 'luxon';
import { TranslateService } from '@ngx-translate/core';
import { CaseRouteReuseStrategy } from 'src/app/helpers/case-route-resolver.stategy';
import { SearchFilterService } from '../../services/cases-overview/search-filter.service';
import { AppStore, AppStoreKeys } from 'src/app/store';
import { FavoriteFilterModel } from '../../models/cases-overview/favorite-filter.model';
import { CaseSortFieldModel } from 'src/app/modules/case-edit-module/services/model/case-sort-field.model';
import { LocalStorageService } from 'src/app/services/local-storage';
import { CaseSearchStateModel } from 'src/app/modules/shared-module/models/cases-overview/case-search-state.model';

@Component({
  selector: 'app-cases-overview',
  templateUrl: './cases-overview.component.html',
  styleUrls: ['./cases-overview.component.scss']
})
export class CasesOverviewComponent implements OnInit, OnDestroy {
  @ViewChild('searchInput') searchInput: any;

  private selectedFilterId: number;
  private filter: CasesOverviewFilter;
  private destroy$ = new Subject();
  private defaultHeaderText = this.ngxTranslateService.instant('Ärendeöversikt');
  private pageSize = 10;
  private DefaultSortField = 'RegTime';
  private DefaultSortOrder = SortOrder.SortDesc;

  headerText: string;
  DateTime: DateTime;
  showSearchPanel = false;
  filtersForm: FormGroup;
  cases: CaseOverviewItem[] = [];
  isLoading = false;
  favoriteFilters: FavoriteFilterModel[] = null;
  selectedSortFieldId: string;
  selectedSortFieldOrder: string;
  caseSortFields: CaseSortFieldModel[];

  listviewSettings: any = {
    enhance: false,
    swipe: false,
    animateAddRemove: false,
    // an event to handle load on demand when scrolling results to the end
    onListEnd: (event, inst) => {
      if (!this.isLoading) {
          this.filter.Page += 1;
          this.search();
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
    this.selectedFilterId = CaseStandardSearchFilters.AllCases;
    this.pageSize = this.caclucatePageSize();
    this.selectedSortFieldId = this.DefaultSortField;
    this.selectedSortFieldOrder = this.DefaultSortOrder;

    const filtersLoaded$ = new Subject<FavoriteFilterModel[]>();

    // get case sort fields
    const sortFields$ = this.casesOverviewService.getCaseSortingFields();

    // run initial search after filter state (sorting, filters) is fully loaded
    forkJoin(sortFields$, filtersLoaded$).pipe(
      take(1),
      catchError(err => throwError(err))
    ).subscribe(([sortFields, favFilters]: [CaseSortFieldModel[], FavoriteFilterModel[]]) => {
        // load sort state
        const searchState = this.localStorageService.getCaseSearchState();
        if (searchState) {
          if (searchState.sortField) {
            this.selectedSortFieldId = searchState.sortField || this.DefaultSortField;
            this.selectedSortFieldOrder = searchState.sortOrder || this.DefaultSortOrder;
          }
          // filter state if any
          const stateFilterId = searchState && searchState.filterId ? +searchState.filterId : +CaseStandardSearchFilters.AllCases;
          if (!isNaN(stateFilterId) && stateFilterId !== +CaseStandardSearchFilters.AllCases) {
            const ff = favFilters.find(f => f.id === stateFilterId);
            if (ff) {
              this.headerText = ff.name;
              this.selectedFilterId = stateFilterId;
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
     this.appStore.select<FavoriteFilterModel[]>(AppStoreKeys.FavoriteFilters).pipe(
      takeUntil(this.destroy$),
      distinctUntilChanged(),
      filter(m => m && m.length > 0)
    ).subscribe((favFilters: FavoriteFilterModel[]) => {
        // trigger filters load is complete
        filtersLoaded$.next(favFilters);
        filtersLoaded$.complete();
    });

    //clear case page snapshots in reuse strategy
    CaseRouteReuseStrategy.deleteSnaphots();
  }

  processFilterChanged(filterChangeArg) {
    this.selectedFilterId = +filterChangeArg.filterId;
    this.headerText = filterChangeArg.filterName ? filterChangeArg.filterName : this.defaultHeaderText;
    this.saveSearchState();

    // run new search
    this.runNewSearch();
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
    this.goToCase(this.cases[event.index].id);
  }

  private runNewSearch() {
    this.initSearchFilter();
    this.resetCases();
    this.search();
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
    this.filter = new CasesOverviewFilter();
    this.filter.FreeTextSearch = this.filtersForm.controls.freeSearch.value;
    this.filter.InitiatorSearchScope = 0; // TODO: use enum instead
    this.filter.PageSize =  this.pageSize || PagingConstants.pageSize;
    this.filter.Page = PagingConstants.page;
    this.filter.Ascending = this.selectedSortFieldOrder === SortOrder.SortAsc;
    this.filter.OrderBy = this.selectedSortFieldId;
    this.filter.SearchInMyCasesOnly = this.selectedFilterId === +CaseStandardSearchFilters.MyCases;
    this.filter.CaseProgress = CaseProgressFilter.CasesInProgress;

    // Apply case search favorite filter values
    if (this.selectedFilterId > 0) {
      //get selected filter info from app store state
      const filters = this.appStore.state.favFilters.filter(f => f.id === this.selectedFilterId);
      if (filters && filters.length) {
        this.searchFilterService.applyFavoriteFilter(this.filter, filters[0]);
      }
    }
  }

  private caclucatePageSize(): number {
    const headerSize = 53;
    const caseElemSize = 130; // TODO: get real height from UI
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

  trackByFn(index, item: CaseOverviewItem) {
    return item.id;
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

}
