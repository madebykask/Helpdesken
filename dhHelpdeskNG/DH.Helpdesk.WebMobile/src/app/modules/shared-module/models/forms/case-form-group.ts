import { FormGroup, ValidatorFn, AbstractControlOptions, AsyncValidatorFn } from "@angular/forms";
import { CaseFormControl } from "./case-form-control";
import { BehaviorSubject } from "rxjs";

export class CaseFormGroup extends FormGroup {
  private isSubmitted$ = new BehaviorSubject<boolean>(false);
  constructor(controls: { [key: string]: CaseFormControl; }, 
      validatorOrOpts?: ValidatorFn | ValidatorFn[] | AbstractControlOptions | null, 
      asyncValidator?: AsyncValidatorFn | AsyncValidatorFn[] | null) {
    super(controls, validatorOrOpts, asyncValidator);
  }

  public get isSubmitted() {
    return this.isSubmitted$.value;
  }

  public submit(value = true) {
    this.isSubmitted$.next(value);
    this.caseControls.forEach((ctrl) => ctrl.submit());
  }

  public get caseControls(): CaseFormControl[] {
    return Object.keys(this.controls)
    .map(k => this.controls[k] as CaseFormControl);
  }

  public get(name: string) {
    return <CaseFormControl>super.get(name);
  }

}