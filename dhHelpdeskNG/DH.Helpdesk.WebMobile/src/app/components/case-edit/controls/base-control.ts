import { FormGroup, AbstractControl } from "@angular/forms";
import { Input } from "@angular/core";

export class BaseControl {
    @Input() form: FormGroup;
    @Input('readonly') readOnly: false;
    
    protected formControl: AbstractControl;
    protected isRequired = false;

    constructor() {

    }

    protected init(fieldName: string) {
      this.formControl = this.getFormControl(fieldName);
      this.isRequired = this.hasRequiredField(this.formControl);

    }

    protected getFormControl(name: string) {
        if (this.form == null) return null;

        return this.form.get(name);
    }

    protected hasRequiredField(abstractControl: AbstractControl): boolean {
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
  }

  public get requiredSymbol(): string {
    return this.isRequired ? ' *' : '';
  }
}