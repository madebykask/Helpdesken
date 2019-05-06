import { Validators } from '@angular/forms';
import { ICaseField, FieldModel, IFieldBase } from 'src/app/modules/case-edit-module/models';
import { CaseFormGroup } from './case-form-group';
import { CaseFormControl } from '.';
import { CaseFieldsNames } from '../../constants';
import { TranslateService } from '@ngx-translate/core';

export class CaseFormGroupBuilder {

  constructor(private ngxTranslateService: TranslateService) {
  }

  createFormGroup(fields: ICaseField<any>[], canSave: boolean): CaseFormGroup {
    const controls: { [key: string]: CaseFormControl; } = {};

    //create controls from webapi response
    for (const field of fields) {
      controls[field.name] = this.createCaseFormControl(field.value, field, canSave);
    }

    // internal log TO:
    controls[CaseFieldsNames.Log_InternalEmailsTo] =
      this.createCaseFormControl('', Object.assign(new FieldModel(), {
        name: CaseFieldsNames.Log_InternalEmailsTo,
        label: this.ngxTranslateService.instant('Till'),
        options: []
      }), canSave);

    // internal log CC:
    controls[CaseFieldsNames.Log_InternalEmailsCC] =
      this.createCaseFormControl('', Object.assign(new FieldModel(), {
        name: CaseFieldsNames.Log_InternalEmailsCC,
        label: this.ngxTranslateService.instant('Kopia'),
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
