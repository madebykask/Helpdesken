import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { finalize, take } from 'rxjs/operators';
import { Router } from '@angular/router';
import { MbscForm, MbscListview } from '@mobiscroll/angular';
import { Subject } from 'rxjs';
import { PagingConstants } from 'src/app/modules/shared-module/constants';
import { CasesOverviewFilter } from '../../models/cases-overview/cases-overview-filter.model';
import { CaseOverviewItem } from '../../models/cases-overview/cases-overview-item.model';
import { CasesOverviewService } from '../../services/cases-overview';
import { CaseProgressFilter, CaseStandardSearchFilters } from '../../models/cases-overview/enums';
import { DateTime } from 'luxon';
import { TranslateService } from '@ngx-translate/core';
import { CaseRouteReuseStrategy } from 'src/app/helpers/case-route-resolver.stategy';
import { SearchFilterService } from '../../services/cases-overview/search-filter.service';
import { AppStore, AppStoreKeys } from 'src/app/store';

@Component({
  selector: 'app-cases-overview',
  templateUrl: './cases-overview.component.html',
  styleUrls: ['./cases-overview.component.scss']
})
export class CasesOverviewComponent implements OnInit, OnDestroy {
  @ViewChild('searchInput') searchInput: any;
  @ViewChild('loading') loadingElem: MbscForm;
  @ViewChild('listview') listView: MbscListview;

  private selectedFilterId: number;
  private filter: CasesOverviewFilter;
  private destroy$ = new Subject();
  private defaultHeaderText = this.ngxTranslateService.instant('Ärendeöversikt');
  private pageSize = 10;

  stateFilterId = 0;
  headerText: string;
  DateTime: DateTime;
  showSearchPanel = false;
  filtersForm: FormGroup;
  cases: CaseOverviewItem[] = [];
  isLoading = false;

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
              private searchFilterService: SearchFilterService,
              private ngxTranslateService: TranslateService) {
  }

  ngOnInit() {
    //create form
    this.filtersForm = this.formBuilder.group({
      freeSearch: ['']
    });

    this.pageSize = this.caclucatePageSize();
    this.headerText = this.defaultHeaderText;

    // run search based on local store filter value
    this.runInitialSearch();

    //clear case page snapshots in reuse strategy
    CaseRouteReuseStrategy.deleteSnaphots();
  }

  private runInitialSearch() {
    //get filterId from local storage
    const filterId = this.searchFilterService.getFilterIdFromState();

    if (filterId !== CaseStandardSearchFilters.AllCases) {
      // verify if state filterId is valid and reset if not. We do this in ctor be onInit to ensure correct filterId for cases-filter presentation component.
      console.log('>> caseOverview: reading filters from state');
      const favoriteFilter = this.appStore.state.favFilters.filter(f => f.id === filterId);
      if (favoriteFilter && favoriteFilter.length) {
        this.headerText = favoriteFilter[0].name;
        this.stateFilterId = filterId;
        this.selectedFilterId = filterId;
      }
      this.searchFilterService.saveFilterIdToState(this.stateFilterId);
    }

    // run cases search
    this.runNewSearch();
  }

  processFilterChanged(filterChangeArg) {
    this.selectedFilterId = +filterChangeArg.filterId;
    this.headerText = filterChangeArg.filterName ? filterChangeArg.filterName : this.defaultHeaderText;
    this.searchFilterService.saveFilterIdToState(this.selectedFilterId);
    this.runNewSearch();
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
    this.filter.Ascending = false;
    this.filter.OrderBy = 'CaseNumber'; // TODO - remove hardcode
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
    const caseElemSize = 60; // TODO: get real height from UI
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
