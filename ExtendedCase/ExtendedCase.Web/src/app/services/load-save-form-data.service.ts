import { of, Observable } from 'rxjs';
import { mergeMap } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { IKeyedCollection } from '../shared/keyed-collection';
import { FormDataService } from './data/form-data.service';
import { LogService } from './log.service';
import { FormDataModel, FormDataSaveModel, FormDataSaveResult, FormFieldValueModel, FieldValueModel,
    FieldProperties } from '../models/form-data.model';
import { FormStateModel, FormStateItem, FormStateKeys } from '../models/form-state.model';

@Injectable()
export class LoadSaveFormDataService {

    constructor(
        private formDataService: FormDataService,
        private logService: LogService) {
    }

    loadFormData(extendedCaseId: string, helpdeskCaseId: number, authToken: string): Observable<FormDataModel> {
        let authTokenLogValue = authToken && authToken.length > 0 ? authToken.slice(0,5) : 'empty';
        this.logService.debugFormatted('loadSavedData: loading form saved data. extendedCaseGuid: {0}, helpdeskCaseId:{1}, Authtoken: {2}.', extendedCaseId, helpdeskCaseId, authTokenLogValue);

        return this.formDataService.getFormDataById(extendedCaseId, helpdeskCaseId, authToken).pipe(
            mergeMap(x => {
                let formDataModel: FormDataModel = this.createFormDataModel(x);
                return of(formDataModel);
            }));
    }

    saveFormData(
        formData: FormDataModel,
        helpdeskCaseId: number,
        authToken: string): Observable<FormDataSaveResult>{

        let saveData: FormDataSaveModel = {
            UniqueId: formData.ExtendedCaseGuid,
            HelpdeskCaseId: helpdeskCaseId,
            FormId: formData.ExtendedCaseFormId,
            FieldsValues: this.mapToFormFieldModel(formData.ExtendedCaseFieldsValues),
            CaseFieldsValues: this.mapToFormFieldModel(formData.CaseFieldsValues),
            FormState: formData.formState && formData.formState.Items ? formData.formState.Items : []
        };

        let authTokenLogValue = authToken && authToken.length > 0 ? authToken.slice(5) : 'empty';

        this.logService.debugFormatted('saveFormData: saving form data. UniqueId: {0}, formId: {1}, Authtoken: {2}.',
            formData.ExtendedCaseGuid, formData.ExtendedCaseFormId, authTokenLogValue);

        return this.formDataService.saveFormData(saveData, authToken);
    }

    private createFormDataModel(data: any): FormDataModel {
        let formData: FormDataModel = new FormDataModel();

        if (data.Id !== undefined) {
            let id = parseInt(data.Id);
            if (!isNaN(id)) {
                formData.Id = id;
            }
        }

        formData.ExtendedCaseGuid = data.ExtendedCaseGuid || '';
        formData.ExtendedCaseFormId = data.ExtendedCaseFormId || 0;

        if (data.Data !== undefined) {

            if (data.Data.ExtendedCaseFieldsValues) {
                (<Array<any>>data.Data.ExtendedCaseFieldsValues)
                    .forEach((item: FormFieldValueModel) => formData.ExtendedCaseFieldsValues.add(item.FieldId, this.createFieldValueModel(item)));
            }

            if (data.Data.CaseFieldsValues) {
                (<Array<any>>data.Data.CaseFieldsValues)
                    .forEach((item: FormFieldValueModel) => formData.CaseFieldsValues.add(item.FieldId, this.createFieldValueModel(item)));
            }
        }

        // init form state
        if (data.FormState && data.FormState.length) {
            let formStateItems = data.FormState.map((el: any) => this.mapToFormStateItem(el));
            formData.formState = new FormStateModel(formStateItems);
        } else {
            formData.formState = new FormStateModel();
        }

        // test data only
        //formData.formState = new FormStateModel([ new FormStateItem('tab1', 'sec1', 0, FormStateKeys.enableStateSelection, 'false') ]);

        return formData;
    }

    private mapToFormStateItem(data: any) : FormStateItem {
        return new FormStateItem(data.TabId, data.SectionId, data.SectionIndex, data.Key, data.Value);
    }

    private createFieldValueModel(data: any) {
        let fieldProperties = <FieldProperties>data.Properties || new FieldProperties();
        return new FieldValueModel(data.Value, data.SecondaryValue, fieldProperties.Pristine);
    }

    private mapToFormFieldModel(fieldValues: IKeyedCollection<FieldValueModel>) : FormFieldValueModel[] {

        return fieldValues.getKeys().map(fieldId => {
            let fieldValue = fieldValues.getItem(fieldId);
            let fieldProperties = new FieldProperties(fieldValue.Pristine);
            return new FormFieldValueModel(fieldId, fieldValue.Value, fieldValue.SecondaryValue, fieldProperties);
        });
    }
}

