import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { LocalStorageService } from "src/app/services/local-storage";
import { HttpApiServiceBase } from "src/app/modules/shared-module/services";
import { map, take, switchMap } from "rxjs/operators";

@Injectable({ providedIn: 'root' })
export class CaseWatchDateApiService extends HttpApiServiceBase {
  constructor(httpClient: HttpClient, localStorageService: LocalStorageService) {
      super(httpClient, localStorageService);
  }

  getWatchDate(departmentId: number) {
    return this.getJson(this.buildResourseUrl('/api/casewatchdate', { departmentId: departmentId }, true, false))
    .pipe(
          take(1)
        );
  }
}