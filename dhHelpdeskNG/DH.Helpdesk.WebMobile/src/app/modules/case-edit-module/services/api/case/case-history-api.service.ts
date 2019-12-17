import { Injectable } from '@angular/core';
import { HttpApiServiceBase } from 'src/app/modules/shared-module/services';
import { LocalStorageService } from 'src/app/services/local-storage';
import { HttpClient } from '@angular/common/http';
import { take, catchError } from 'rxjs/operators';
import { Observable, of, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CaseHistoryApiService extends HttpApiServiceBase {

  constructor(protected http: HttpClient, protected localStorageService: LocalStorageService) {
    super(http, localStorageService);
  }

  getHistoryEvents(caseId: number, customerId: number): Observable<any> {
    const url = this.buildResourseUrl(`/api/case/${caseId}/histories`, { cid: customerId}, false, true);
    return this.getJson<Array<any>>(url)
        .pipe(
            take(1),
            catchError(
              (error: any, caught: Observable<any>) => {
                  if (error.status === 403) {
                    return of(null);
                  }
                  return throwError(error);
              })
        );
  }
}
