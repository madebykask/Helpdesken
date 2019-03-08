import { FormGroup, AbstractControl } from "@angular/forms";
import { Input } from "@angular/core";
import { IBaseCaseField } from "../../../models";
import { Subject } from "rxjs";
import { CaseOptions } from "src/app/modules/shared-module/models";
import { CaseFieldOptions } from "src/app/modules/shared-module/constants";

export class BaseControl {
    @Input() form: FormGroup;
    @Input('readonly') readOnly: false;

    private fieldName = '';
    private defaultRequiredMessage = 'Field is required.'// TODO: Move to const
    private defaultMaxLengthMessage = 'TODO: MaxLength message here.'// TODO: Move to const
    protected formControl: AbstractControl;
    protected isRequired = false;
    protected destroy$ = new Subject();

    constructor() {
    }

    protected init(field: IBaseCaseField<any>) {
      this.fieldName = field.name;
      this.formControl = this.getFormControl(field.name);
      this.isRequired = field.isRequired;
    }

    protected getFormControl(name: string) {
        if (this.form == null) return null;
        return this.form.get(name);
    }

    protected onDestroy() {
      this.destroy$.next();
      this.destroy$.complete();
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

  public getErrorState() {
    return this.formControl.invalid; // TODO: Move and add isSubmitted
  }

  public get requiredSymbol(): string {
    return this.isRequired ? ' *' : '';
  }

  public getErrorMessage(): string { // TODO: Move and add isSubmitted
    var message = '';
    if (this.formControl && this.formControl.errors) {
        for (var err in this.formControl.errors) {
            if (!message && this.formControl.errors[err]) {
              if (this.errorMessages[this.fieldName] && this.errorMessages[this.fieldName][err]) {
                return this.errorMessages[this.fieldName][err];
              }
              if (err == CaseFieldOptions.required) {
                return this.defaultRequiredMessage;
              }
              if (err == CaseFieldOptions.maxlength) {
                return this.defaultMaxLengthMessage;
              }
            }
        }
    }
    return message;
  }

   errorMessages = {//TODO: just for test. Move to other place
      username: {
          required: 'Username required',
          minlength: 'Has to be at least 2 characters'
      },
      gender: {
          required: 'Gender required'
      },
      bio: {
          required: 'Bio required',
          minlength: "Don't be shy, surely you can tell more"
      },
      email: {
          required: 'Email address required',
          email: 'Invalid email address'
      },
      password: {
          required: 'Password required',
          minlength: 'At least 6 characters required'
      }
  } 
}