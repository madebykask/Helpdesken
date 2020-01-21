import { HttpApiServiceBase } from 'src/app/modules/shared-module/services';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';

import { LocalStorageService } from '../../local-storage';
import { HttpClient } from '@angular/common/http';
import { take, map } from 'rxjs/operators';
import { CaseTemplateFullModel } from 'src/app/models/caseTemplate/case-template-full.model';

@Injectable({ providedIn: 'root' })
export class CaseTemplateApiService extends HttpApiServiceBase {

  constructor(protected http: HttpClient, protected localStorageService: LocalStorageService) {
    super(http, localStorageService);
  }

  getCaseTemplates(): Observable<Array<any>> {
    return this.getJson<Array<any>>(this.buildResourseUrl('/api/templates/', { mobileOnly: true }, true, true));
  }

  getCaseTemplate(id: number) {
    return this.getJson(this.buildResourseUrl(`/api/templates/${id}`, { mobileOnly: true }, true, true))
    .pipe(
      take(1),
      map((jsItem: any) => {
        return Object.assign(new CaseTemplateFullModel(), jsItem);
      }));
  }
}

