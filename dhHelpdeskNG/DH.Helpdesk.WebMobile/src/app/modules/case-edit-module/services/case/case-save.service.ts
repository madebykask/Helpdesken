import { Injectable } from "@angular/core";
import { CaseApiService } from "../api/case/case-api.service";
import { FormGroup } from "@angular/forms";
import { of, throwError } from "rxjs";
import { switchMap } from "rxjs/operators";
import { isNumeric } from "rxjs/internal/util/isNumeric";
import { CaseEditOutputModel } from "../../models";
import { CaseFieldsNames } from "src/app/modules/shared-module/constants";

@Injectable({ providedIn: 'root' })
export class CaseSaveService {

  protected constructor(private caseApiService: CaseApiService ) {
  }

  public saveCase(form: FormGroup, caseId?: number) {
    let model = new CaseEditOutputModel();
    model.caseId = caseId;
    model.performerId = this.getNumericValue(form, CaseFieldsNames.PerformerUserId);
    model.responsibleUserId = this.getNumericValue(form, CaseFieldsNames.CaseResponsibleUserId);
    model.workingGroupId = this.getNumericValue(form, CaseFieldsNames.WorkingGroupId);
    model.stateSecondaryId = this.getNumericValue(form, CaseFieldsNames.StateSecondaryId);

    return this.caseApiService.saveCaseData(model)
      .pipe(
          switchMap((r: any) => {
            return of(r)
          })
        )
  }

  private getNumericValue(form: FormGroup, fieldName: string): number {
    if(this.hasValue(form, fieldName)) {
      const value = form.controls[fieldName].value;
      if(!value) return null;
      if(isNumeric(value)) { 
        return Number(form.controls[fieldName].value);
      }
      throwError(`Not supported value. Expecting number, but recieved ${typeof(value)}.`)
    }
    return undefined;
  }

  private hasValue(form: FormGroup, fieldName: string): boolean {
    return form.controls[fieldName] != null;
  }
}