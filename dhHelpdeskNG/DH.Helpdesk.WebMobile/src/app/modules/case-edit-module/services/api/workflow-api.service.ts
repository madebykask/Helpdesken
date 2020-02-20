import { Injectable } from '@angular/core';
import { HttpApiServiceBase } from 'src/app/modules/shared-module/services';
import { LocalStorageService } from 'src/app/services/local-storage';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { OptionsHelper } from 'src/app/helpers/options-helper';
import { take, map } from 'rxjs/operators';
import { OptionItem } from 'src/app/modules/shared-module/models';

@Injectable({ providedIn: 'root' })
export class CaseWorkflowsApiService extends HttpApiServiceBase {
  constructor(protected http: HttpClient,
    protected localStorageService: LocalStorageService,
    private caseHelper: OptionsHelper) {
    super(http, localStorageService);
  }

  getWorkflows(caseId: number, customerId: number): Observable<any> {
      let params = { cid: customerId };
      if (caseId != null){
        params = Object.assign(params, { caseId: caseId });
      }

      return this.getJson(this.buildResourseUrl('/api/workflows/options', params, false, true))
      .pipe(
          take(1),
          map((jsItems: any) => {
              return this.caseHelper.toOptionItems(jsItems as Array<any>) || new Array<OptionItem>();
          })
      );
  }
}
