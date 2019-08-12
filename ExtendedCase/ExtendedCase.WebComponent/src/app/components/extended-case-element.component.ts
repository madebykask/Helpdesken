import { Component, Input, ViewChild, Output, ViewEncapsulation, NgZone, EventEmitter } from '@angular/core';
import { ExtendedCaseComponent } from './extended-case.component';
import { FormParametersModel, FormAssignmentParameters } from '../models/form-parameters.model';
import { FieldValueModel } from '../models/form-data.model';

@Component({
  template: '<extended-case></extended-case>',
  styleUrls: ['../../styles/css/bootstrap.scss' , '../../styles/css/site.scss'],
  /* Emulated is used - IE, Edge doesn't support ShadowDom even with polyfills
    If ShadowDow or Native is used - remove :host ::ng-deep from bootstrap scss */
  encapsulation: ViewEncapsulation.Emulated
})
export class ExtendedCaseElementComponent {

  // events
  @Output() formLoadSuccess = new EventEmitter();
  @Output() formLoadError = new EventEmitter();

  @Output() formSaveSuccess = new EventEmitter();
  @Output() formSaveError = new EventEmitter();

  @Output() validationComplete = new EventEmitter<any>();
  
  @Output() caseValuesRead = new EventEmitter<any>();
  @Output() formParametersRead = new EventEmitter<any>();

  @ViewChild(ExtendedCaseComponent, {static: true}) exCaseCmp: ExtendedCaseComponent;

  private _caseValues: {} = null;
  private _validationResult: {} = null;
  private _formParameters: {} = null;

  get validationResult(): any {
    return this._validationResult;
  }

  get caseValues() {
    return this._caseValues;
  }

  get getFormParameters() {
    return this._formParameters;
  }

  @Input()
  set loadForm(state: any) {
    console.log('>>> loadForm was called: %s', JSON.stringify(state));
    this.ngZone.run(() => {
      this.doLoadForm(state);
    });
  }

  @Input()
  set saveForm(state: boolean | null) {
    console.log('>>> saveForm: %s', state);
    this.ngZone.run(() => {
      this.doSaveForm(state);
    });
  }

  @Input()
  set setInitialData(state: any) {
    console.log('>>> setInitialData: %s', state);
    this.ngZone.run(() => {
      this.exCaseCmp.setInitialData(state);
    });
  }

  @Input()
  set validateForm(state: boolean | null) {
    console.log('>>> validateForm: %s', state);
    this.ngZone.run(() => {
      this.doValidate(state);
    });
  }

  @Input()
  set getCaseValues(state: any) {
    console.log('>>> getCaseValues: %s', state);
    this.ngZone.run(() => {
      this._caseValues = this.exCaseCmp.getCaseValues();
      this.caseValuesRead.emit({data: this._caseValues});
    });
  }

  @Input()
  set getFormParameters(state: any) {
    console.log('>>> getFormParameters: %s', state);
    this.ngZone.run(() => {
      this._formParameters = this.exCaseCmp.getFormParameters();
      this.formParametersRead.emit({data: this._formParameters});
    });
  }

  @Input()
  set setNextStep(state: any) {
    console.log('>>> setNextStep: %s', state);
    this.ngZone.run(() => {
      this.exCaseCmp.setNextStep(state.step, state.isNextValidation);
    });
  }

  @Input()
  set updateCaseFieldValues(state: any) {
    console.log('>>> updateCaseFieldValues: %s', state);
    this.ngZone.run(() => {
      this.doUpdateCaseFieldValues(state);
    });
  }

  constructor(private ngZone: NgZone) { }

  private doLoadForm(state: any) {
      this.exCaseCmp.loadForm({
        formParameters: this.createFormParametersModel(state.formParameters),
        caseValues: this.createCaseValues(state.caseValues)
      }).then(
        (data: {success: boolean}) => {
          if (data.success === true) {
            this.formLoadSuccess.emit(data.success.toString());
          } else  {
            this.formLoadError.emit(data.success.toString());
          }
        },
        (res: any) => {
          let msg = 'Unknown error.';
          if ('message' in res) {
              msg += res.message;
          }
          console.error(msg);
          this.formLoadError.emit(msg);
        }
      );
  }

  private doSaveForm(state: boolean | null) {
    this.exCaseCmp.save(state)
      .then((res: any) => {
        let exCaseGuid = 'unknown';
        if ('extendedCaseGuid' in res) {
          exCaseGuid = res.extendedCaseGuid.toString();
        }
        this.formSaveSuccess.emit({ data: exCaseGuid });
      },
      (err: any) => {
        let msg = 'Failed to save. ';
        msg += (err instanceof Error) ? err.message : err.toString();
        this.formSaveError.emit(msg);
      });
  }

  private doValidate(state: boolean | null) {
    const res = this.exCaseCmp.validate(state);
    if (res) {
      this._validationResult = res;
      this.validationComplete.emit({ data: res });
    }
  }

  private doUpdateCaseFieldValues(state: any) {
    if (state) {
      const fieldValues = {};
      Object.keys(state).forEach(prop => {
        const fv = new FieldValueModel(state[prop]);
        fieldValues[prop] = fv;
      });
      this.exCaseCmp.updateCaseFieldValues(fieldValues);
    }
  }

  private createFormParametersModel(data: any): FormParametersModel | null {
    const parametersModel = <FormParametersModel>Object.assign(new FormParametersModel(), data, {
      assignmentParameters: new FormAssignmentParameters(data.userRole, data.caseStatus, data.customerId)
    });
    return parametersModel;
  }

  // expects: key - value object
  private createCaseValues(data: any): {[id: string]: FieldValueModel} | null {
    const caseValues: {[id: string]: FieldValueModel} = {};
    Object.keys(data).forEach(prop => {
      const val = data[prop];
      caseValues[prop] = new FieldValueModel(val);
    });
    return caseValues;
  }

}
