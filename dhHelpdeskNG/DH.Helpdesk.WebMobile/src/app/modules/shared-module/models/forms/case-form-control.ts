import { FormControl, ValidatorFn } from "@angular/forms";
import { BehaviorSubject } from "rxjs";

export class CaseFormControl extends FormControl {
  label: string;
  isRequired = false;
  isHidden = false;
  private isSubmitted$ = new BehaviorSubject<boolean>(false);

  constructor(label: string, value: any, validators: ValidatorFn[]) {
    super(value, validators);
    this.label = label;
  }

  public get isSubmitted() {
    return this.isSubmitted$.value;
  }

  public submit(value = true) {
    this.isSubmitted$.next(value);
  }
}