import { FormGroup, ValidatorFn, AbstractControlOptions, AsyncValidatorFn, AbstractControl, FormArray, FormControl } from "@angular/forms";
import { CaseFormControl } from "./case-form-control";
import { BehaviorSubject } from "rxjs";
import { CaseFieldsNames } from "../../constants";

export class CaseFormGroup extends FormGroup {
  private isSubmitted$ = new BehaviorSubject<boolean>(false);
  
  constructor(controls: { [key: string]: CaseFormControl; }, 
      validatorOrOpts?: ValidatorFn | ValidatorFn[] | AbstractControlOptions | null, 
      asyncValidator?: AsyncValidatorFn | AsyncValidatorFn[] | null) {
    super(controls, validatorOrOpts, asyncValidator);
  }
    
  get(name: string):CaseFormControl {
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

  setSafe(name:string, val:any, raiseValueChange:boolean = true) {
    const control = <CaseFormControl>super.get(name);
    if (control === undefined || control === null)
      return;
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
        if (input.invalid) invalidControls.push(input);
        return invalidControls;
    }

    if (!(input instanceof FormArray) && !(input instanceof FormGroup) ) return invalidControls;

    const controls = input.controls;
    for (const name in controls) {
        let control = controls[name];
        if (control.invalid) invalidControls.push( control );
        switch( control.constructor.name )
        {    
            case 'FormArray':
                (<FormArray> control ).controls.forEach( _control => invalidControls = this.findInvalidControls( _control, invalidControls ) );
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

export class NotifierFormFieldsSetter {
  constructor(private isRegarding: boolean, private form: CaseFormGroup) {    
  }

  setRegion(val) {
    this.setSafe(CaseFieldsNames.RegionId, val);
  }

  setDepartment(val) {
    this.setSafe(CaseFieldsNames.DepartmentId, val);
  }

  setOU(val) {
    this.setSafe(CaseFieldsNames.OrganizationUnitId, val);
  }

  setReportedBy(val:string) {
    this.setSafe(CaseFieldsNames.ReportedBy, val || '');
  }

  setPersonName(val:string) {
    this.setSafe(CaseFieldsNames.PersonName, val || '');
  }

  setPersonEmail(val:string) {
    this.setSafe(CaseFieldsNames.PersonEmail, val || '');
  }

  setPersonCellPhone(val:string) {
    this.setSafe(CaseFieldsNames.PersonCellPhone, val || '');
  }

  setPlace(val:string) {
    this.setSafe(CaseFieldsNames.Place, val || '');
  }
    
  setUserCode(val:string) {
    this.setSafe(CaseFieldsNames.UserCode, val || '');
  }
    
  setCostCenter(val:string) {
    this.setSafe(CaseFieldsNames.CostCentre, val || '');
  }

  private setSafe(fieldNameBase:string, val: any) {
    const fieldName = this.isRegarding? 'IsAbout_' + fieldNameBase : fieldNameBase;
    this.form.setSafe(fieldName, val);
  }

}