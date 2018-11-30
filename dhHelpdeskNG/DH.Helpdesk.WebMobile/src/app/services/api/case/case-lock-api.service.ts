import { Injectable } from "@angular/core";
import { HttpApiServiceBase } from "..";
import { LocalStorageService } from "../../local-storage";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { take, map } from "rxjs/operators";
import { CaseLockModel } from "src/app/models";

@Injectable({ providedIn: 'root' })
export class CaseLockApiService extends HttpApiServiceBase {

  constructor(protected http: HttpClient, protected localStorageService: LocalStorageService) {
    super(http, localStorageService);
  }

  acquireCaseLock(caseId: number, sessionId: string): Observable<CaseLockModel> {
    const data = {
        caseId: caseId,
        sessionId: sessionId
    };
    const requestUrl = this.buildResourseUrl('/api/case/lock', null, true, false);
    return this.postJson<CaseLockModel>(requestUrl, data).pipe(
        take(1)
    );
  }

  reExtendedCaseLock(caseId: number, lockGuid: string, extendValue: number): Observable<Boolean>{
      const data = {
          caseId: caseId,
          lockGuid : lockGuid,
          extendValue: extendValue
      };
      const requestUrl = this.buildResourseUrl('/api/case/extendlock', null, true, false);
      return this.postJson<Boolean>(requestUrl, data).pipe(
          take(1),              
          map((res: Boolean) => res)
      );
  }

  unLockCase(caseId: number, lockGuid: string): Observable<Boolean> {
      const data = {
        caseId: caseId, 
        lockGuid: lockGuid 
      };
      const requestUrl = this.buildResourseUrl('/api/case/unlock', null, true, false);
      return this.postJson<Boolean>(requestUrl, data).pipe(
          take(1),        
          map((res: Boolean) => res)
      );
  }
}
