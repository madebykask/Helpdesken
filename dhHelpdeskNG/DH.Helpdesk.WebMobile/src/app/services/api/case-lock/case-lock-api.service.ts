import { Injectable } from "@angular/core";
import { HttpApiServiceBase } from "..";
import { LocalStorageService } from "../../local-storage";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { take, map } from "rxjs/operators";


@Injectable({ providedIn: 'root' })
export class CaseLockApiService extends HttpApiServiceBase {

  constructor(protected http: HttpClient, protected localStorageService: LocalStorageService) {
    super(http, localStorageService);
  }

  reExtendedCaseLock(lockGuid: string, extendValue: number): Observable<Boolean>{
      const data = {
          lockGuid : lockGuid,
          extendValue: extendValue
      };
      const requestUrl = this.buildResourseUrl('/api/case/extendlock', null, true, false);
      return this.postJson<Boolean>(requestUrl, data)
          .pipe(
              take(1),
              // tap(res => console.log('>>>> reexend case lock: ' + res)),
              map((res: Boolean) => res)
          );
  }

  unLockCase(lockGuid: string): Observable<Boolean> {
      const requestUrl = this.buildResourseUrl('/api/case/unlock', null, true, false);
      return this.postJson<Boolean>(requestUrl, { lockGuid: lockGuid })
          .pipe(
              take(1),
              // tap(res => console.log('>>>> unlock case lock: ' + res)),
              map((res: Boolean) => res)
          );
  }
}
