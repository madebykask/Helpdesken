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

  @Output() formLoadSuccess = new EventEmitter();
  @Output() formLoadError = new EventEmitter();

  @ViewChild(ExtendedCaseComponent, {static: true}) exCaseCmp: ExtendedCaseComponent;

  @Input()
  set loadForm(state: any) {
    console.log('>>> loadForm was called: %s', JSON.stringify(state));
    this.doLoadForm(state);
  }

  constructor(private ngZone: NgZone) { }

  private doLoadForm(state: any) {
    this.ngZone.run(() => {
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
    });
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
