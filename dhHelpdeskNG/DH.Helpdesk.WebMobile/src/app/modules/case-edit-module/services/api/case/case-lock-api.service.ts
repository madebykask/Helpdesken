import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { take, map, catchError } from 'rxjs/operators';
import { LocalStorageService } from 'src/app/services/local-storage';
import { CaseLockModel } from '../../../models';
import { HttpApiServiceBase } from 'src/app/modules/shared-module/services/api/httpServiceBase';

@Injectable({ providedIn: 'root' })
export class CaseLockApiService extends HttpApiServiceBase {

  constructor(protected http: HttpClient, protected localStorageService: LocalStorageService) {
    super(http, localStorageService);
  }

  acquireCaseLock(caseId: number, sessionId: string, customerId: number): Observable<CaseLockModel> {
    const data = {
        caseId: caseId,
        sessionId: sessionId
    };
    const requestUrl = this.buildResourseUrl('/api/case/lock', { cid: customerId }, false, false);
    return this.postJson<CaseLockModel>(requestUrl, data).pipe(
        take(1),
        catchError((e) => throwError(e))
    );
  }

  reExtendedCaseLock(caseId: number, lockGuid: string, extendValue: number, customerId: number): Observable<Boolean> {
      const data = {
          caseId: caseId,
          lockGuid : lockGuid,
          extendValue: extendValue
      };
      const requestUrl = this.buildResourseUrl('/api/case/extendlock', { cid: customerId }, false, false);
      return this.postJson<Boolean>(requestUrl, data).pipe(
          take(1),
          map((res: Boolean) => res)
      );
  }

  unLockCase(caseId: number, lockGuid: string, customerId: number): Observable<Boolean> {
      const data = {
        caseId: caseId,
        lockGuid: lockGuid
      };
      const requestUrl = this.buildResourseUrl('/api/case/unlock', { cid: customerId }, false, false);
      return this.postJson<Boolean>(requestUrl, data).pipe(
          take(1),
          map((res: Boolean) => res)
      );
  }
}
