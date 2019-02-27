import { CaseApiService } from "../api/case/case-api.service";
import { Injectable } from "@angular/core";
import { take, map } from "rxjs/operators";
import { CaseTemplateModel } from "../../models/case/case-template.model";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root' })
export class CaseTemplateService {

  protected constructor(private caseApiService: CaseApiService ) {
  }

  loadTemplates(): Observable<CaseTemplateModel[]> {
    //tood: move to a separate service
    return this.caseApiService.getCaseTemplates().pipe(
      take(1),
      map(data => {
        return data.map(x => Object.assign(new CaseTemplateModel(), { ...x, id: x.caseSolutionId }));
      })
    );
  } 
  
}