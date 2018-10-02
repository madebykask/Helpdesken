import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest } from '@angular/common/http';
import { map, tap } from 'rxjs/operators';
import { CasesOverviewFilter, CaseOverviewItem, CaseOverviewColumn } from '../../models'
import { HttpApiServiceBase } from '../api'
import { LocalStorageService } from '../local-storage';

@Injectable({ providedIn: 'root' })
export class CasesOverviewService extends HttpApiServiceBase {
    
    protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService){
        super(http, localStorageService);
    }   
   
    searchCases(filter: CasesOverviewFilter) {        
        let requestUrl = this.buildResourseUrl('/api/casesoverview/overview')
        return this.postJson<any>(requestUrl, filter)
        .pipe(
            //tap(d => console.log('>>searchCases')),
            map(data => {
                let cases: CaseOverviewItem[] = [];
                if(data && data.items)
                {
                    for(let item of data.items) {
                        let _case = new CaseOverviewItem();
                        _case.caseIcon = item.caseIcon;
                        _case.id = item.id;
                        _case.columns = new Array<CaseOverviewColumn>();
                        for(let itemCol of item.columns) {
                            let col = new CaseOverviewColumn();
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
        )
    }
}