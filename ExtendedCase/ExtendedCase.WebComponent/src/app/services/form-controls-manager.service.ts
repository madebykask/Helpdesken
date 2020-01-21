import { throwError, forkJoin, of, Observable } from 'rxjs';

import {catchError, mergeMap, map, take} from 'rxjs/operators';
import { Injectable, Inject, forwardRef } from '@angular/core';
import { ProxyModel, ProxyControl } from '../models/proxy.model';

import * as moment from 'moment';
import { IMap, ChangedFieldItem } from '../shared/common-types';
import { KeyedCollection } from '../shared/keyed-collection';

import {
    FormTemplateModel, SectionTemplateModel, BaseControlTemplateModel,
    CustomQueryDataSourceTemplateModel, OptionsDataSourceTemplateModel,
    ControlCustomDataSourceTemplateModel, ControlSectionDataSourceTemplateModel,
    DisabledStateAction, DisabledStateActionCondition, CaseBindingBehaviour
} from '../models/template.model';

import { FormDataModel, FieldValueModel } from '../models/form-data.model';
import { DigestUpdateLog } from '../models/digest.model';

import {
    FormModel,TabModel, SectionModel, SectionInstanceModel, SingleControlFieldModel,
     MultiValueSingleControlFieldModel, MultiControlFieldModel, FieldModelBase,
    ItemModel, CustomDataSourceModel, FormControlType
} from '../models/form.model';

import { FormFieldPathModel } from '../models/form-field-path.model';
import { DataSourcesLoaderService } from './datasources-loader.service';
import { FormModelService } from './form-model.service';
import { TemplateService } from './template.service';
import * as commonMethods from '../utils/common-methods';
import { LogService } from './log.service';
import { ComponentCommService, ControlDataSourceChangeParams } from '../services/component-comm.service';
import { AppConfig } from '../shared/app-config/app-config';
import { ErrorHandlingService } from './error-handling.service';

import { Form, Tab, Section, SectionInstance, Field } from '../models/form-public.model'
import { IAppConfig } from '../shared/app-config/app-config.interface';

@Injectable()
export class FormControlsManagerService {

    private lastSyncedFields: IMap<FieldValueModel> = {};
    constructor(
        private dataSourcesLoaderService: DataSourcesLoaderService,
        private formModelService: FormModelService,
        private templateService: TemplateService,
        private errorHandlingService: ErrorHandlingService,
        private logService: LogService,
        private componentCommService: ComponentCommService,
        @Inject(forwardRef(() => AppConfig)) private config: IAppConfig) {
    }

    getFormData(formModel: FormModel): Form {

        let form = new Form();

        let lastTab: Tab = null;
        let lastSection: Section = null;
        let lastSectionInstance: SectionInstance = null;

        let fieldsIterator = formModel.createFieldsIterator();
        fieldsIterator.forEach(
            (fieldModel, fieldPathModel) => {

                if (fieldModel.hidden) {
                    return;
                }

                let proxyModel = formModel.proxyModel;
                let controlTemplate = fieldModel.template;

                let fieldPath = fieldPathModel.buildFormFieldPath();
                let proxyControl = proxyModel.findProxyControl(fieldPathModel);
                let fieldValue = this.getControlValueForSave(fieldPath, fieldModel, proxyControl, controlTemplate);

                let fi = new Field();
                fi.label = controlTemplate.label;
                fi.caseBinding = controlTemplate.caseBinding;
                fi.value = fieldValue.Value;
                fi.secondaryValue = fieldValue.SecondaryValue;

                // add to collection
                lastSectionInstance.fields.push(fi);

            },

            (sectionInstance: SectionInstanceModel, sectionIndex: number) => {

                if (sectionInstance.section.hidden) {
                    return;
                }

                if (sectionIndex === 0) {
                    let s = new Section();
                    s.name = sectionInstance.section.template.name;
                    lastSection = s;
                    lastTab.sections.push(s);
                }

                let si = new SectionInstance();
                si.index = sectionIndex;
                lastSection.instances.push(si);
                lastSectionInstance = si;
            },

            (tab: TabModel) => {
                if (tab.hidden) {
                    return;
                }

                let t = new Tab();
                t.name = tab.template.name;
                form.tabs.push(t);
                lastTab = t;
            }
        );

        return form;
    }

    getFormFieldValues(formModel: FormModel, checkCaseBindingBehaviour = false):
      { fieldsValues: IMap<FieldValueModel>, caseFieldsValues: IMap<FieldValueModel> } {
        let fieldsValues: IMap<FieldValueModel> = {};
        let caseFieldsValues: IMap<FieldValueModel> = {};

        let proxyModel = formModel.proxyModel;

        let fieldsIterator = formModel.createFieldsIterator();
        fieldsIterator.forEach((fieldModel, fieldPathModel) => {

            let controlTemplate = fieldModel.template;

            // do not save review values
            if (controlTemplate.shouldNotSave) {
                return;
            }

            let fieldPath = fieldPathModel.buildFormFieldPath();
            const proxyControl = proxyModel.findProxyControl(fieldPathModel);

            let fieldValueModel = this.getControlValueForSave(fieldPath, fieldModel, proxyControl, controlTemplate);
            fieldValueModel.Pristine = proxyControl.pristine;

            if (controlTemplate.caseBinding && controlTemplate.caseBinding.length > 0 ) {
              if (!checkCaseBindingBehaviour || (checkCaseBindingBehaviour &&
                        (controlTemplate.caseBindingBehaviour === CaseBindingBehaviour.Overwrite ||
                         (controlTemplate.caseBindingBehaviour === CaseBindingBehaviour.NewOnly &&
                             (this.lastSyncedFields[controlTemplate.caseBinding] &&
                               this.lastSyncedFields[controlTemplate.caseBinding].Value !== fieldValueModel.Value))))) {
                  caseFieldsValues[controlTemplate.caseBinding] = fieldValueModel;
                }
                this.lastSyncedFields[controlTemplate.caseBinding] = fieldValueModel;

                // do not save values for caseBinding fields - only pristine flag.
                fieldsValues[fieldPath] = new FieldValueModel('', '', proxyControl.pristine);
            } else {
                fieldsValues[fieldPath] = fieldValueModel;
            }
        });

        return { fieldsValues, caseFieldsValues };
    }

    private getControlValueForSave(path: string,
        fieldModel: FieldModelBase,
        proxyControl: ProxyControl,
        controlTemplate: BaseControlTemplateModel): FieldValueModel {
        // add handling for other multi values controls should new added
        if (fieldModel instanceof MultiControlFieldModel) {
            return this.getMultiControlValueForSave(fieldModel, proxyControl);
        }

        // serialize array for multiselect controls
        if (fieldModel instanceof MultiValueSingleControlFieldModel) {
            return this.getMultiSelectValueForSave(fieldModel, proxyControl);
        }

        let value = proxyControl.value || '';
        let secondaryValue = proxyControl.secondaryValue;

        // convert date to dbDateFormat format
        // todo: refactor - move to separate class/method
        if (controlTemplate.controlType === FormControlType.Date && value.length) {
            let momentDate = moment(value, fieldModel.template.mode === 'year' ? this.config.yearFormat : this.config.dateFormat);
            if (!momentDate.isValid()) {
                this.errorHandlingService.handleUserError(`Unknown date format recieved from UI: ${value}, expecting ${this.config.dateFormat}`);
            }
            value = momentDate.format(this.config.dbDateFormat);
        } else if (value.length && (controlTemplate.controlType === FormControlType.Amount ||
            controlTemplate.controlType === FormControlType.Percentage)) {
            let regex = new RegExp(`[${this.config.decimalSeparator}]+`, 'g');
            value = value.replace(regex, this.config.dbDecimalSeparator);
        } else if (controlTemplate.controlType === FormControlType.Search) {
            return new FieldValueModel(value, secondaryValue);
        }

        // convert array to coma-separated string
        if (commonMethods.isArray(value)) {
            value = value.join(',');
        }

        let selectedItem = fieldModel.items ? fieldModel.items.find((x: ItemModel) => x.value === proxyControl.value) : null;
        return new FieldValueModel(value, selectedItem ? selectedItem.text : null);
    }

    getMultiControlValueForSave(controlField: FieldModelBase, proxyControl: ProxyControl): FieldValueModel {
        // "Value": { "val1": "true", "val2": "true", "val3": true }
        let values: string[] = [];

        for (let prop of Object.keys(proxyControl.value)) {
            let val = proxyControl.value[prop];
            val = commonMethods.anyToBoolean(val);
            if (val === true) {
                values.push(prop);
            }
        }

        let selectedTexts: string[] = proxyControl.secondaryValue || [];

        return new FieldValueModel(values.join(','), selectedTexts.length > 0 ? selectedTexts.join(',') : null);
    }

    private getMultiSelectValueForSave(controlField: FieldModelBase, proxyControl: ProxyControl): FieldValueModel {

        let value = proxyControl.value || '';
        if (commonMethods.isArray(value)) {
            value = value.join(',');
        }

        let selectedTexts: string[] = proxyControl.secondaryValue || [];
        let selectedTextValue = selectedTexts.length > 0 ? selectedTexts.join(',') : null;
        return new FieldValueModel(value, selectedTextValue);
    }

    updateCaseFieldValues(caseValues: IMap<FieldValueModel>, formModel: FormModel): ChangedFieldItem[] {
        let changedItems: ChangedFieldItem[] = [];
        let caseFields = this.findCaseFields(formModel);

        for (let caseFieldKey of Object.keys(caseValues)) {
            let caseFieldValue = caseValues[caseFieldKey];

            let fieldModels = caseFields.filter((fm: FieldModelBase) => fm.template.caseBinding === caseFieldKey);
            if (fieldModels && fieldModels.length) {
                for (let fieldModel of fieldModels) {
                    this.setFormControlValue(fieldModel, fieldModel.template, caseFieldValue);

                    let item = new ChangedFieldItem(fieldModel.previousValue, fieldModel.lastValue, fieldModel.getFieldPath());
                    changedItems.push(item);
                }
            }
        }

        return changedItems;
    }

    private findCaseFields(formModel: FormModel): FieldModelBase[] {
        let fields: FieldModelBase[] = [];
        let iterator = formModel.createFieldsIterator();
        iterator.forEach((field, fieldPath) => {
            let template = field.template;
            if (template.caseBinding && template.caseBinding.length) {
                fields.push(field);
            }
        });
        return fields;
    }

    setFormData(formData: FormDataModel, formModel: FormModel) {
        let res = this.prepareFormFieldsValues(formData, formModel);
        let exCaseFieldsValuesMap = res.exCaseFieldsMap;
        let caseBindingFieldsMap = res.caseBindingFieldsMap;

        // let valuesMapping: IExtendedCaseValuesFieldPathMap[] = this.createFieldValuesArrayWithFieldPath(formData.ExtendedCaseFieldsValues);

        // tabs
        for (let tabId of Object.keys(formModel.tabs)) {
            let tabModel = formModel.tabs[tabId];

            // sections
            for (let sectionId of Object.keys(tabModel.sections)) {
                let sectionModel = tabModel.sections[sectionId];
                let sectionTpl = sectionModel.template;

                let sectionsInstancesMaxIndex = 0;
                let sectionValues = exCaseFieldsValuesMap
                    .filter((el: IExtendedCaseValuesFieldPathMap) =>
                        el.fieldPath.tabId === tabId && el.fieldPath.sectionId === sectionId);

                // calculate section instances required number based on fields path
                sectionValues.forEach((el: IExtendedCaseValuesFieldPathMap) =>
                    sectionsInstancesMaxIndex =
                        el.fieldPath.sectionInstanceIndex > sectionsInstancesMaxIndex ?
                        el.fieldPath.sectionInstanceIndex :
                        sectionsInstancesMaxIndex);

                if (sectionsInstancesMaxIndex > 0) {
                    this.createSectionModelInstances(sectionsInstancesMaxIndex + 1, sectionTpl, sectionModel, formModel.proxyModel);
                }

                let sectionInstanceIndex = -1;

                // section instances
                for (let sectionInstance of sectionModel.instances) {

                    sectionInstanceIndex++;

                    // controls
                    for (let fieldIndex of Object.keys(sectionInstance.fields)) {

                        let fieldModel = sectionInstance.fields[fieldIndex];
                        let controlTemplate = fieldModel.template;

                        let fieldPath = new FormFieldPathModel(tabModel.id, sectionModel.id, sectionInstanceIndex, fieldModel.id);
                        let caseBinding = controlTemplate.caseBinding;

                        let fieldsValuesSource =
                            (caseBinding && caseBinding.length > 0) ? caseBindingFieldsMap : sectionValues;

                        let fieldValueItem = fieldsValuesSource.find((el: IExtendedCaseValuesFieldPathMap) => el.fieldPath.equals(fieldPath));
                        let fieldValue = !commonMethods.isUndefinedOrNull(fieldValueItem) ? fieldValueItem.fieldValue : undefined;

                        if (fieldValue) {
                            this.setFormControlValue(fieldModel, controlTemplate, fieldValue);
                            fieldModel.acceptChanges(); // set original values

                            // setting correct pristine value is required for calculated fields that have been changed manually to keep this value.
                            fieldModel.setPristine(fieldValue.Pristine);
                        }
                    }
                }
            }
        }
    }

    private prepareFormFieldsValues(formData: FormDataModel, formModel: FormModel): any {

        let exCaseFieldsMap = this.getExtendedCaseFieldsWithFieldPath(formData, formModel);
        let caseBindingFieldsMap = this.getCaseBindingFieldsWithFieldPath(formData, formModel);

        return { exCaseFieldsMap, caseBindingFieldsMap };
    }

    private getExtendedCaseFieldsWithFieldPath(formData: FormDataModel, formModel: FormModel): IExtendedCaseValuesFieldPathMap[] {

        let exCaseFieldsValues = formData.ExtendedCaseFieldsValues;
        let items: IExtendedCaseValuesFieldPathMap[] = [];

        if (exCaseFieldsValues) {

            // get caseBinding fields
            let caseBindingFields = new KeyedCollection<BaseControlTemplateModel>();
            formModel.createFieldsIterator()
                .forEach((formField: FieldModelBase, fieldPath: FormFieldPathModel) => {
                    let fieldTemplate = formField.template;
                    if (fieldTemplate.caseBinding && fieldTemplate.caseBinding.length) {
                        caseBindingFields.add(fieldPath.buildFormFieldPath(), fieldTemplate);
                    }
                });

            for (let fieldKey of exCaseFieldsValues.getKeys()) {
                // selected only non-caseBinding fields
                if (!caseBindingFields.containsKey(fieldKey)) {
                    items.push({
                        fieldPath: FormFieldPathModel.parse(fieldKey),
                        fieldValue: exCaseFieldsValues.getItemSafe(fieldKey)
                    });
                }
            }
        }
        return items;
    }

    private getCaseBindingFieldsWithFieldPath(formData: FormDataModel, formModel: FormModel): IExtendedCaseValuesFieldPathMap[] {

        let exCaseFieldsValues = formData.ExtendedCaseFieldsValues;
        let caseBindingFieldsValues = formData.CaseFieldsValues;

        let items: IExtendedCaseValuesFieldPathMap[] = [];
        if (caseBindingFieldsValues) {

            let caseBindingFieldsPathMap = new KeyedCollection<FormFieldPathModel>();
            formModel.createFieldsIterator()
                .forEach((formField: FieldModelBase, fieldPath: FormFieldPathModel) => {
                    let fieldTemplate = formField.template;
                    if (fieldTemplate.caseBinding && fieldTemplate.caseBinding.length) {
                        caseBindingFieldsPathMap.add(fieldTemplate.caseBinding, fieldPath);
                    }
                });

            for (let caseBindingKey of caseBindingFieldsValues.getKeys()) {

                // select only non-caseBinding fields
                let caseBindingFieldPath = caseBindingFieldsPathMap.getItemSafe(caseBindingKey);

                if (caseBindingFieldPath) {
                    // override pristine value from db field values
                    let path = caseBindingFieldPath.buildFormFieldPath() || '';
                    let fieldValueModel = caseBindingFieldsValues.getItem(caseBindingKey);
                    let pristine = exCaseFieldsValues.containsKey(path)
                        ? exCaseFieldsValues.getItem(path).Pristine
                        : !commonMethods.isUndefinedNullOrEmpty(fieldValueModel.Value);

                    items.push({
                        fieldPath: caseBindingFieldPath,
                        fieldValue: new FieldValueModel(fieldValueModel.Value, fieldValueModel.SecondaryValue, pristine)
                    });
                }
            }
        }

        return items;
    }

    getSectionFieldsChangeSet(sectionInstance: SectionInstanceModel): ChangedFieldItem[]  {
        let changedItems: ChangedFieldItem[] = [];
        for (let fieldId of Object.keys(sectionInstance.fields)) {

            let fieldModel = sectionInstance.fields[fieldId];
            let fieldPath = fieldModel.getFieldPath();

            let item = new ChangedFieldItem(fieldModel.previousValue, fieldModel.lastValue, fieldPath);
            changedItems.push(item);
        }
        return changedItems;
    }

    private createSectionModelInstances(sectionInstancesCount: number, sectionTpl: SectionTemplateModel,
       sectionModel: SectionModel, proxyModel: ProxyModel): void {
        while (sectionModel.instances.length < sectionInstancesCount) {
            try {
                this.formModelService.addSectionInstance(sectionTpl, sectionModel, proxyModel);
            } catch (e) {
                this.errorHandlingService.handleError(e, `Failed to create section model instance (id=${sectionModel.id})`);
            }
        }
    }



    private setFormControlValue(field:FieldModelBase, controlTemplate:BaseControlTemplateModel, fieldValue: FieldValueModel) {
        this.logService.debugFormatted('setFormControlValue: {0}', field.id);

        // todo: handle caseBinding property from the metaData to read values from caseFieldValues collection!

        if (field instanceof SingleControlFieldModel) {
            this.setSingleControlValue(field, fieldValue, controlTemplate);
        } else if (field instanceof MultiControlFieldModel) {
            this.setMultiControlValue(field, fieldValue, controlTemplate);
        } else {
            this.logService.warningFormatted('setFormControlValue: Not supported field type. Id= {0}', field.id);
        }
    }

    private setSingleControlValue(fieldModel: SingleControlFieldModel,
       fieldValue: FieldValueModel, controlTemplate: BaseControlTemplateModel) {
        let value = fieldValue.Value || '';
        let secondaryValue = fieldValue.SecondaryValue || '';

        if (controlTemplate.controlType === FormControlType.Date && value.length) {
            let momentDate = moment(value, this.config.dbDateFormat);
            if (!momentDate.isValid()) {
                this.errorHandlingService.handleUserError(`Unknown date format recieved from template: ${value}, expecting ${this.config.dbDateFormat}`);
            }
            value = momentDate.format(controlTemplate.mode === 'year' ? this.config.yearFormat : this.config.dateFormat); // convert from dbDateFormat to ui dateFormat
        } else if ((controlTemplate.controlType === FormControlType.Amount ||
           controlTemplate.controlType === FormControlType.Percentage) && value.length) {
            value = value.replace(new RegExp(`[${this.config.dbDecimalSeparator}]+`, 'g'), this.config.decimalSeparator);
        }

        let controlValue: any;
        let additionalData = '';

        if (fieldModel instanceof MultiValueSingleControlFieldModel) {
            controlValue = value.split(',');
        } else if (controlTemplate.controlType === FormControlType.Search) {
            controlValue = value;
            additionalData = secondaryValue; // id
        } else {
             controlValue = value;
        }

        this.formModelService.setFieldValue(fieldModel, controlValue, additionalData);
    }

    private setMultiControlValue(fieldModel: MultiControlFieldModel,
       fieldValue: FieldValueModel, controlTemplate: BaseControlTemplateModel) {
        let value = fieldValue.Value || '';
        if (controlTemplate.controlType === FormControlType.CheckboxList) {
            this.setCheckBoxListValues(fieldModel, value);
        } else {
            this.errorHandlingService.handleUserError(`Not supported multi-control type. ControlType: ${controlTemplate.controlType}, FieldId: ${fieldModel.id}`);
        }
    }

    private setCheckBoxListValues(fieldModel: MultiControlFieldModel, value: string) {
        let values: string[] = [];
        if (!commonMethods.isUndefinedNullOrEmpty(value)) {
            values = value.split(',');
        }

        let checkBoxValues = fieldModel.getMultiControlValues();
        for (let chkBoxKey of Object.keys(checkBoxValues)) {
            checkBoxValues[chkBoxKey] = values.indexOf(chkBoxKey) !== -1;
        }

        this.formModelService.setFieldValue(fieldModel, checkBoxValues);
    }

    populateSectionWithValues(section: SectionInstanceModel, values: IMap<string>): ChangedFieldItem[] {
        let changedItems: ChangedFieldItem[] = [];

        if (values) {
            for (let fieldId of Object.keys(values)) {
                let newValue = values[fieldId];
                let fieldModel = section.fields[fieldId];
                this.formModelService.setFieldValue(fieldModel, newValue);

                // use fieldModel.previousValue, fieldModel.lastValue since they are set by setFieldValue method call correctly
                let item = new ChangedFieldItem(fieldModel.previousValue, fieldModel.lastValue, fieldModel.getFieldPath());
                changedItems.push(item);
            }
        }

        return changedItems;
    }

    processFieldWarnings(fieldPath: FormFieldPathModel, formModel: FormModel, digestUpdateLog: DigestUpdateLog): void {
        // reset warnings
        const fieldModel = formModel.findFormField(fieldPath);
        fieldModel.warnings = new Array<string>();

        // process warnings
        const templateCtrl = fieldModel.template;
        if (typeof templateCtrl.warningBinding === 'function') {
            let proxyControl = formModel.proxyModel.findProxyControl(fieldPath);
            const msg = templateCtrl.warningBinding.call(proxyControl, formModel.proxyModel, digestUpdateLog);
            if (msg) {
                fieldModel.warnings.push(msg);
            }
        }
    }

    loadFormCustomDataSources(templateModel: FormTemplateModel, formModel: FormModel): Observable<Array<boolean>> {
        let dsObservation: Observable<boolean>[] = [];

        for (let dsTemplate of templateModel.dataSources) {
            if (dsTemplate instanceof CustomQueryDataSourceTemplateModel) {
                this.logService.infoFormatted('processCustomDataSources for dsTemplateId = {0}', dsTemplate.id);
                let result$ =
                    this.dataSourcesLoaderService.loadCustomQueryDataSourceData(formModel.proxyModel, dsTemplate.id, dsTemplate.parameters).pipe(
                        take(1),
                        map((data: any) => {
                            formModel.dataSources[dsTemplate.id].setData(data);
                            this.logService.infoFormatted('Custom data source ({0}) has been loaded successfully.', dsTemplate.id);
                            return true;
                    }));
                dsObservation.push(result$);
            }
        }

        // return at least one to proceed with other processings... false means there were no changes
        if (dsObservation.length === 0) {
            return of([false]);
        }

        // wait for all observables to complete
        let res = forkJoin(dsObservation);
        return res;
    }

    processDataSourcesForControl(
        formModel: FormModel,
        controlTemplateModel: BaseControlTemplateModel,
        fieldModel: FieldModelBase): Observable<boolean> {

        // t0d0: check if its correct usage
        let digestUpdateLog = new DigestUpdateLog();

        let dataSourceTemplate = controlTemplateModel.dataSource;
        let refreshComplete$: Observable<boolean> = null;

        // process custom data source for this control only if exists
        if (dataSourceTemplate && dataSourceTemplate instanceof OptionsDataSourceTemplateModel) {
            // this.logService.info(`options::processControlsDataSources: forceLoadDataSources:
            // ${this.forceLoadDataSources}, paramChanged - ${requiresUpdate}, controlTpl- ${control.id}, dataSourceTemplate - ${dataSourceTemplate.id}`);
            refreshComplete$ = this.refreshOptionsDataSource(formModel, dataSourceTemplate, fieldModel, controlTemplateModel, digestUpdateLog);
        } else if (dataSourceTemplate && dataSourceTemplate instanceof ControlCustomDataSourceTemplateModel) { // process data source for this control only if exists
            // this.logService.info(`custom:processControlsDataSources:forceLoadDataSources -
            // ${this.forceLoadDataSources}, paramChanged - ${requiresUpdate}, controlTpl- ${control.id}, dataSourceTemplate - ${dataSourceTemplate.id}`);
            refreshComplete$ = this.refreshControlCustomDataSource(formModel,
                dataSourceTemplate,
                fieldModel,
                controlTemplateModel,
                digestUpdateLog);
        } else if (dataSourceTemplate && dataSourceTemplate instanceof ControlSectionDataSourceTemplateModel) {

            let sectionInstance = fieldModel.sectionInstance;

            refreshComplete$ = this.refreshControlCustomSectionDataSource(
                sectionInstance,
                formModel,
                dataSourceTemplate,
                fieldModel,
                controlTemplateModel,
                digestUpdateLog);
        } else {
            refreshComplete$ = of(this.setControlDataSourceOptions(formModel.proxyModel,
                fieldModel,
                controlTemplateModel,
                digestUpdateLog));
        }

        if (!refreshComplete$) {
            return of(false);
        }
        return refreshComplete$;
    }

    refreshControlCustomDataSource(
        formModel:FormModel,
        dataSourceTemplate: ControlCustomDataSourceTemplateModel,
        fieldModel: FieldModelBase,
        controlTemplateModel: BaseControlTemplateModel,
        digestUpdateLog: DigestUpdateLog): Observable<boolean> {

        this.logService.infoFormatted('refreshControlCustomDataSource: loading data from {0}', dataSourceTemplate.id);
        return this.refreshControlCustomDataSourceInner(
            formModel,
            formModel.proxyModel,
            dataSourceTemplate,
            fieldModel,
            controlTemplateModel,
            digestUpdateLog);
    }

    refreshControlCustomSectionDataSource(
        sectionInstance: SectionInstanceModel,
        formModel:FormModel,
        dataSourceTemplate: ControlCustomDataSourceTemplateModel | ControlSectionDataSourceTemplateModel,
        fieldModel: FieldModelBase,
        controlTemplateModel: BaseControlTemplateModel,
        digestUpdateLog: DigestUpdateLog): Observable<boolean> {

        this.logService.infoFormatted('refreshControlCustomSectionDataSource: loading data from {0}', dataSourceTemplate.id);
        return this.refreshControlCustomDataSourceInner(
            sectionInstance,
            formModel.proxyModel,
            dataSourceTemplate,
            fieldModel,
            controlTemplateModel,
            digestUpdateLog);
    }

    private refreshControlCustomDataSourceInner(
        customDataSourcesOwner: any,
        proxyModel: ProxyModel,
        dsTemplate: ControlCustomDataSourceTemplateModel | ControlSectionDataSourceTemplateModel,
        fieldModel: FieldModelBase,
        controlTemplateModel: BaseControlTemplateModel,
        digestUpdateLog: DigestUpdateLog): Observable<boolean> {

        let dsModel: CustomDataSourceModel = customDataSourcesOwner.dataSources[dsTemplate.id];
        if (dsModel) {
            // map data to ItemModel
            let data = dsModel.getData().slice();
            let options: ItemModel[] = [];
            data.map((el: any) => {
                if (el.hasOwnProperty(dsTemplate.valueField) && el.hasOwnProperty(dsTemplate.textField)) {
                    let valueField: string = el[dsTemplate.valueField];
                    let textField: string = el[dsTemplate.textField];
                    options.push(new ItemModel(valueField, textField));
                } else {
                    this.logService.warningFormatted('warning: custom data source ({0}) data item doesn\'t have specified properties: {1} | {2})',
                        dsTemplate.id, dsTemplate.valueField, dsTemplate.textField);
                }
            });

            // set control model data
            if (options.length > 0) {
                fieldModel.preFilteredItems = options;
                let isChanged = this.setControlDataSourceOptions(proxyModel, fieldModel, controlTemplateModel, digestUpdateLog);
                this.logService.infoFormatted('refreshControlCustomDataSource: loaded. control {0} value changed - {1}', controlTemplateModel.id, isChanged);
                return of(isChanged);
            }
        }

        return of(false);
    }

    refreshOptionsDataSource(
        formModel: FormModel,
        dataSourceTemplate: OptionsDataSourceTemplateModel,
        fieldModel: FieldModelBase,
        controlTemplateModel: BaseControlTemplateModel,
        digestUpdateLog: DigestUpdateLog): Observable<boolean> {

        this.logService.infoFormatted('refreshOptionsDataSource: start. ds:{0}', dataSourceTemplate.id);

        let fieldPathModel = fieldModel.getFieldPath();
        let dsParameters = this.templateService
            .expandParametersForSection(fieldPathModel.sectionInstanceIndex, dataSourceTemplate.parameters);

        return this.dataSourcesLoaderService.loadOptionsDataSourceData(formModel.proxyModel, dataSourceTemplate.id, dsParameters).pipe(
            catchError((e: any) => {
                let errMsg = 'Failed to load options dataSource: ' + dataSourceTemplate.id;
                this.errorHandlingService.handleError(e, errMsg);
                return throwError({ id: dataSourceTemplate.id });
            }),
            mergeMap((data: any) => {
                fieldModel.preFilteredItems = data.map((dataItem: any) => new ItemModel(dataItem.Value, dataItem.Text));
                this.logService.infoFormatted('refreshOptionsDataSource: success! options data loaded from {0}.', dataSourceTemplate.id);

                let isChanged = this.setControlDataSourceOptions(formModel.proxyModel, fieldModel, controlTemplateModel, digestUpdateLog);
                this.logService.infoFormatted('refreshOptionsDataSource: control {0} value changed - {1}', controlTemplateModel.id, isChanged);
                return of(isChanged);
            }));
    }

    setControlDataSourceOptions(proxyModel: ProxyModel, fieldModel: FieldModelBase,
       control: BaseControlTemplateModel, digestUpdateLog: DigestUpdateLog): boolean {
        this.logService.debugFormatted('setControlDataSourceOptions: set new values into control {0}', control.id);
        let filteredItems = this.filterItems(proxyModel, fieldModel, control, digestUpdateLog);

        if (control.emptyElement && control.emptyElement.length) {
            filteredItems.unshift(ItemModel.createEmptyElement(control.emptyElement));
        }

        return this.setFieldItems(control, fieldModel, filteredItems);
    }

    private filterItems(proxyModel: ProxyModel, fieldModel: FieldModelBase,
       controlTemplateModel: BaseControlTemplateModel, digestUpdateLog: DigestUpdateLog): ItemModel[] {
        if (controlTemplateModel.dataSourceFilterBinding instanceof Function) {
            this.logService.debugFormatted('dataSourceFilterBinding called for {0}', controlTemplateModel.id);

            let proxyControl = proxyModel.findProxyControl(fieldModel.getFieldPath());
            let items = fieldModel.preFilteredItems ? fieldModel.preFilteredItems.slice() : [];
            let mappedDataSource =
                controlTemplateModel.dataSourceFilterBinding.call(proxyControl, proxyModel, digestUpdateLog, items);

            if (mappedDataSource && mappedDataSource instanceof Array) {
                let newItems =
                    mappedDataSource.map((dataSourceElement: ItemModel) =>
                        new ItemModel(dataSourceElement.value, dataSourceElement.text));

                return newItems;
            }
        }
        return fieldModel.preFilteredItems;
    }

    private setFieldItems(control: BaseControlTemplateModel, fieldModel: FieldModelBase, items: ItemModel[]): boolean {

        if (commonMethods.areItemModelArraysEqual(fieldModel.items, items)) {
            this.componentCommService.announceControlDataSourceChange(new ControlDataSourceChangeParams(fieldModel.id, items, false));
            return false;
        }

        this.logService.debugFormatted('[ITEMS] setFieldItems: set new option items. TemplateControl id = {0}', fieldModel.id);

        fieldModel.items = items;
        this.componentCommService.announceControlDataSourceChange(new ControlDataSourceChangeParams(fieldModel.id, items, true));

        if (fieldModel instanceof MultiControlFieldModel) {
            this.formModelService.createControlsForMultiControl(fieldModel, control);
            return true;
        } else if (fieldModel instanceof SingleControlFieldModel) {
            return this.tryResetSingleValueOnItemsChange(control, fieldModel);
        }
        return undefined;
    }

    private tryResetSingleValueOnItemsChange(controlTemplateModel: BaseControlTemplateModel, fieldModel: SingleControlFieldModel): boolean {

        if (commonMethods.isUndefinedNullOrEmpty(fieldModel.control.value) ||
         controlTemplateModel.controlType === FormControlType.Search) {
            return false;
        }

        if (fieldModel instanceof MultiValueSingleControlFieldModel){
            return this.tryResetSingleArrayValueOnItemsChange(fieldModel, controlTemplateModel.resetValueOnItemsUpdate);
        }

        return this.tryResetSingleSimpleValueOnItemsChange(fieldModel, controlTemplateModel.resetValueOnItemsUpdate);
    }

    private tryResetSingleSimpleValueOnItemsChange(fieldModel: SingleControlFieldModel, forceReset: boolean): boolean {
        let items: ItemModel[] = fieldModel.items;
        let control = fieldModel.control;
        let isInItems = commonMethods.isUndefinedNullOrEmpty(control.value);

        if (!forceReset) {
            isInItems = items.findIndex((x: ItemModel) => {
                return x.value === control.value;
            }) > -1;
        }

        if (isInItems) {
            return false;
        }

        // reset control value since new items don't have control value
        this.formModelService.setFieldValue(fieldModel, '');
        return true;
    }

    private tryResetSingleArrayValueOnItemsChange(fieldModel: SingleControlFieldModel, forceReset: boolean): boolean {
        let items: ItemModel[] = fieldModel.items;
        let control = fieldModel.control;
        let valueArray = control.value as any[];
        let matchingItems: any[] = [];
        var valueChanged = valueArray.length > 0;

        if (!forceReset) {
            valueChanged = false;
            matchingItems = valueArray.filter((x) => {
                let found = items.findIndex((y) => { return y.value === x; }) > -1;
                if (!found) {
                    valueChanged = true;
                }
                return found;
            });
        }

        if (!valueChanged) {
            return false;
        }

        // keep only those selected values that exist in new items
        this.formModelService.setFieldValue(fieldModel, matchingItems);
        return true;
    }

    resetDisabledSectionState(sectionInstance: SectionInstanceModel, isDisabledByUser: boolean) : ChangedFieldItem[] {
        let disabledStateBehavior = sectionInstance.section.template.disabledStateBehavior;
        if (disabledStateBehavior) {

            // ignore if section was not disabled by user when condition is UserOnly
            if (disabledStateBehavior.condition === DisabledStateActionCondition.UserOnly && !isDisabledByUser) {
                return null;
            }

            let action = disabledStateBehavior.action;

            if (action === DisabledStateAction.Clear) {
                return this.resetSectionState(sectionInstance);
            } else if (action === DisabledStateAction.RestorePrev) {
                return this.resetSectionToPrevState(sectionInstance);
            }
        }

        return null;
    }

    private resetSectionState(sectionInstance: SectionInstanceModel) : ChangedFieldItem[] {

        let changedItems:ChangedFieldItem[] = [];

        // controls
        for (let fieldIndex of Object.keys(sectionInstance.fields)) {
            let fieldModel = sectionInstance.fields[fieldIndex];

            this.formModelService.setFieldValue(fieldModel, null, null);

            let item = new ChangedFieldItem(fieldModel.previousValue, fieldModel.lastValue, fieldModel.getFieldPath());
            changedItems.push(item);
        }

        return changedItems;
    }

    private resetSectionToPrevState(sectionInstance: SectionInstanceModel): ChangedFieldItem[] {
        let changedItems: ChangedFieldItem[] = [];

        // controls
        for (let fieldIndex of Object.keys(sectionInstance.fields)) {
            let fieldModel = sectionInstance.fields[fieldIndex];

            // restore to prev values
            let value = fieldModel.originalValue;
            let additionalData = fieldModel.originalAdditionalData;

            this.formModelService.setFieldValue(fieldModel, value, additionalData);

            // add changed item to collection
            let item = new ChangedFieldItem(fieldModel.previousValue, fieldModel.lastValue, fieldModel.getFieldPath());
            changedItems.push(item);
        }

        return changedItems;
    }

    getReviewControlStringValues(fieldModel: SingleControlFieldModel) {
        let value1 = fieldModel.control.value && fieldModel.control.value.length > 0 ? fieldModel.control.value[0] : '';
        let value2 = fieldModel.control.value && fieldModel.control.value.length > 1 ? fieldModel.control.value[1] : '';

        let fixedValue1 = commonMethods.convertAnyToString(value1);
        let fixedValue2 = commonMethods.convertAnyToString(value2);
        return [fixedValue1.trim(), fixedValue2.trim()];
    }
}

interface IExtendedCaseValuesFieldPathMap {
    fieldPath: FormFieldPathModel;
    fieldValue: FieldValueModel;
}



