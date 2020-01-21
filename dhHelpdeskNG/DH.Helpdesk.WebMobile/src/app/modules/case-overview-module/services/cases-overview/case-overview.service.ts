import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, catchError, tap, take } from 'rxjs/operators';
import { HttpApiServiceBase } from 'src/app/modules/shared-module/services/api/httpServiceBase';
import { CasesOverviewFilter } from '../../models/cases-overview/cases-overview-filter.model';
import { CaseOverviewItem, CaseOverviewColumn } from '../../models/cases-overview/cases-overview-item.model';
import { LocalStorageService } from 'src/app/services/local-storage';
import { Observable, throwError } from 'rxjs';
import { CaseSortFieldModel } from 'src/app/modules/case-edit-module/services/model/case-sort-field.model';

@Injectable({ providedIn: 'root' })
export class CasesOverviewService extends HttpApiServiceBase {

    protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService) {
        super(http, localStorageService);
    }

    searchCasesCount(filter: CasesOverviewFilter): Observable<number> {
      const requestUrl = this.buildResourseUrl('/api/casesoverview');
      return this.postJson<any>(requestUrl, filter).pipe(
          map(data => data && data.count ? +data.count : 0)
      );
    }

    searchCases(filter: CasesOverviewFilter): Observable<CaseOverviewItem[]> {
        const requestUrl = this.buildResourseUrl('/api/casesoverview');
        return this.postJson<any>(requestUrl, filter).pipe(
            //tap(d => console.log('>>searchCases')),
            map(data => {
                const cases: CaseOverviewItem[] = [];
                if (data && data.items) {
                    for (const item of data.items) {
                        const _case = new CaseOverviewItem();
                        _case.caseIcon = item.caseIcon;
                        _case.id = item.id;
                        _case.isUnread = item.isUnread;
                        _case.showCustomerName = item.showCustomerName;
                        _case.isUrgent = item.isUrgent;
                        _case.columns = new Array<CaseOverviewColumn>();
                        for (const itemCol of item.columns) {
                            const col = new CaseOverviewColumn();
                            col.id = itemCol.id;
                            col.key = itemCol.key;
                            col.stringValue = itemCol.stringValue;
                            col.dateTimeValue = itemCol.dateTimeValue;
                            //col.TranslateThis
                            //col.TreeTranslation
                            _case.columns.push(col);
                        }
                        cases.push(_case);
                    }
                }
                return cases;
            })
        );
    }

    getCaseSortingFields(): Observable<CaseSortFieldModel[]> {
      const requestUrl = this.buildResourseUrl('/api/casesoverview/sortingfields', null, true, true);
      return this.getJson<any[]>(requestUrl).pipe(
        take(1),
        tap(x => console.log('fields are loaded!')),
        map(data => data && data.length ? data.map(x => new CaseSortFieldModel(x.text, x.fieldId)) : []),
        catchError(err => throwError(err))
      );
  }
}
