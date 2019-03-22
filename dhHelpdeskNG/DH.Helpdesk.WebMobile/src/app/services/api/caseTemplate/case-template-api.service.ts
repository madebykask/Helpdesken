import { HttpApiServiceBase } from 'src/app/modules/shared-module/services';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class CaseTemplateApiService extends HttpApiServiceBase {
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
}