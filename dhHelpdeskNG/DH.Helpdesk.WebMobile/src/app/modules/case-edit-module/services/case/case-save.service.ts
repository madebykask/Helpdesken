import { Injectable } from "@angular/core";
import { CaseApiService } from "../api/case/case-api.service";
import { FormGroup } from "@angular/forms";
import { throwError, Observable } from "rxjs";
import { isNumeric } from "rxjs/internal/util/isNumeric";
import { CaseEditOutputModel, CaseEditInputModel } from "../../models";
import { CaseFieldsNames } from "src/app/modules/shared-module/constants";

@Injectable({ providedIn: 'root' })
export class CaseSaveService {

  protected constructor(private caseApiService: CaseApiService ) {
  }

  public saveCase(form: FormGroup, caseInputData: CaseEditInputModel):Observable<any> {
    let model = new CaseEditOutputModel();
    model.caseId = caseInputData.id;
    model.caseGuid = caseInputData.caseGuid;
    model.performerId = this.getNumericValue(form, CaseFieldsNames.PerformerUserId);
    model.responsibleUserId = this.getNumericValue(form, CaseFieldsNames.CaseResponsibleUserId);
    model.workingGroupId = this.getNumericValue(form, CaseFieldsNames.WorkingGroupId);
    model.stateSecondaryId = this.getNumericValue(form, CaseFieldsNames.StateSecondaryId);
    model.priorityId = this.getNumericValue(form, CaseFieldsNames.PriorityId);
    model.productAreaId = this.getNumericValue(form, CaseFieldsNames.ProductAreaId);
    model.watchDate = this.getDateValue(form, CaseFieldsNames.WatchDate);
    model.logExternalText = form.get(CaseFieldsNames.Log_ExternalText).value;
    model.logInternalText = form.get(CaseFieldsNames.Log_InternalText).value;
    return this.caseApiService.saveCaseData(model);
  }

  private getNumericValue(form: FormGroup, fieldName: string): number {
    if (this.hasValue(form, fieldName)) {
      const value = form.controls[fieldName].value;
      if (!value) return null;
      if (isNumeric(value)) { 
        return Number(form.controls[fieldName].value);
      }
      throwError(`Not supported value. Expecting number, but recieved ${typeof(value)}.`)
    }
    return undefined;
  }

  private getDateValue(form: FormGroup, fieldName: string): string
  {
    if(this.hasValue(form, fieldName)) {
      const value = form.controls[fieldName].value;
      if(!value) return null;
      return form.controls[fieldName].value;
    }
    return undefined;
  }

  private hasValue(form: FormGroup, fieldName: string): boolean {
    return form.controls[fieldName] != null;
  }
}