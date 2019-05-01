import { FormControl, ValidatorFn } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { FormStatuses } from 'src/app/modules/shared-module/constants';
import { IFieldBase } from 'src/app/modules/case-edit-module/models';

export class CaseFormControl extends FormControl {

  fieldInfo: IFieldBase;
  private isSubmitted$ = new BehaviorSubject<boolean>(false);

  constructor(fieldInfo: IFieldBase, value: any, validators: ValidatorFn[]) {
    super(value, validators);
    this.fieldInfo = fieldInfo;
  }

  get label() {
    return this.fieldInfo.label;
  }

  get fieldName() {
    return this.fieldInfo.name;
  }

  public get isSubmitted() {
    return this.isSubmitted$.value;
  }

  public get isDisabled() {
    return this.status === FormStatuses.DISABLED;
  }

  public submit(value = true) {
    this.isSubmitted$.next(value);
  }

}
