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
  private filter: CasesOverviewFilter;
  private scrollBindFunc: any;
  private timer: any;
  private _destroy$ = new Subject();

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
              private userSettingsService: UserSettingsService, 
              private router: Router) {                
               }

  ngOnInit() {
    this.filtersForm = this.formBuilder.group({
      freeSearch: ['']      
    });
    this.pageSize = this.caclucatePageSize();
    this.initFilter();
    this.search();
    this.scrollBindFunc = this.checkLoad.bind(this);
    window.addEventListener('scroll', this.scrollBindFunc);
  }

  ngOnDestroy() {
    window.removeEventListener('scroll', this.scrollBindFunc);
    this._destroy$.next();
  }

  applyFilterAndSearch() {
    this.initFilter();
    this.resetCases();
    this.search();
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
    if(caseId <= 0) return;

    this.router.navigate(['/case', caseId ]);
  }

  private resetCases() {
    this.cases = new Array<CaseOverviewItem>();
  }

  private search() {
    this.isLoading = true;
    this.casesOverviewService.searchCases(this.filter)
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
    this.filter = new CasesOverviewFilter();
    this.filter.FreeTextSearch = this.filtersForm.controls.freeSearch.value;
    this.filter.InitiatorSearchScope = 0;//TODO: use enum
    this.filter.CaseProgress = -1;//TODO: use enum
    this.filter.PageSize =  this.pageSize || PagingConstants.pageSize;
    this.filter.Page = PagingConstants.page;
    this.filter.Ascending = false;
    this.filter.OrderBy = 'CaseNumber';//TODO - remove use hardcode
  }

  private caclucatePageSize(): number {
    const headerSize = 170;
    const caseElemSize = 86;
    const windowHeight = window.innerHeight;
    const defaultPageSize = 2;
    let size = ((windowHeight - headerSize) / caseElemSize) + 1 || defaultPageSize;
    return Math.floor(size > defaultPageSize ? size : defaultPageSize);
  }

}
