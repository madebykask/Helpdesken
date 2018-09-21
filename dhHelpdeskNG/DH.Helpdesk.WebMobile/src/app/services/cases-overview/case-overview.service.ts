import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest } from '@angular/common/http';
import { map, tap } from 'rxjs/operators';
import { CasesOverviewFilter, CaseOverviewItem, CaseOverviewColumn } from '../../models'
import { HttpApiServiceBase } from '../api'

@Injectable({ providedIn: 'root' })
export class CasesOverviewService extends HttpApiServiceBase {
    
    protected constructor(http: HttpClient){
        super(http);
    }

    searchCases(filter: CasesOverviewFilter) {
        return this.postJson<any>(this.buildResourseUrl('/api/casesoverview/overview'), filter)
        .pipe(
            //tap(d => console.log('>>searchCases')),
            map(data => {
                let cases: CaseOverviewItem[] = [];
                if(data && data.Items)
                {
                    for(let item of data.Items) {
                        let _case = new CaseOverviewItem();
                        _case.CaseIcon = item.CaseIcon;
                        _case.Id = item.Id;
                        _case.Columns = new Array<CaseOverviewColumn>();
                        for(let itemCol of item.Columns) {
                            let col = new CaseOverviewColumn();
                            col.Id = itemCol.Id;
                            col.Key = itemCol.Key;
                            col.StringValue = itemCol.StringValue;
                            col.DateTimeValue = itemCol.DateTimeValue;
                            //col.TranslateThis
                            //col.TreeTranslation
                            _case.Columns.push(col);
                        }
                        cases.push(_case);                        
                    }
                }
                return cases;
            })
        )
    }
}