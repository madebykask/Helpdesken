import { Injectable } from "@angular/core";
import { take, map } from "rxjs/operators";
import { CaseTemplateModel } from "../../models/caseTemplate/case-template.model";
import { Observable } from "rxjs";
import { CaseTemplateApiService } from "../api/caseTemplate/case-template-api.service";

@Injectable({ providedIn: 'root' })
export class CaseTemplateService {

  protected constructor(private caseTemplateApiService: CaseTemplateApiService ) {
  }

  loadTemplates(): Observable<CaseTemplateModel[]> {
    //tood: move to a separate service
    return this.caseTemplateApiService.getCaseTemplates().pipe(
      take(1),
      map(data => {
        return data.map(x => Object.assign(new CaseTemplateModel(), { ...x, id: x.caseSolutionId }));
      })
    );
  } 
  
}