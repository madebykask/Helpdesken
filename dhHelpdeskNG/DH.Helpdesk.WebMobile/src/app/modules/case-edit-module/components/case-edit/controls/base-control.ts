import { Input } from '@angular/core';
import { Subject } from 'rxjs';
import { CaseFormGroup, CaseFormControl } from 'src/app/modules/shared-module/models/forms';
import { CaseFieldsCustomErrorMessages, CaseFieldsDefaultErrorMessages } from '../../../logic/constants/case-fields.constants';
import { CaseFieldOptions } from 'src/app/modules/shared-module/constants';
import { StringUtil } from 'src/app/modules/shared-module/utils/string-util';

export class BaseControl<T> {
  @Input() form: CaseFormGroup;
  @Input() field: string;
  @Input() readOnly: false; //TODO: check if required?

  formControl: CaseFormControl;

  protected destroy$ = new Subject();

  constructor() {
  }

  protected init(fieldName: string) {
    this.formControl = this.getFormControl(fieldName);
  }

  protected getFormControl(name: string): CaseFormControl {
    if (this.form === null) { return null; }
    return this.form.get(name);
  }

  protected get fieldName() {
    return this.field;
  }

  protected get fieldLabel() {
    return this.formControl.label;
  }

  protected get isRequired() {
    return this.formControl.fieldInfo.isRequired;
  }

  get isFormControlDisabled() {
    return this.formControl.isDisabled;
  }

  public getErrorState() {
    return ((this.formControl.disabled && this.formControl.errors != null) || this.formControl.invalid) && this.formControl.isSubmitted;
  }

  public get requiredSymbol(): string {
    return this.isRequired ? ' *' : '';
  }

  public getErrorMessage(): string {
    if (!this.getErrorState()) return;

    const message = '';
    if (this.formControl && this.formControl.errors) {
        for (const err in this.formControl.errors) {
            if (!message && this.formControl.errors[err]) {

              if (CaseFieldsCustomErrorMessages[this.field] && CaseFieldsCustomErrorMessages[this.field][err]) {
                return CaseFieldsCustomErrorMessages[this.field][err];
              }
              if (err === CaseFieldOptions.required) {
                return CaseFieldsDefaultErrorMessages[err];
              }
              if (err === CaseFieldOptions.maxlength) {
                return StringUtil.format(CaseFieldsDefaultErrorMessages[err], this.formControl.fieldInfo.maxLength);
              }
              return 'Missing field error description.';
            }
        }
    }
    return message;
  }

  protected onDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

}
