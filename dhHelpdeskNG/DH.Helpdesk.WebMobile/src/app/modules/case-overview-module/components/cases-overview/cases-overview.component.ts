import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { finalize, take, map, takeUntil } from 'rxjs/operators';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { MbscForm, MbscListview } from '@mobiscroll/angular';
import { Subject } from 'rxjs';
import { CasesSearchType, PagingConstants } from 'src/app/modules/shared-module/constants';
import { CasesOverviewFilter } from '../../models/cases-overview/cases-overview-filter.model';
import { CaseOverviewItem } from '../../models/cases-overview/cases-overview-item.model';
import { CasesOverviewService } from '../../services/cases-overview';
import { LocalStorageService } from 'src/app/services/local-storage';
import { CaseSearchStateModel } from '../../../shared-module/models/cases-overview/case-search-state.model';
import { CaseProgressFilter } from '../../models/cases-overview/enums';
import { DateTime } from 'luxon';
import { CasesFilterComponent } from '../cases-filter/cases-filter.component';
import { TranslateService } from '@ngx-translate/core';;

@Component({
  selector: 'app-cases-overview',
  templateUrl: './cases-overview.component.html',
  styleUrls: ['./cases-overview.component.scss']
})
export class CasesOverviewComponent implements OnInit, OnDestroy {
  @ViewChild('searchInput') searchInput: any;
  @ViewChild('loading') loadingElem: MbscForm;
  @ViewChild('listview') listView: MbscListview;
  @ViewChild(CasesFilterComponent) casesFilter: CasesFilterComponent;

  private filter: CasesOverviewFilter;
  private searchType = CasesSearchType.AllCases;
  private scrollBindFunc: any;
  private timer: any;
  private destroy$ = new Subject();
  private defaultHeaderText = this.ngxTranslateService.instant('Ärendeöversikt');

  headerText:string;
  DateTime: DateTime;
  showSearchPanel = false;
  filtersForm: FormGroup;
  cases: CaseOverviewItem[] = [];
  isLoading: boolean = false;
  pageSize: number = 10;
  listviewSettings: any = {
    enhance: false,
    swipe: false
  }

  constructor(private casesOverviewService: CasesOverviewService,
              private formBuilder: FormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private localStorage: LocalStorageService,
              private ngxTranslateService: TranslateService) {
  }

  ngOnInit() {
    //console.log('>>> cases-overview: onInit is called!!!')
    this.filtersForm = this.formBuilder.group({
      freeSearch: ['']
    });
    this.pageSize = this.caclucatePageSize();
    this.route.paramMap.pipe(
            map((x:ParamMap) => x.get('searchType'))
                ).subscribe((st:string) => {
                    //console.log('>>> st: ' + st);
                    this.searchType = this.getSearchType(st);
                    this.initFilter();
                    this.resetCases();
                    this.saveSearchState();
                    this.search();
                });
    this.scrollBindFunc = this.checkLoad.bind(this);
    window.addEventListener('scroll', this.scrollBindFunc);    
  }

  ngAfterViewInit(): void {
    //update header on filter change
    this.casesFilter.filterChanged.pipe(
      takeUntil(this.destroy$)
    ).subscribe(x =>
      this.headerText = x ? x : this.defaultHeaderText
    );

    setTimeout(() => this.headerText = this.casesFilter.filterName || this.defaultHeaderText, 200); 
  }

  saveSearchState(): any {
    let state = new CaseSearchStateModel();
    state.SearchType = this.searchType;
    this.localStorage.setCaseSearchState(state);
  }

  private getSearchType(st: string): CasesSearchType {
   return CasesSearchType[st];
  }

  ngOnDestroy() {
    //console.log('>>> cases-overview: destroy is called!!!')
    window.removeEventListener('scroll', this.scrollBindFunc);
    this.destroy$.next();
  }

  applyFilterAndSearch() {
    if (this.searchInput.element.blur != null) { // on android/ios removing focus from field hides keyboard
      this.searchInput.element.blur();
    }
    this.initFilter();
    this.resetCases();
    this.search();
  }

  cancelSearch() {
    if (this.isLoading) return;
    const defaultValue = "";

    if (this.filtersForm.controls.freeSearch.value != defaultValue) {
       this.filtersForm.controls.freeSearch.setValue(defaultValue);
       this.applyFilterAndSearch();
    }

    this.showSearchPanel = false;
  }

  onItemTap(event) {
    this.goToCase(this.cases[event.index].id);
  }

  shouldLoad() {
    var el = this.loadingElem.element;
    // return el.getBoundingClientRect().top + el.clientTop + el.offsetHeight - 1 < (window.innerHeight + window.pageYOffset);
    return el.getBoundingClientRect().top + el.clientTop + el.offsetHeight - 1 < window.innerHeight;
  }

  checkLoad() {
    clearTimeout(this.timer);
    let timer = setTimeout(() => {
        if (!this.isLoading && this.shouldLoad()) {
            this.isLoading = true;
            this.filter.Page += 1;
            this.search();
        }
    }, 250);
    this.timer = timer;
  }

  trackByFn(index, item: CaseOverviewItem) {
    return item.id;
  }

  private goToCase(caseId: number) {
    if (caseId <= 0) return;

    this.router.navigate(['/case', caseId ]);
  }

  private resetCases() {
    this.cases = new Array<CaseOverviewItem>();
  }

  private search() {
    this.isLoading = true;
    this.casesOverviewService.searchCases(this.filter).pipe(
        take(1),
        finalize(() => this.isLoading = false),
        // catchError(err => {})// TODO:
      ).subscribe(
        data => {
          if (data != null && data.length > 0)
          this.cases = this.cases.concat(data);
        });
  }

  private initFilter() {
    this.filter = new CasesOverviewFilter();
    this.filter.FreeTextSearch = this.filtersForm.controls.freeSearch.value;
    this.filter.InitiatorSearchScope = 0;// TODO: use enum
    this.filter.CaseProgress = -1;// TODO: use enum
    this.filter.PageSize =  this.pageSize || PagingConstants.pageSize;
    this.filter.Page = PagingConstants.page;
    this.filter.Ascending = false;
    this.filter.OrderBy = 'CaseNumber';// TODO - remove use hardcode
    this.filter.SearchInMyCasesOnly = Boolean(this.searchType);    
    this.filter.CaseProgress = CaseProgressFilter.CasesInProgress;
  }

  private caclucatePageSize(): number {
    const headerSize = 53;
    const caseElemSize = 60;// TODO: get real height from UI
    const windowHeight = window.innerHeight;
    const defaultPageSize = 2;
    let size = ((windowHeight - headerSize) / caseElemSize) + 1 || defaultPageSize;
    return Math.floor(size > defaultPageSize ? size : defaultPageSize);
  }

}
