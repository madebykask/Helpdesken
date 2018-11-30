import { Injectable } from "@angular/core";
import { CaseApiService } from "../api/case/case-api.service";
import { FormGroup } from "@angular/forms";
import { CaseEditOutputModel } from "src/app/models";
import { CaseFieldsNames } from "src/app/helpers/constants";
import { of } from "rxjs";
import { switchMap } from "rxjs/operators";

@Injectable({ providedIn: 'root' })
export class CaseSaveService {

  protected constructor(private _caseApiService: CaseApiService ) {
  }

  public saveCase(form: FormGroup, caseId?: number) {
    let model = new CaseEditOutputModel();
    model.caseId = caseId;
    model.performerId = this.getValue<number>(form, CaseFieldsNames.PerformerUserId);
    model.responsibleUserId = this.getValue<number>(form, CaseFieldsNames.CaseResponsibleUserId);

    return this._caseApiService.saveCaseData(model)
      .pipe(
          switchMap((r: any) => {
            return of(r)
          })
        )
  }

  private getValue<T>(form: FormGroup, fieldName: string): T {
    if (this.hasValue(form, fieldName)) {
      return <T>form.controls[fieldName].value;
    }
    return undefined;
  }

  private hasValue(form: FormGroup, fieldName: string): boolean {
    return form.controls[fieldName] != null;
  }
}