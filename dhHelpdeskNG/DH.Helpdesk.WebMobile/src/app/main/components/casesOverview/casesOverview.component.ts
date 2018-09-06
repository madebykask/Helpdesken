import { Component, OnInit } from '@angular/core';
import { CaseOverviewItem, CasesOverviewFilter } from '../../../models'
import { CasesOverviewService } from '../../../services/casesOverview';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserSettingsService } from '../../../services/user';
import { map, finalize, catchError } from 'rxjs/operators';

@Component({
  selector: 'app-casesOverview',
  templateUrl: './casesOverview.component.html',
  styleUrls: ['./casesOverview.component.scss']
})
export class CasesOverviewComponent implements OnInit {

  filtersForm: FormGroup;
  cases: CaseOverviewItem[] = [];
  loading: boolean = false;

  listviewSettings: any = {
    theme: 'auto',
    enhance: true,
  }

  constructor(private casesOverviewService: CasesOverviewService,
              private formBuilder: FormBuilder,
              private userSettingsService: UserSettingsService) { }

  ngOnInit() {
    this.filtersForm = this.formBuilder.group({
      freeSearch: ['']     
    });
  }

  search() {
    let filter = new CasesOverviewFilter();
    filter.FreeTextSearch = this.filtersForm.controls.freeSearch.value;
    filter.CustomerId = this.userSettingsService.getUserData().selectedCustomerId
    filter.InitiatorSearchScope = 0;//TODO: use enum
    filter.CaseProgress = -1;//TODO: use enum
    filter.PageSize = 5;
    filter.Page = 0;

    this.loading = true;
    this.casesOverviewService.searchCases(filter)
      .pipe(
        finalize(() => this.loading = false),
        //catchError(err => {})//TODO:
      )
      .subscribe(
        data => {
          this.cases = data;
        },
/*         error => {},
        () => {
          
        } */
      );
  }

}
