import { Injectable } from "@angular/core";
import { Observable, of } from "rxjs";
import { take } from "rxjs/operators";
import { HttpClient } from "@angular/common/http";
import { LocalStorageService } from "src/app/services/local-storage";
import { CaseEditOutputModel } from "../../../models/case/case-edit-output.model";
import { HttpApiServiceBase } from "src/app/modules/shared-module/services/api/httpServiceBase";

@Injectable({ providedIn: 'root' })
export class CaseApiService extends HttpApiServiceBase {

  constructor(protected http: HttpClient, protected localStorageService: LocalStorageService) {
    super(http, localStorageService);
  }

  getCaseTemplates() : Observable<Array<any>> {
    return this.getJson<Array<any>>(this.buildResourseUrl('/api/templates/', { mobileOnly: true }, true, true)) // TODO: error handling
    .pipe(
        take(1) 
    );
  }

  getCaseTemplate(templateId: number) : Observable<Array<any>> {
    return this.getJson<Array<any>>(this.buildResourseUrl(`/api/case/template/${templateId}`, null, true, true)) // TODO: error handling
    .pipe(
        take(1) 
    );
  }
  
  getCaseData(caseId: number): Observable<any> {
    const userData = this.localStorageService.getCurrentUser();
    let params = null;
    if (userData !== null) {
      params = { sessionId: userData.authData.sessionId };
    }
    let url = this.buildResourseUrl('/api/case/' + caseId, params, true, true);
    return this.getJson(url)// TODO: error handling
        .pipe(
            take(1)
        );
  }
  
  saveCaseData(data: CaseEditOutputModel): Observable<any> {
    const requestUrl = this.buildResourseUrl(`/api/case/save${ !data.caseId ? '' : '/' +data.caseId }`, null, true, true);
    return this.postJson<any>(requestUrl, data)
      .pipe(
        take(1)
      );
  }

  getCaseSections(): Observable<any> {
    return this.getJson(this.buildResourseUrl('/api/casesections/get', null, true, true)) // TODO: error handling
        .pipe(
            take(1)
        );
  }
}