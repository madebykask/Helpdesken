import { Component, Input, ViewChild, Output, ViewEncapsulation, NgZone, EventEmitter } from '@angular/core';
import { ExtendedCaseElementComponent } from './extended-case-element.component';
import { FormParametersModel, FormAssignmentParameters } from '../models/form-parameters.model';
import { FieldValueModel } from '../models/form-data.model';

@Component({
  template: '<extended-case-element></extended-case-element>',
  styleUrls: ['../../styles/css/bootstrap.scss' , '../../styles/css/site.scss'],
  /* Emulated is used - IE, Edge doesn't support ShadowDom even with polyfills
    If ShadowDow or Native is used - remove :host ::ng-deep from bootstrap scss */
  encapsulation: ViewEncapsulation.Emulated
})
export class ExtendedCaseComponent {

  // events
  @Output() formLoadComplete = new EventEmitter();
  @Output() formSaveComplete = new EventEmitter();
  @Output() validationComplete = new EventEmitter<any>();
/*   @Output() caseValuesRead = new EventEmitter<any>();
  @Output() formParametersRead = new EventEmitter<any>();
 */
  @ViewChild(ExtendedCaseElementComponent, {static: true}) exCaseCmp: ExtendedCaseElementComponent;

  // private _caseValues: {} = null;
  private _validationResult: {} = null;
  // private _formParameters: {} = null;

  @Input()
  public get validationResult(): any {
    return this._validationResult;
  }

  @Input()
  public get getCaseValues() {

    let result = {}
    this.ngZone.run(() => {
      result = this.exCaseCmp.getCaseValues();
    });
    return result;
  }

  @Input()
  public get getFormParameters() {
    return this.exCaseCmp.getFormParameters();
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
          this.formLoadComplete.emit({ isSuccess: data.success });
        },
        (res: any) => {
          let msg = 'Unknown error.';
          if ('message' in res) {
              msg += res.message;
          }
          console.error(msg);
          this.formLoadComplete.emit({ isSuccess: false, msg });
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
        this.formSaveComplete.emit({ isSuccess: true, data: exCaseGuid });
      },
      (err: any) => {
        let msg = 'Failed to save. ';
        msg += (err instanceof Error) ? err.message : err.toString();
        this.formSaveComplete.emit({ isSuccess: false, msg });
      });
  }

  private doValidate(state: boolean | null) {
    const res = this.exCaseCmp.validate(state);
    this._validationResult = res;
    this.validationComplete.emit({ data: res });
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
      caseValues[prop] = new FieldValueModel(val.Value);
    });
    return caseValues;
  }

}
