import { HttpApiServiceBase } from 'src/app/modules/shared-module/services';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';

import { LocalStorageService } from '../../local-storage';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class CaseTemplateApiService extends HttpApiServiceBase {

  constructor(protected http: HttpClient, protected localStorageService: LocalStorageService) {
    super(http, localStorageService);
  }

  getCaseTemplates() : Observable<Array<any>> {
    return this.getJson<Array<any>>(this.buildResourseUrl('/api/templates/', { mobileOnly: true }, true, true));
  }

  getCaseTemplate(templateId: number) : Observable<Array<any>> {
    return this.getJson<Array<any>>(this.buildResourseUrl(`/api/case/template/${templateId}`, null, true, true));
  }
}