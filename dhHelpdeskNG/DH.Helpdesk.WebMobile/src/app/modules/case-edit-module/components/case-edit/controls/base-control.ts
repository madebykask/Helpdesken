import { FormGroup, AbstractControl } from "@angular/forms";
import { Input } from "@angular/core";
import { IBaseCaseField } from "../../../models";

export class BaseControl {
    @Input() form: FormGroup;
    @Input('readonly') readOnly: false;
    
    protected formControl: AbstractControl;
    protected isRequired = false;

    constructor() {

    }

    protected init(field: IBaseCaseField<any>) {
      this.formControl = this.getFormControl(field.name);
      this.isRequired = field.isRequired;
    }

    protected getFormControl(name: string) {
        if (this.form == null) return null;

        return this.form.get(name);
    }

/*     protected hasRequiredField(abstractControl: AbstractControl): boolean {
      if (abstractControl.validator) {
          const validator = abstractControl.validator({}as AbstractControl);
          if (validator && validator.required) {
              return true;
          }
      }
      if (abstractControl['controls']) {
          for (const controlName in abstractControl['controls']) {
              if (abstractControl['controls'][controlName]) {
                  if (this.hasRequiredField(abstractControl['controls'][controlName])) {
                      return true;
                  }
              }
          }
      }
      return false;
  } */

  public get requiredSymbol(): string {
    return this.isRequired ? ' *' : '';
  }
}