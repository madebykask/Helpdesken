import { Injectable } from '@angular/core';
import { HttpApiServiceBase } from 'src/app/modules/shared-module/services';
import { LocalStorageService } from 'src/app/services/local-storage';
import { HttpClient } from '@angular/common/http';
import { take } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CaseHistoryApiService extends HttpApiServiceBase {

  constructor(protected http: HttpClient, protected localStorageService: LocalStorageService) {
    super(http, localStorageService);
  }

  getHistoryEvents(caseId: number): Observable<any> {
    //return of(this._data);
    //todo: implement once api is available
    let url = this.buildResourseUrl(`/api/case/${caseId}/histories`, null, true, true);
    return this.getJson<Array<any>>(url)
        .pipe(
            take(1)
        );
  }

  private _data = JSON.parse(`{
    "emailLogs": [],
    "changes": [{
      "id": 17705,
      "fieldName": "PersonName",
      "fieldLabel": "Anmälare",
      "previousValue": "Stina Svensmo",
      "currentValue": "Björn Gunnarsson",
      "createAt": "2019-01-28T09:47:33.337Z",
      "createBy": "Håkan Gunnarsson"
    },
    {
      "id": 17705,
      "fieldName": "ReportedBy",
      "fieldLabel": "Användar ID",
      "previousValue": "stina",
      "currentValue": "bg@datahalland.se",
      "createAt": "2019-01-28T09:47:33.337Z",
      "createBy": "Håkan Gunnarsson"
    },   
    {
      "id": 17706,
      "fieldName": "PersonName",
      "fieldLabel": "Anmälare",
      "previousValue": null,
      "currentValue": "Stina Svensmo",
      "createAt": "2019-01-28T10:47:33.337Z",
      "createBy": "Håkan Gunnarsson"
    }]
}`);

}
