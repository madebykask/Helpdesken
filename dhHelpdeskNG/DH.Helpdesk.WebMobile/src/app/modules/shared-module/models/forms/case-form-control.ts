import { FormControl, ValidatorFn } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { FormStatuses } from 'src/app/modules/shared-module/constants';
import { IFieldBase } from 'src/app/modules/case-edit-module/models';

export class CaseFormControl extends FormControl {

  private _fieldInfo: IFieldBase;
  private isSubmitted$ = new BehaviorSubject<boolean>(false);
  private prevValue: any = null;

  constructor(fieldInfo: IFieldBase, value: any, validators: ValidatorFn[]) {
    super(value, validators);
    this._fieldInfo = fieldInfo;
  }

  get label() {
    return this.fieldInfo.label;
  }

  get fieldInfo() {
    return this._fieldInfo;
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

  // overriding (hiding) formControl.setValue method to store prev value
  public setValue(value: any, options?: {
    onlySelf?: boolean;
    emitEvent?: boolean;
    emitModelToViewChange?: boolean;
    emitViewToModelChange?: boolean;
  }): void {
    //console.log(`>>> setValue: value has changed: fieldName: [${this.fieldName}: ${this.value} -> ${value}]`);
    this.prevValue = this.value;
    super.setValue(value, options);
  }

  public restorePrevValue(raiseChangeEvent = true) {
      this.setValue(this.prevValue, { onlySelf: true, emitEvent: raiseChangeEvent });
  }

}
