import { FormGroup, ValidatorFn, AbstractControlOptions, AsyncValidatorFn, AbstractControl,
         FormArray, FormControl, ValidationErrors } from '@angular/forms';
import { CaseFormControl } from './case-form-control';
import { BehaviorSubject } from 'rxjs';
import { NotifierFormFieldsSetter } from './notifier-form-fields-setter';

export class CaseFormGroup extends FormGroup {
  private isSubmitted$ = new BehaviorSubject<boolean>(false);

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
              if (ctrl.validator != null) {
                const isError = ctrl.validator(ctrl);
                if (isError) {
                  ctrl.setErrors(isError, { emitEvent: false });
                }
              }

              if (ctrl.errors != null) {
                errors = {...errors, ...ctrl.errors };
              }
            });
            return errors;
          };
        }
  }

  get(name: string): CaseFormControl {
    return <CaseFormControl>super.get(name);
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
    const control = <CaseFormControl>super.get(name);
    if (control === undefined || control === null) return;

    control.setValue(val, { emitEvent: raiseValueChange });
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
    if (!invalidControls) invalidControls = [];

    input = input || this;
    if (input instanceof FormControl) {
        if (input.invalid || (input.disabled && input.errors != null)) invalidControls.push(input);
        return invalidControls;
    }

    if (!(input instanceof FormArray) && !(input instanceof FormGroup) ) return invalidControls;

    const controls = input.controls;
    for (const name of Object.keys(controls)) {
        const control = controls[name];
        if (control.invalid || (input.disabled && input.errors != null)) invalidControls.push( control );
        switch (control.constructor.name) {
            case 'FormArray':
                (<FormArray> control).controls.forEach( _control => invalidControls = this.findInvalidControls(_control, invalidControls));
                break;

            case 'FormGroup':
                invalidControls = this.findInvalidControls( control, invalidControls );
                break;
        }
    }
    return invalidControls;
  }

  getNotifierFieldsSetter(isRegarding) {
    return new NotifierFormFieldsSetter(isRegarding, this);
  }
}
