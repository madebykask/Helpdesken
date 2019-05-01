import { Validators } from '@angular/forms';
import { ICaseField, FieldModel, IFieldBase } from 'src/app/modules/case-edit-module/models';
import { CaseFormGroup } from './case-form-group';
import { CaseFormControl } from '.';
import { CaseFieldsNames } from '../../constants';

export class CaseFormGroupBuilder {

  createFormGroup(fields: ICaseField<any>[], canSave: boolean): CaseFormGroup {
    const controls: { [key: string]: CaseFormControl; } = {};

    //create controls from webapi response
    for (const field of fields) {
      controls[field.name] = this.createCaseFormControl(field.value, field, canSave);
    }

    // TODO: review lognotes controls init logic
  
    // extnernal log  EmailsTO:
    controls[CaseFieldsNames.Log_SendMailToNotifier] =
      this.createCaseFormControl(
        true, // by default
        Object.assign(new FieldModel(), {
          name: CaseFieldsNames.Log_SendMailToNotifier,
          options: []
      }),
      canSave);


    // extnernal log  EmailsTO:
    controls[CaseFieldsNames.Log_ExternalEmailsTo] =
      this.createCaseFormControl(
        '', //TODO: init controls below based on info from other fields
        Object.assign(new FieldModel(), {
          name: CaseFieldsNames.Log_ExternalEmailsTo,
          label: 'To', //todo: translate
          options: []
      }),
      canSave);

   // external log EmailsCC:
   controls[CaseFieldsNames.Log_ExternalEmailsCC] =
     this.createCaseFormControl(
       '', //TODO: init controls from web api
       Object.assign(new FieldModel(), {
          name: CaseFieldsNames.Log_ExternalEmailsCC,
          label: 'CC', //todo: translate
          options: []
     }), canSave);

    // internal log TO:
    controls[CaseFieldsNames.Log_InternalEmailsTo] =
      this.createCaseFormControl('', Object.assign(new FieldModel(), {
        name: CaseFieldsNames.Log_InternalEmailsTo,
        label: 'To', //todo: translate
        options: []
      }), canSave);

    // internal log CC:
    controls[CaseFieldsNames.Log_InternalEmailsCC] =
      this.createCaseFormControl('', Object.assign(new FieldModel(), {
        name: CaseFieldsNames.Log_InternalEmailsCC,
        label: 'CC', //todo: translate
        options: []
      }), canSave);

    return new CaseFormGroup(controls);
  }

  private createCaseFormControl(value: any, field: IFieldBase, canSave: boolean) {
    const validators = this.createValidators(field);
    const control = new CaseFormControl(field, {
        value: value == null ? '' : value,
        disabled: !canSave || field.isReadonly,
      }, validators);
    return control;
  }

  private createValidators(field: IFieldBase) {
    const validators = [];

    if (!field.isHidden) {
      if (field.isRequired) {
        validators.push(Validators.required);
      }

      if (field.maxLength != null) {
        validators.push(Validators.maxLength(field.maxLength));
      }
    }
    return validators;
  }
}
