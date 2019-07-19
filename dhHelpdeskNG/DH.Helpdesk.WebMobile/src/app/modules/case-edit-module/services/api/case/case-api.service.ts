import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { LocalStorageService } from 'src/app/services/local-storage';
import { CaseEditOutputModel } from '../../../models/case/case-edit-output.model';
import { HttpApiServiceBase } from 'src/app/modules/shared-module/services/api/httpServiceBase';
import { QueryParamsOptions } from 'src/app/modules/shared-module/services/api/query-params-options';

@Injectable({ providedIn: 'root' })
export class CaseApiService extends HttpApiServiceBase {

  constructor(protected http: HttpClient,
    protected localStorageService: LocalStorageService) {
    super(http, localStorageService);
  }

  getCaseData(caseId: number): Observable<any> {
    const userData = this.localStorageService.getCurrentUser();
    const p = userData !== null ?  { sessionId: userData.authData.sessionId } : null;
    const params = this.createQueryParams(p, new QueryParamsOptions(true, true));
    return this.getJsonWithParams('/api/case/' + caseId, params); // TODO: error handling
  }

  saveCaseData(data: CaseEditOutputModel): Observable<any> {
    const requestUrl = this.buildResourseUrl(`/api/case/save${ !data.caseId ? '' : '/' + data.caseId }`, null, true, true);
    return this.postJson<any>(requestUrl, data);
  }

  getCaseSections(): Observable<any> {
    return this.getJson(this.buildResourseUrl('/api/casesections/get', null, true, true)); // TODO: error handling
  }

  getNewCase(templateId: number): Observable<Array<any>> {
    return this.getJson<Array<any>>(this.buildResourseUrl(`/api/case/new/${templateId}`, null, true, true));
  }

}
