import { Input } from "@angular/core";
import { IBaseCaseField } from "../../../models";
import { Subject } from "rxjs";
import { CaseFormGroup, CaseFormControl } from "src/app/modules/shared-module/models/forms";
import { CaseFieldsCustomErrorMessages, CaseFieldsDefaultErrorMessages } from "../../../logic/constants/case-fields-error-messages.constants";
import { CaseFieldOptions } from "src/app/modules/shared-module/constants";
import { StringUtil } from "src/app/modules/shared-module/utils/string-util";

export class BaseControl<T> {
  @Input() form: CaseFormGroup;
  @Input('readonly') readOnly: false;

  @Input() field: IBaseCaseField<T>;
  public formControl: CaseFormControl;
  protected destroy$ = new Subject();

  constructor() {
  }

  protected init(field: IBaseCaseField<any>) {
    this.formControl = this.getFormControl(field.name);
  }

  protected getFormControl(name: string) {
      if (this.form == null) return null;
      return this.form.get(name);
  }

  protected onDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  protected get isRequired() { 
    return this.formControl.isRequired;
  }

  public getErrorState() {
    return ((this.formControl.disabled && this.formControl.errors != null) || this.formControl.invalid) && this.formControl.isSubmitted;
  }

  public get requiredSymbol(): string {
    return this.isRequired ? ' *' : '';
  }

  public getErrorMessage(): string {
    if (!this.getErrorState()) return;

    let message = '';
    if (this.formControl && this.formControl.errors) {
        for (let err in this.formControl.errors) {
            if (!message && this.formControl.errors[err]) {
              if (CaseFieldsCustomErrorMessages[this.field.name] && CaseFieldsCustomErrorMessages[this.field.name][err]) {
                return CaseFieldsCustomErrorMessages[this.field.name][err];
              }
              if (err == CaseFieldOptions.required) {
                return CaseFieldsDefaultErrorMessages[err];
              }
              if (err == CaseFieldOptions.maxlength) {
                return StringUtil.format(CaseFieldsDefaultErrorMessages[err], this.field.maxLength) ;
              }
              return 'Missing field error description.';
            }
        }
    }
    return message;
  }
}