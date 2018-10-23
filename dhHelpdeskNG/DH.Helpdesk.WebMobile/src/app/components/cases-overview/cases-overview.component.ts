import { Component, OnInit, ViewChild, ElementRef, OnDestroy } from '@angular/core';
import { CaseOverviewItem, CasesOverviewFilter } from '../../models'
import { CasesOverviewService } from '../../services/cases-overview';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserSettingsService } from '../../services/user';
import { map, finalize, catchError, take, takeUntil } from 'rxjs/operators';
import { PagingConstants } from '../../helpers/constants';
import { Router } from '@angular/router';
import { MbscForm, MbscListview } from '@mobiscroll/angular';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-cases-overview',
  templateUrl: './cases-overview.component.html',
  styleUrls: ['./cases-overview.component.scss']
})
export class CasesOverviewComponent implements OnInit, OnDestroy {
  @ViewChild('loading') loadingElem: MbscForm; 
  @ViewChild('listview') listView: MbscListview;
  private _filter: CasesOverviewFilter;
  private _scrollBindFunc: any;
  private _timer: any;  
  private _destroy$ = new Subject();

  showSearchPanel = false;
  filtersForm: FormGroup;
  cases: CaseOverviewItem[] = [];
  isLoading: boolean = false;
  pageSize:number = 10;

  listviewSettings: any = {
    enhance: false,
    swipe: false,
/*     onItemTap: function(event, inst) {
        if(inst && inst.id) this.goToCase(inst.id);
    },
 */  }

  constructor(private casesOverviewService: CasesOverviewService,
              private formBuilder: FormBuilder,
              private router: Router) {                
               }

  ngOnInit() {
    this.filtersForm = this.formBuilder.group({
      freeSearch: ['']      
    });
    this.pageSize = this.caclucatePageSize();
    this.initFilter();
    this.search();
    this._scrollBindFunc = this.checkLoad.bind(this);
    window.addEventListener('scroll', this._scrollBindFunc);
  }

  ngOnDestroy() {
    window.removeEventListener('scroll', this._scrollBindFunc);
    this._destroy$.next();
  }

  applyFilterAndSearch() {
    this.initFilter();
    this.resetCases();
    this.search();
  }

  cancelSearch() {
    if(this.isLoading) return;
    const defaultValue = "";
    if(this.filtersForm.controls.freeSearch.value != defaultValue) {
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
    //return el.getBoundingClientRect().top + el.clientTop + el.offsetHeight - 1 < (window.innerHeight + window.pageYOffset);
    return el.getBoundingClientRect().top + el.clientTop + el.offsetHeight - 1 < window.innerHeight;
  }

  checkLoad() {    
    clearTimeout(this._timer);
    let timer = setTimeout(() => {
        if (!this.isLoading && this.shouldLoad()) {
            this.isLoading = true;
            this._filter.Page += 1;
            this.search();
        }
    }, 250);
    this._timer = timer;
  }

  trackByFn(index, item: CaseOverviewItem) {
    return item.id;
  }

  private goToCase(caseId: number) {
    if(caseId <= 0) return;

    this.router.navigate(['/case', caseId ]);
  }

  private resetCases() {
    this.cases = new Array<CaseOverviewItem>();
  }

  private search() {
    this.isLoading = true;
    this.casesOverviewService.searchCases(this._filter)
      .pipe(
        take(1),
        takeUntil(this._destroy$),
        finalize(() => this.isLoading = false),
        //catchError(err => {})//TODO:
      )
      .subscribe(
        data => {
          if(data != null && data.length > 0)
          this.cases = this.cases.concat(data);
        });
  }

  private initFilter () {
    this._filter = new CasesOverviewFilter();
    this._filter.FreeTextSearch = this.filtersForm.controls.freeSearch.value;
    this._filter.InitiatorSearchScope = 0;//TODO: use enum
    this._filter.CaseProgress = -1;//TODO: use enum
    this._filter.PageSize =  this.pageSize || PagingConstants.pageSize;
    this._filter.Page = PagingConstants.page;
    this._filter.Ascending = false;
    this._filter.OrderBy = 'CaseNumber';//TODO - remove use hardcode
  }

  private caclucatePageSize(): number {
    const headerSize = 53;
    const caseElemSize = 68;//TODO: get real height from UI
    const windowHeight = window.innerHeight;
    const defaultPageSize = 2;
    let size = ((windowHeight - headerSize) / caseElemSize) + 1 || defaultPageSize;
    return Math.floor(size > defaultPageSize ? size : defaultPageSize);
  }

}
