
import { debounceTime } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { FormArray, FormGroup, FormControl } from '@angular/forms';
import { TabModel, SectionModel, SectionInstanceModel, FieldModelBase,
         FormModel, SingleControlFieldModel, MultiValueSingleControlFieldModel,
         MultiControlFieldModel, ItemModel, CustomDataSourceModel, FormControlType } from '../models/form.model';
import { ControlValueChangedParams, ComponentCommService } from './component-comm.service';
import { FormTemplateModel, TabTemplateModel, BaseControlTemplateModel, DataSourceItemTemplateModel,
         CustomStaticDataSourceTemplateModel, SectionTemplateModel, SectionType } from '../models/template.model';
import { ProxyModelService } from './proxy-model.service';
import { ProxyModel, FormInfo } from '../models/proxy.model';
import { ErrorHandlingService } from './error-handling.service';
import { LogService } from './log.service';
import * as commonMethods from '../utils/common-methods';


@Injectable()
export class FormModelService {

    constructor(private proxyModelService: ProxyModelService,
        private componentCommService: ComponentCommService,
        private errorHandlingService: ErrorHandlingService,
        private logService: LogService) {
    }

    buildForm(templateModel: FormTemplateModel, formInfo: FormInfo): FormModel {
        this.logService.debug('Building form model');

        let tabs: { [id: string]: TabModel; } = {};
        let proxyModel = new ProxyModel(formInfo, templateModel.globalFunctions, templateModel.localization);
        let dataSourceModels: { [id: string]: CustomDataSourceModel } = {};

        let formModel = new FormModel(templateModel);
        formModel.tabs = tabs;
        formModel.proxyModel = proxyModel;
        formModel.dataSources = dataSourceModels;

        templateModel.tabs.forEach((tab: TabTemplateModel) => {
            let tabModel = new TabModel(tab.id, tab);
            let tabGroup: any = {};

            tab.sections.forEach((section: SectionTemplateModel) => {

                // do not create form model for review sections
                if (section.type !== SectionType.review) {
                    let sectionModel = this.createSectionModel(section, tab, tabModel, proxyModel);
                    tabModel.sections[section.id] = sectionModel;
                    tabGroup[section.id] = sectionModel.group;
                }
            });

            tabModel.group = new FormGroup(tabGroup);
            tabs[tab.id] = tabModel;
        });

         // init custom datasource data model
        let dataSourceTemplateModels = templateModel.dataSources || [];
        for (let dsTemplateModel of dataSourceTemplateModels) {
            dataSourceModels[dsTemplateModel.id] = new CustomDataSourceModel(dsTemplateModel.id);
            // set data immedeiately if its a statis data source!
            if (dsTemplateModel instanceof CustomStaticDataSourceTemplateModel) {
                dataSourceModels[dsTemplateModel.id].setData(dsTemplateModel.data);
            }

            // add accessor property to the proxyModel.dataSources
            this.proxyModelService.createDataSourceProperty(
                proxyModel,
                dsTemplateModel.id,
                dataSourceModels[dsTemplateModel.id]);
        }

        this.logService.debug('Form model has been built successfully');
        return formModel;
    }

    private createSectionModel(sectionTpl: SectionTemplateModel, tabTpl: TabTemplateModel,
        tabModel: TabModel, proxyModel: ProxyModel): SectionModel {

        let sectionModel = new SectionModel(sectionTpl.id, sectionTpl, tabModel);
        sectionModel.instances = [];

        // 1. create initial section instance
        let firstSection = this.createSectionInstanceModel(sectionTpl, sectionModel, proxyModel);
        sectionModel.instances.push(firstSection);

        // 2. add first section instance group to form array of the section model form
        let instancesFormArray = new FormArray([firstSection.group]);
        sectionModel.group = new FormGroup({ instances: instancesFormArray });

        return sectionModel;
    }

    addSectionInstance(sectionTpl: SectionTemplateModel, sectionModel: SectionModel, proxyModel: ProxyModel): SectionInstanceModel {
        let sectionInstance = this.createSectionInstanceModel(sectionTpl, sectionModel, proxyModel);
        let formArray = <FormArray>sectionModel.group.get('instances');
        formArray.push(sectionInstance.group);
        sectionModel.instances.push(sectionInstance);

        return sectionInstance;
    }

    removeSectionInstance(sectionInstance: SectionInstanceModel, formModel: FormModel): boolean {
        if (!sectionInstance) {
            return false;
        }

        let sectionModel = sectionInstance.section;
        let instancesCount = sectionModel.instances.length;

        // remove from section model collection
        sectionModel.instances =
            sectionModel.instances.filter((item: SectionInstanceModel) => item.id !== sectionInstance.id);

        // delete from formArray - sectionModel.group["instances"]
        let formArray = <FormArray>sectionModel.group.get('instances');
        if (formArray) {
            let index = formArray.controls.indexOf(sectionInstance.group);
            if (index !== -1) {
                formArray.removeAt(index);
            }
        }

        // delete from proxy model
        this.proxyModelService.removeSectionInstance(sectionInstance, formModel.proxyModel);

        // remove link to parent
        sectionInstance.section = null;
        return sectionModel.instances.length !== instancesCount;
    }

    private createSectionInstanceModel(
        sectionTpl: SectionTemplateModel,
        sectionModel: SectionModel,
        proxyModel: ProxyModel): SectionInstanceModel {

        let newInstanceId = sectionModel.getNextInstanceId();
        let sectionInstance = new SectionInstanceModel(newInstanceId, sectionModel);

        // create customer datasource models at section instance level!
        let dataSourceModels: { [id: string]: CustomDataSourceModel } = {};

        if (sectionTpl.dataSources && sectionTpl.dataSources.length > 0) {
            for (let dsTemplate of sectionTpl.dataSources) {
                dataSourceModels[dsTemplate.id] = new CustomDataSourceModel(dsTemplate.id);
            }
        }

        sectionInstance.dataSources = dataSourceModels;

        if (sectionTpl.enableAction) {
            sectionInstance.sectionEnableStateSelection = sectionTpl.enableAction.initialState;
        }

        let groups: any = {};

        sectionTpl.controls.forEach((control: BaseControlTemplateModel) => {

            let fieldModel = this.createField(control, sectionInstance);

            sectionInstance.fields[control.id] = fieldModel;
            groups[control.id] = fieldModel.getControlGroup();

            this.proxyModelService.createProxyModelProperty(
                proxyModel,
                sectionTpl.tab,
                sectionTpl,
                control,
                sectionModel.tab,
                sectionModel,
                sectionInstance,
                fieldModel);
        });

        sectionInstance.group = new FormGroup(groups);
        return sectionInstance;
    }

    createMultiControlField(controlTemplateModel: BaseControlTemplateModel, sectionInstance: SectionInstanceModel): MultiControlFieldModel {
        let fieldModel = new MultiControlFieldModel(controlTemplateModel.id, controlTemplateModel, sectionInstance);
        fieldModel.items = fieldModel.preFilteredItems = this.getFieldItemsArray(controlTemplateModel);

        this.createControlsForMultiControl(fieldModel, controlTemplateModel);
        return fieldModel;
    }

    createControlsForMultiControl(fieldModel: MultiControlFieldModel, controlTemplateModel: BaseControlTemplateModel): void {
        let formGroupItems: any = {};
        if (fieldModel.items) {
            fieldModel.items.forEach((item: ItemModel) => {
                formGroupItems[item.value] = (fieldModel.controls && fieldModel.controls.controls[item.value])
                    ? <FormControl>fieldModel.controls.controls[item.value]
                    : new FormControl('');
            });
        }

        fieldModel.controls = new FormGroup(formGroupItems);

        // subscribe to Form Model change
        fieldModel.controls.valueChanges.pipe(
            debounceTime(100)
        ).subscribe((value: any) => {
            this.componentCommService.announceControlValueChanged(
                new ControlValueChangedParams(controlTemplateModel, fieldModel.controls, value, fieldModel));
        });
    }

    private createField(controlTpl: BaseControlTemplateModel, sectionInstance: SectionInstanceModel): FieldModelBase {
        let fieldModel: FieldModelBase;

        if (controlTpl.controlType === FormControlType.CheckboxList) {
            fieldModel = this.createMultiControlField(controlTpl, sectionInstance);
        } else {
            fieldModel = this.createSingleControlField(controlTpl, sectionInstance);
        }
        fieldModel.isRequired = controlTpl.isRequired;
        return fieldModel;
    }

    getSectionFields(section: SectionInstanceModel, fieldIds?: string[], includeHtmlFields = true): FieldModelBase[] {
        let fields: FieldModelBase[] = [];
        if (section && section.fields) {
            for (let fieldId of Object.keys(section.fields)) {
                if (!fieldIds || fieldIds.indexOf(fieldId) !== -1) {
                    if (!includeHtmlFields && section.fields[fieldId].template.controlType === FormControlType.Html) {
                        continue;
                    }
                    let fieldModel = section.fields[fieldId];
                    if (fieldModel) {
                        fields.push(fieldModel);
                    }
                }
            }
        }
        return fields;
    }

    private createSingleControlField(controlTpl: BaseControlTemplateModel, sectionInstance: SectionInstanceModel): SingleControlFieldModel {

        let fieldModel =
            (controlTpl.controlType === FormControlType.Multiselect)
                ? new MultiValueSingleControlFieldModel(controlTpl.id, controlTpl, sectionInstance)
                : new SingleControlFieldModel(controlTpl.id, controlTpl, sectionInstance);

        fieldModel.items = fieldModel.preFilteredItems = this.getFieldItemsArray(controlTpl);

        fieldModel.control = new FormControl('');
        // subscribe to Form Model change
        fieldModel.control.valueChanges.pipe(
            debounceTime(100))
            .subscribe((value: any) => {
                this.componentCommService.announceControlValueChanged(new ControlValueChangedParams(controlTpl, fieldModel.control, value, fieldModel));
            });

        return fieldModel;
    }

    private getFieldItemsArray(control: BaseControlTemplateModel): ItemModel[] {
        let items: ItemModel[] = null;
        if (control.hasOwnProperty('dataSource') && control.dataSource instanceof Array) {
            let ds = <Array<DataSourceItemTemplateModel>>control.dataSource;
            if (ds) {
                items =
                    ds.map((dataSourceItem: DataSourceItemTemplateModel) => new ItemModel(dataSourceItem.value, dataSourceItem.text));
            }
        }
        return items;
    }

    setFieldValue(fieldModel: FieldModelBase, value: any, additionalData: any = '') {
        fieldModel.setAdditionalData(additionalData);

        // fix null, undefined, empty to control supported value
        let fixedValue = this.fixValueForControl(value, fieldModel);

        this.changeControlValue(fieldModel, fixedValue);
    }

    fixValueForControl(value: any, fieldModel: FieldModelBase) {
        let newValue = value;

        if (commonMethods.isUndefinedNullOrEmpty(newValue)) {
            if (fieldModel instanceof SingleControlFieldModel) {

                if (fieldModel instanceof MultiValueSingleControlFieldModel) {
                    newValue = [];
                } else {
                    newValue = '';
                }
            } else if (fieldModel instanceof MultiControlFieldModel) {
                let chkValues = fieldModel.getMultiControlValues();
                Object.keys(chkValues).forEach(key => chkValues[key] = false);
                newValue = chkValues;
            }
        }

        return newValue;
    }

    private changeControlValue(fieldModel: FieldModelBase, value: any) {
        this.logService.debugFormatted('[CTRL_VALUE_CHANGE] Programatic value change! Control: {0}, NewValue: {1}', fieldModel.id, value);
        fieldModel.setControlValue(value);
    }

    setDisabled(fieldModel: FieldModelBase, newValue: boolean): boolean {
        let fieldControlGroup = fieldModel.getControlGroup();
        if (fieldControlGroup.disabled !== newValue) {
            if (newValue) {
                fieldControlGroup.disable({ emitEvent: false });
            } else {
                fieldModel.enable({ emitEvent: false}, true);
            }
            return true;
        }

        return false;
    }
}


