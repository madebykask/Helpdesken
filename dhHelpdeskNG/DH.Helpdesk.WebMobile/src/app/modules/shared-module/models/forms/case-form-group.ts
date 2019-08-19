import {
  FormGroup, ValidatorFn, AbstractControlOptions, AsyncValidatorFn, AbstractControl,
  FormArray, FormControl, ValidationErrors
} from '@angular/forms';
import { CaseFormControl } from './case-form-control';
import { BehaviorSubject } from 'rxjs';
import { NotifierFormFieldsSetter } from './notifier-form-fields-setter';
import { CommunicationService, Channels, CaseFieldValueChangedEvent } from 'src/app/services/communication';

export class CaseFormGroup extends FormGroup {
  private isSubmitted$ = new BehaviorSubject<boolean>(false);
  private commService: CommunicationService;

  constructor(controls: { [key: string]: CaseFormControl; },
    validatorOrOpts?: ValidatorFn | ValidatorFn[] | AbstractControlOptions | null,
    asyncValidator?: AsyncValidatorFn | AsyncValidatorFn[] | null) {
    super(controls, validatorOrOpts, asyncValidator);

    //set default validator function if missing
    if (!validatorOrOpts) {
      this.validator = (group: CaseFormGroup): ValidationErrors => {
        let errors = Object.create(null);

        Object.keys(group.controls).forEach((controlName) => {
          const ctrl = group.get(controlName);
          if (ctrl) {
            if (ctrl.validator) {
              const isError = ctrl.validator(ctrl);
              if (isError) {
                ctrl.setErrors(isError, { emitEvent: false });
              }
            }

            if (ctrl.errors != null) {
              errors = { ...errors, ...ctrl.errors };
            }
          }
        });
        return errors;
      };
    }
  }

  get(name: string): CaseFormControl {
    if (this.controls.hasOwnProperty(name)) {
      return <CaseFormControl>super.get(name);
    }
    return undefined;
  }

  setCommService(commService: CommunicationService) {
    this.commService = commService;
  }

  getValue(name: string) {
    const noField = undefined;
    if (!this.contains(name)) {
      return noField;
    }
    const control = this.get(name);
    return control != null ? control.value || null : noField; // null - value is null, undefined - no such field
  }

  setSafe(name: string, val: any, raiseValueChange: boolean = true) {
    const control = this.get(name);
    if (control === undefined || control === null) { return; }

    control.setValue(val, { emitEvent: raiseValueChange });
  }

  setValueWithNotification(fieldName: string, value: any) {
    this.setSafe(fieldName, value);
    this.commService.publish(Channels.CaseFieldValueChanged, new CaseFieldValueChangedEvent(value, null, fieldName));
  }

  get isSubmitted() {
    return this.isSubmitted$.value;
  }

  submit(value = true) {
    this.isSubmitted$.next(value);
    this.caseControls.forEach((ctrl) => ctrl.submit());
  }

  get caseControls(): CaseFormControl[] {
    return Object.keys(this.controls).map(k => this.controls[k] as CaseFormControl);
  }

  findInvalidControls(input: AbstractControl | null = null, invalidControls: AbstractControl[] | null = null): AbstractControl[] {
    if (!invalidControls) { invalidControls = []; }

    input = input || this;
    if (input instanceof FormControl) {
      if (input.invalid || (input.disabled && input.errors != null)) { invalidControls.push(input); }
      return invalidControls;
    }

    if (!(input instanceof FormArray) && !(input instanceof FormGroup)) { return invalidControls; }

    const controls = input.controls;
    for (const name of Object.keys(controls)) {
      const control = controls[name];
      if (control.invalid || (input.disabled && input.errors != null)) { invalidControls.push(control); }
      switch (control.constructor.name) {
        case 'FormArray':
          (<FormArray>control).controls.forEach(_control => invalidControls = this.findInvalidControls(_control, invalidControls));
          break;

        case 'FormGroup':
          invalidControls = this.findInvalidControls(control, invalidControls);
          break;
      }
    }
    return invalidControls;
  }
}
