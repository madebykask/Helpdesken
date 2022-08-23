import { Injectable, Inject, forwardRef } from '@angular/core';

import {
    FormTemplateModel, BaseControlTemplateModel, SectionTemplateModel, TabTemplateModel, OptionsDataSourceTemplateModel, DataSourceItemTemplateModel, ControlCustomDataSourceTemplateModel,
    ControlSectionDataSourceTemplateModel, CustomStaticDataSourceTemplateModel, CustomQueryDataSourceTemplateModel, TemplateValidators, TemplateValidator, SectionType,
    DisabledStateBehavior, DisabledStateAction, DisabledStateActionCondition, ControlDataSourceTemplateModelTypes, CustomDataSourceTypes, DataSourceParameterTemplateModel, IDataSourceParameter
} from '../models/template.model';

import { LogService } from '../services/log.service';
import { Trigger, ValidateOn } from '../shared/validation-types';
import { AppConfig } from '../shared/app-config/app-config';
import { FormControlType } from '../models/form.model';
import { ErrorHandlingService } from './error-handling.service';

import * as commonMethods from '../utils/common-methods';
import { IAppConfig } from '../shared/app-config/app-config.interface';

@Injectable()
export class TemplateService {

    constructor(private logService: LogService,
        private errorHandlingService: ErrorHandlingService,
        @Inject(forwardRef(() => AppConfig)) private config: IAppConfig) {
    }

    findCustomDataSourceTemplateById(dataSources: CustomDataSourceTypes[], id: string): CustomQueryDataSourceTemplateModel {
        if (dataSources && dataSources.length) {
            let dsModel =
                <CustomQueryDataSourceTemplateModel>dataSources.find(
                    (el: any) => el instanceof CustomQueryDataSourceTemplateModel && el.id === id);
            return dsModel;
        }
        return null;
    }

    toTemplateModel(template: any): FormTemplateModel {
        this.logService.debug('Building template model.');
        let model = new FormTemplateModel();
        // TODO: add hadOwnProperty check
        model.id = template.id;
        model.name = template.name;

        if (template.hasOwnProperty('dataSources')) {
            model.dataSources = template.dataSources.map((el: any) => this.createCustomDataSourceModel(el));
        }

        if (template.hasOwnProperty('localization')) {
            model.localization = template.localization;
        }
        if (template.hasOwnProperty('styles')) {
            model.styles = template.styles;
        }

        model.globalFunctions = template.globalFunctions || {};

        model.tabs = template.tabs.map((tabElem: any) => {

            let columnCount = tabElem.columnCount ? parseInt(tabElem.columnCount) : 0;

            // set default value if nan or empty
            if (!columnCount || isNaN(columnCount) || columnCount > 4 || columnCount < 1) {
                columnCount = 2;
            }

            let tab = new TabTemplateModel(tabElem.id, tabElem.name, columnCount,
                tabElem.hasOwnProperty('hiddenBinding') ? this.getBinding(tabElem.hiddenBinding, template.globalFunctions) : null,
                tabElem.hasOwnProperty('disabledBinding') ? this.getBinding(tabElem.disabledBinding, template.globalFunctions) : null);

            let sections = tabElem.sections.map((secElem: any) => {
                let section =
                    new SectionTemplateModel(secElem.id,
                        secElem.name,
                        tab,
                        secElem.column || 0,
                        secElem.hasOwnProperty('hiddenBinding') ? this.getBinding(secElem.hiddenBinding, template.globalFunctions) : null,
                        secElem.hasOwnProperty('disabledBinding') ? this.getBinding(secElem.disabledBinding, template.globalFunctions) : null,
                        secElem.hasOwnProperty('multiSectionAction') ? secElem.multiSectionAction : null,
                        secElem.hasOwnProperty('populateAction') ? this.getPopulateAction(secElem.populateAction, template.globalFunctions) : null,
                        secElem.hasOwnProperty('enableAction') ? secElem.enableAction : null,
                        secElem.hasOwnProperty('disabledStateAction') ? this.getDisabledStateBehavior(secElem) : null);

                if (secElem.hasOwnProperty('type')) {
                    section.type = <SectionType>SectionType[<string>secElem.type];
                }

                if (secElem.hasOwnProperty('reviewSectionId')) {
                    section.reviewSectionId = <string>secElem.reviewSectionId || '';
                }

                if (secElem.hasOwnProperty('reviewControls')) {
                    section.reviewControls = secElem.reviewControls;
                }

                if (secElem.hasOwnProperty('dataSources')) {
                    section.dataSources = secElem.dataSources.map((el: any) => this.createCustomDataSourceModel(el));
                }

                if (secElem.controls && secElem.controls.length) {

                    let controls = secElem.controls.map((controlElem: any, index: number) => {
                        let dataSource: ControlDataSourceTemplateModelTypes = null;
                        if (controlElem.hasOwnProperty('dataSource')) {
                            dataSource = this.createControlDataSourceTemplateModel(controlElem.dataSource);
                        }

                        let control = new BaseControlTemplateModel({
                            id: controlElem.id,
                            label: controlElem.label != null ? controlElem.label : (controlElem.htmlLabel || ''),
                            isLabelHtml: controlElem.htmlLabel != null,
                            addonText: controlElem.addonText,
                            order: controlElem.order || index,
                            controlType: controlElem.type,
                            caseBinding: controlElem.caseBinding,
                            caseBindingBehaviour: controlElem.caseBindingBehaviour,
                            resetValueOnItemsUpdate: controlElem.resetValueOnItemsUpdate,
                            shouldNotSave: controlElem.hasOwnProperty('shouldNotSave') ?
                                          controlElem.shouldNotSave :
                                          (controlElem.type === FormControlType.Review),
                            noDigest: controlElem.noDigest,
                            section: section,
                            valueBinding: controlElem.hasOwnProperty('valueBinding') ?
                                          this.getBinding(controlElem.valueBinding, template.globalFunctions) :
                                          null,
                            hiddenBinding: controlElem.hasOwnProperty('hiddenBinding') ?
                                          this.getBinding(controlElem.hiddenBinding, template.globalFunctions) :
                                          null,
                            disabledBinding: controlElem.hasOwnProperty('disabledBinding') ?
                                             this.getBinding(controlElem.disabledBinding, template.globalFunctions) :
                                             null,
                            dataSourceFilterBinding: controlElem.hasOwnProperty('dataSourceFilterBinding') ?
                                                     this.getBinding(controlElem.dataSourceFilterBinding, template.globalFunctions) :
                                                     null,
                            dataSource: dataSource,
                            validators: controlElem.hasOwnProperty('validators') ?
                                        this.getValidators(controlElem.validators, template.validatorsMessages, template.globalFunctions) :
                                        null,
                            warningBinding: controlElem.hasOwnProperty('warningBinding') ?
                                            this.getBinding(controlElem.warningBinding, template.globalFunctions) :
                                            null,
                            mode: controlElem.hasOwnProperty('mode') ? controlElem.mode : '',
                            reviewControlId: controlElem.hasOwnProperty('reviewControlId') ? controlElem.reviewControlId : '',
                            emptyElement: controlElem.hasOwnProperty('emptyElement') ? controlElem.emptyElement : '',
                            showSearchResultsBinding: controlElem.hasOwnProperty('showSearchResultsBinding') ?
                                              this.getBinding(controlElem.showSearchResultsBinding, template.globalFunctions) :
                                              () => true
                        });

                        // mark if control is required for first time
                        if (control.validators) {
                            if (control.validators.onSave && control.validators.onSave.filter((val: TemplateValidator) => {
                                return val.type === 'required';
                            }).length > 0) {
                                control.isRequired = ValidateOn.OnSave;
                            }
                            if (control.validators.onNext && control.validators.onNext.filter((val: TemplateValidator) => {
                                return val.type === 'required';
                            }).length > 0) {
                                control.isRequired = ValidateOn.OnNext;
                            }
                        }

                        // add mandatory validators
                        if (control.controlType === FormControlType.Date) {
                            this.addDateFormatValidator(control, template.validatorsMessages);
                        }

                        // register control in the map for fast and easy access
                        // model.registerControl(tab.id, section.id, control.id, control);

                        return control;
                    });

                    section.controls = controls.sort((a: any, b: any) => a.order - b.order);
                }
                return section;
            });

            tab.sections = sections;
            return tab;
        });

        this.logService.debug('Template model has been built.');
        return model;
    }

    expandParametersForSection(sectionIndex: number, parameters: DataSourceParameterTemplateModel[]) {
        let fixedParameters: DataSourceParameterTemplateModel[] = [];

        if (parameters && parameters.length) {
            for (let parameter of parameters) {
                let fieldPath = parameter.field.replace('%index%', sectionIndex.toString());
                let fixedParameter = new DataSourceParameterTemplateModel(parameter.name, fieldPath);
                fixedParameters.push(fixedParameter);
            }
        }
        return fixedParameters;
    }

    expandFieldsForSection(sectionIndex: number, fields: string[]) {
        let expandedFields: string[] = [];

        if (fields && fields.length) {
            expandedFields =
                fields.map((field: string) => field.replace('%index%', sectionIndex.toString()));
        }
        return expandedFields;
    }

    private addDateFormatValidator(controlTemplate: BaseControlTemplateModel, messages: any): void {
        controlTemplate.validators = controlTemplate.validators || new TemplateValidators();
        controlTemplate.validators.onSave = controlTemplate.validators.onSave || new Array<TemplateValidator>();
        const errMessage = (controlTemplate.mode === 'year' ? messages['dateYearFormat'] : messages['dateFormat']) || '';
        if (!controlTemplate.validators.onSave
            .filter((item: TemplateValidator) => { return item.type === 'dateFormat' }).length) {
            controlTemplate.validators.onSave.push(new TemplateValidator({
                type: 'dateFormat',
                message: errMessage.length === 0 ? this.config.dateFormat : errMessage
            }));
        }
    }

    private createCustomDataSourceModel(item: any): CustomStaticDataSourceTemplateModel | CustomQueryDataSourceTemplateModel {
        let dataSourceType: string = item.type;

        if (dataSourceType === 'static') {
            return new CustomStaticDataSourceTemplateModel(item.id, item.data);
        } else if (dataSourceType === 'query') {
            return new CustomQueryDataSourceTemplateModel(item.id, item.parameters);
        }

        throw `Not supported custom data source template. Id:${item.id}, Type: ${dataSourceType} `;
    }

    private createControlDataSourceTemplateModel(dataSourceMeta: any): ControlDataSourceTemplateModelTypes {
        if (dataSourceMeta instanceof Array) {
            return dataSourceMeta.map((dataSourceItem: any) => new DataSourceItemTemplateModel(dataSourceItem.value, dataSourceItem.text));
        } else if (dataSourceMeta.hasOwnProperty('type')) {
            if (dataSourceMeta.type === 'option') {
                return new OptionsDataSourceTemplateModel(dataSourceMeta.id, dataSourceMeta.parameters, dataSourceMeta.dependsOn);
            } else if (dataSourceMeta.type === 'custom') {
                return new ControlCustomDataSourceTemplateModel(dataSourceMeta.id, dataSourceMeta.valueField, dataSourceMeta.textField);
            } else if (dataSourceMeta.type === 'section') {
                return new ControlSectionDataSourceTemplateModel(dataSourceMeta.id, dataSourceMeta.valueField, dataSourceMeta.textField);
            }
        }

        throw `Not supported control data source template. Id:${dataSourceMeta.id}`;
    }

    // creates template validators from metaData
    private getValidators(templateValidators: any, messages: any, globalFunctions: any): TemplateValidators {
        const controlValidators = new TemplateValidators();

        const createTemplate = (items: Array<any>, mode: ValidateOn): Array<TemplateValidator> => {

            return items.map((item: any): TemplateValidator => {

                let validatorTpl = new TemplateValidator({
                    type: item.type,
                    message: this.getValidatorMessage(item, messages),
                    enabled: this.getBinding(item.enabled, globalFunctions) || null,
                    valid: this.getBinding(item.valid, globalFunctions) || null,
                    value: this.resolveValue(item.value, globalFunctions) || null,
                    validationMode: mode,
                    trigger: this.getTrigger(item)
                });

                if (item.hasOwnProperty('emptyValues')) {
                    let emptyValues = item.emptyValues || [];
                    validatorTpl.emptyValues =
                        commonMethods.isArray(emptyValues) ? emptyValues.map((e:any) => e.toString()) : [ emptyValues.toString() ];
                }

                return validatorTpl;
            });
        }

        if (templateValidators.hasOwnProperty('onSave')) {
            controlValidators.onSave = createTemplate(templateValidators.onSave, ValidateOn.OnSave);
        }

        if (templateValidators.hasOwnProperty('onNext')) {
            controlValidators.onNext = createTemplate(templateValidators.onNext, ValidateOn.OnNext);
        }
        if (!controlValidators.onSave && !controlValidators.onNext) { this.logService.warning('Invalid template validators section.'); }

        return controlValidators;
    }

    private getTrigger(item: any): Trigger{

        //Check if trigger is an property, if not. Return an default enum(0)
        if(!item.hasOwnProperty('trigger')){
            return Trigger.Normal;
        }
        else {

            //Capatalize first char of trigger attribute
            let localTrigger = item.trigger.charAt(0).toUpperCase() + item.trigger.slice(1);

            //Parse to enum
            var triggerEnum : Trigger = Trigger[localTrigger as keyof typeof Trigger];

            //Cast to trigger and return
            return triggerEnum
        }
    }

    private getValidatorMessage(item: any, messages: any): string {
        if (item.hasOwnProperty('message')) { return item.message || ''; }
        if (item.hasOwnProperty('messageName') && messages.hasOwnProperty(item.messageName)) { return messages[item.messageName] || ''; }
        if (messages.hasOwnProperty(item.type)) { return messages[item.type] || ''; }

        this.errorHandlingService.handleWarning(`No message found for validation type: ${item.type}`);
        return '';
    }

    private resolveValue(value: any, globalFunctions: any): any {

        if (typeof value === 'function') {
            return value();
        }

        if (typeof value === 'string' && globalFunctions && globalFunctions.hasOwnProperty(value)) {
            return globalFunctions[value]();
        }

        return value;
    }

    private getBinding(binding: any, globalFunctions: any): (m: any, l?: any) => any {
        if (typeof binding === 'string' && globalFunctions && globalFunctions[binding]) {
            return globalFunctions[binding];
        }

        if (typeof binding === 'function') {
            return binding;
        }

        return null;
    }

    private getDisabledStateBehavior(secTpl:any): DisabledStateBehavior {

        let action = <DisabledStateAction>DisabledStateAction[<string>secTpl.disabledStateAction];
        let condition = DisabledStateActionCondition.UserOnly;

        if (action === DisabledStateAction.ClearOnUserOnly) {
            action = DisabledStateAction.Clear;
            condition = DisabledStateActionCondition.UserOnly;
        }

        if (secTpl.hasOwnProperty('disabledStateActionCondition')) {
            condition = <DisabledStateActionCondition>DisabledStateActionCondition[<string>secTpl.disabledStateActionCondition];
        }

        let behavior = new DisabledStateBehavior(action, condition);
        return behavior;
    }

    private getPopulateAction(populateAction: any, globalFunctions: any): any {
        if (!populateAction) { return null; }

        if (populateAction.populateBinding) {
            populateAction.populateBinding = this.getBinding(populateAction.populateBinding, globalFunctions);
        }

        return populateAction;
    }
}