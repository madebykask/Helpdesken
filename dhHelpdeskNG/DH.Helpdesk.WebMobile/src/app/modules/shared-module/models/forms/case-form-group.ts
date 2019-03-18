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

  public get(name: string) {
    return <CaseFormControl>super.get(name);
  }

  public setSafe(name:string, val:any, raiseValueChange:boolean = true) {
    const control = <CaseFormControl>super.get(name);
    if (control === undefined || control === null)
      return;
    control.setValue(val, { emitEvent: raiseValueChange });
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

}