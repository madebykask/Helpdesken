import { ValidatorError, MinMax, Trigger } from '../shared/validation-types';
import { AbstractControl, ValidatorFn, Validators, ValidationErrors, FormGroup, FormArray, FormControl } from '@angular/forms';
import { LogService } from '../services/log.service';
import { Injectable, Inject, forwardRef } from '@angular/core';
import { ProxyModel, ProxyControl } from '../models/proxy.model';
import { BaseControlTemplateModel, TemplateValidator } from '../models/template.model';
import { FormModel, TabModel, SectionInstanceModel, FieldModelBase, FormControlType } from '../models/form.model';
import { AppConfig } from '../shared/app-config/app-config';
import { UuidGenerator } from '../utils/uuid-generator';
import { ErrorHandlingService } from './error-handling.service';
import * as moment from 'moment';
import * as commonMethods from '../utils/common-methods';
import { IAppConfig } from '../shared/app-config/app-config.interface';

@Injectable()
export class ValidatorsService {

    constructor(private logService: LogService,
        private errorHandlingService: ErrorHandlingService,
        @Inject(forwardRef(() => AppConfig)) private config: IAppConfig) {
    }

    setupValidators(formModel: FormModel, finishingType: number): void {
        let fieldIterator = formModel.createFieldsIterator();


        fieldIterator.forEach(
            (fieldModel, fieldPath) => this.setFieldValidators(fieldModel, formModel.proxyModel, finishingType));
    }

    setupSectionInstanceValidators(sectionInstance: SectionInstanceModel, proxyModel: ProxyModel, finishingType: number): void {
        for (let fieldKey of Object.keys(sectionInstance.fields)) {
            let fieldModel = sectionInstance.fields[fieldKey];
            this.setFieldValidators(fieldModel, proxyModel, finishingType);
        }
    }

    private setFieldValidators(fieldModel: FieldModelBase, proxyModel: ProxyModel, finishingType: number) {
        let controlTpl = fieldModel.template;
        let validators = controlTpl.validators;
        if (validators) {
            

            this.logService.debugFormatted('Validation({0}): applying validators', fieldModel.id);
            fieldModel.getControlGroup().setValidators(this.validatorFactory(fieldModel, proxyModel, finishingType));


        }
    }

    validateFormControls(group: FormGroup | FormArray, deepScan: boolean = true): void {
        Object.keys(group.controls).forEach((controlName) => {
            const ctrl = group.get(controlName);

            if (this.isFormArray(ctrl)) {
                this.validateFormControls(ctrl as FormArray, true);
            }
            else if (this.isFormGroup(ctrl)) {
                this.validateFormControls(ctrl as FormGroup, true);
            }
            //formgroup and form array also can have validators (example checkbox-list)
            ctrl.updateValueAndValidity({
                onlySelf: true,
                emitEvent: false
            });
        });
    }

    getAllTabErrors(tabModel: TabModel): Array<ValidatorError> {
        let result = new Array<ValidatorError>();
        Object.keys(tabModel.sections).forEach((sectionId: string) => {

            for (let sectionInstance of tabModel.sections[sectionId].instances) {

                Object.keys(sectionInstance.fields).forEach((fieldId: string) => {
                    let fieldModel = sectionInstance.fields[fieldId];
                    let control = fieldModel.getControlGroup();
                    if (control.invalid) {
                        Object.keys(control.errors).forEach((errorName: string) => {
                            result.push(control.errors[errorName]);
                        });
                    }
                });
            }
        });

        return result;
    }

    updateFieldsValidationStatus(controlsToValidate: AbstractControl[]) {
        if (!controlsToValidate)
            return;

        controlsToValidate.forEach(
            (ctrl: AbstractControl) => ctrl.updateValueAndValidity({ onlySelf: true, emitEvent: false }));

    }

    updateTabsValidationStatus(formModel: FormModel) {
        Object.keys(formModel.tabs).forEach(
            (tabName: string) => { // force to update top formgroup validation status
                formModel.tabs[tabName].group.updateValueAndValidity({
                    onlySelf: true,
                    emitEvent: false
                });
                formModel.tabs[tabName].valid =
                    !formModel.tabs[tabName].group.invalid;
            });
    }

    updateSectionInstanceFieldsRequiredState(section: SectionInstanceModel, proxyModel: ProxyModel) {
        for (let fieldId of Object.keys(section.fields)) {
            let field = section.fields[fieldId];
            this.updateIsRequired(field, proxyModel);
        }
    }
    
    updateIsRequired(fieldModel: FieldModelBase, proxyModel: ProxyModel): void {
        let controlTemplate = fieldModel.template;
        if (!controlTemplate.validators) return;

        let fieldPath = fieldModel.getFieldPath();
        let proxyControl = proxyModel.findProxyControl(fieldPath);
        let validators = controlTemplate.validators.getValidatorsByType('required');

        validators.forEach((validatorTemplate: TemplateValidator) => {
            const isEnabled = this.isValidatorEnabled(validatorTemplate, proxyControl, proxyModel);
            if (validatorTemplate.type === 'required') {
                fieldModel.isRequired = isEnabled ? validatorTemplate.validationMode : null; // not required if validation is disabled
            }
        });
    }

    private isValidatorEnabled(
        validatorTemplate: TemplateValidator,
        proxyControl: ProxyControl,
        proxyModel: ProxyModel): boolean {
        if (typeof validatorTemplate.enabled === 'function') {
            return validatorTemplate.enabled.call(proxyControl, proxyModel);
        }

        return true;
    }

    private validatorFactory(fieldModel: FieldModelBase, proxyModel: ProxyModel, finishingType: number): ValidatorFn[] {
        //this.logService.debug(`Validation(${controlTemplateModel.id}): validatorFactory`);
        let controlTemplateModel = fieldModel.template;
        const validators = new Array<ValidatorFn>();

        if (controlTemplateModel.validators.onSave) {



            controlTemplateModel.validators.onSave.forEach((item: TemplateValidator) => {



                if(finishingType == 0 || finishingType == NaN){
                    if (item.trigger == Trigger.Normal) {


                        validators.push(this.createValidator(item, fieldModel, proxyModel, controlTemplateModel));
    
                    }
                }
                else{
                    if (item.trigger == Trigger.Normal) {


                        validators.push(this.createValidator(item, fieldModel, proxyModel, controlTemplateModel));
    
                    }
                    else if(item.trigger == Trigger.OnCaseClose){

                        validators.push(this.createValidator(item, fieldModel, proxyModel, controlTemplateModel));

                    }
                }


            });



        }
        if (controlTemplateModel.validators.onNext) {
            controlTemplateModel.validators.onNext.forEach((item: TemplateValidator) => {
                validators.push(this.createValidator(item, fieldModel, proxyModel, controlTemplateModel));
            });
        }
        return validators.length > 0 ? validators : null;
    }

    private createValidator(template: TemplateValidator, fieldModel: FieldModelBase, proxyModel: ProxyModel, controlTemplateModel: BaseControlTemplateModel): ValidatorFn {
        let validatorFn: ValidatorFn = null;
        let proxyControl = proxyModel.findProxyControl(fieldModel.getFieldPath());

        switch (template.type) {
            case 'required':
                {
                    let requiredFn = Validators.required;

                    if (controlTemplateModel.controlType === FormControlType.Dropdown &&
                        !commonMethods.isUndefinedNullOrEmpty(template.emptyValues) &&
                        template.emptyValues.length)
                    {
                        requiredFn = (control: AbstractControl): ValidationErrors => {
                            if (commonMethods.isUndefinedNullOrEmpty(control.value) || 
                                template.emptyValues.some((o: string) => control.value === o)) {
                                const errorResult = Object.create(null);
                                errorResult[template.type] = control.value;
                                return errorResult;
                            }
                            return null;
                        }
                    }

                    if (controlTemplateModel.controlType === FormControlType.CheckboxList) {
                        requiredFn = (control: AbstractControl): ValidationErrors => {
                            const frmGroup = control as FormGroup;
                            let hasValue = false;
                            Object.keys(frmGroup.controls).forEach((name: string) => {
                                if (frmGroup.controls[name].value === true ||
                                    frmGroup.controls[name].value === 'true') {
                                    hasValue = true;
                                }
                            });
                            const errorResult = Object.create(null);
                            errorResult[template.type] = control.value;
                            return hasValue ? null : errorResult;
                        }

                    }

                    validatorFn = (control: AbstractControl): ValidationErrors => {
                        if (this.config.isManualValidation) { return requiredFn(control); } // fire required only on save, not on leave
                        const errorResult = Object.create(null);
                        errorResult[template.type] = control.value;
                        const existingErrors = Object.keys(control.errors || {}).filter((keyName: string) => {
                            return keyName.startsWith('required');
                        });
                        let hasValue = false;
                        if (control.value !== null) {
                            switch (typeof control.value) {
                                case 'string':
                                {
                                    hasValue = control.value.length > 0;
                                    break;
                                }
                                case 'object':
                                {
                                    hasValue = Object.keys(control.value).filter((name: string) => {
                                            return control.value[name] === true || control.value[name] === 'true';
                                        }).length > 0;
                                    break;
                                }
                                case 'boolean':
                                {
                                    hasValue = control.value;
                                    break;
                                }
                                default:
                            };
                        }
                        return (existingErrors.length > 0 && !hasValue) ? errorResult : null;
                    };


                    break;
                }
            case 'minLength': {
                    if (!template.value) { 
                      this.logService.warningFormatted('Missing validation min length for control: {0}', controlTemplateModel.id); 
                    }

                    validatorFn = Validators.minLength(Number(template.value));
                    break;
                }
            case 'maxLength': {
                    if (!template.value) { 
                      this.logService.warningFormatted('Missing validation max length for control: {0}', controlTemplateModel.id); 
                    }

                    validatorFn = Validators.maxLength(Number(template.value));
                    break;
                }
            case 'min': {
                    if (!template.value) { 
                      this.logService.warningFormatted('Missing validation min value for control: {0}', controlTemplateModel.id); 
                    }

                    validatorFn = this.createMinMaxFn(template, controlTemplateModel, MinMax.Min);
                    break;
                }
            case 'max': {
                    if (!template.value) { 
                      this.logService.warningFormatted('Missing validation max value for control: {0}', controlTemplateModel.id); 
                    }

                    validatorFn = this.createMinMaxFn(template, controlTemplateModel, MinMax.Max);
                    break;
                }
            case 'range': {
                    if (!template.value) { 
                      this.logService.warningFormatted('Missing validation range value for control: {0}', controlTemplateModel.id); 
                    }

                    validatorFn = this.createMinMaxFn(template, controlTemplateModel, MinMax.Range);
                    break;
                }
            case 'pattern': {
                    if (!template.value) { 
                      this.logService.warningFormatted('Missing validation regex pattern for control: {0}', controlTemplateModel.id); 
                    }

                    validatorFn = Validators.pattern(template.value);
                    break;
                }
            case 'dateFormat': {
                    validatorFn = (control: AbstractControl): ValidationErrors => {
                        if (control.value.length === 0) { return null; }
                        let dateFormat = controlTemplateModel.mode === 'year' ? this.config.yearFormat : this.config.dateFormat;

                        const isError = !moment(control.value, dateFormat, true).isValid();
                        const errorResult = Object.create(null);
                        errorResult[template.type] = control.value;
                        return isError ? errorResult : null;
                    }
                    break;
                }
            case 'custom': {
                    if (typeof template.valid !== 'function') {
                        this.errorHandlingService.handleError(null, `Valid function is missing for custom validator of the field '${fieldModel.getFieldPath().buildFormFieldPath()}'`);
                        break;
                    }

                    validatorFn = (control: AbstractControl): ValidationErrors => {

                        const isError = !template.valid.call(proxyControl, proxyModel);
                        const errorResult = Object.create(null);
                        errorResult[template.type] = control.value;
                        return isError ? errorResult : null;
                    };
                    break;
                }
            default:
                this.errorHandlingService.handleError(null, `Unknown validator type: ${template.type}. Field: ${fieldModel.getFieldPath().buildFormFieldPath()}`);
        }

        if (!validatorFn) {
            return Validators.nullValidator; // ignore validator
        }

        return (control: AbstractControl): ValidationErrors => {
            const localValidationTemplate = template;
            const localControlTemplate = controlTemplateModel;
            const localFieldModel = fieldModel;
            this.logService.debugFormatted('Validation({0}): running validation func. Mode: {1}', localControlTemplate.id, this.config.validationMode);

            if (localFieldModel.hidden) { return null; }
            // if (control.pristine && !this.config.isManualValidation) return null;

            if (this.config.validationMode < localValidationTemplate.validationMode) { return null; }
            localControlTemplate.isEverValidated = true;

            // ignore validation if validation disabled 
            const isEnabled = this.isValidatorEnabled(localValidationTemplate, proxyControl, proxyModel);
            if (localValidationTemplate.type === 'required') {
                localFieldModel.isRequired = isEnabled ? localValidationTemplate.validationMode : null; // not required if validation is disabled
            }
            if (!isEnabled) return null;

            const isError = validatorFn(control);
            const errorResult = Object.create(null);
            const errorName = `${localValidationTemplate.type}_${UuidGenerator.createUuid()}`;
            errorResult[errorName] = new ValidatorError(localControlTemplate.id,
                localValidationTemplate.type, localControlTemplate.label,
                localValidationTemplate.validationMode, localValidationTemplate.message, localValidationTemplate.trigger);
            return isError ? errorResult : null;
        };
    }

    private isFormGroup(ctrl: AbstractControl): boolean {
        return ctrl instanceof FormGroup;
    }

    private isFormArray(ctrl: AbstractControl): boolean {
        return ctrl instanceof FormArray;
    }

    private isFormControl(ctrl: AbstractControl): boolean {
        return ctrl instanceof FormControl;
    }

    private createMinMaxFn(template: TemplateValidator, controlTemplateModel: BaseControlTemplateModel, type: MinMax): ValidatorFn {
        return (control: AbstractControl): ValidationErrors => {
            if (control.value.length <= 0) { return null; }

            const errorResult = Object.create(null);
            errorResult[template.type] = control.value;
            let dateCompareFn = (...args: any[]) => { return false };
            let numericCompareFn = (...args: any[]) => { return false; }

            switch (type) {
                case MinMax.Min: {
                        dateCompareFn = (curDate: moment.Moment, min: moment.Moment) => {
                            return curDate.isSameOrBefore(min);
                        }
                        numericCompareFn = (curNum: number, min: number) => {
                            return curNum <= min;
                        }
                        break;
                    }
                case MinMax.Max: {
                        dateCompareFn = (curDate: moment.Moment, max: moment.Moment) => {
                            return curDate.isSameOrAfter(max);
                        }
                        numericCompareFn = (curNum: number, max: number) => {
                            return curNum >= max;
                        }
                        break;
                    }
                case MinMax.Range:{
                        dateCompareFn = (curDate: moment.Moment, min: moment.Moment, max: moment.Moment) => {
                            return !curDate.isBetween(min, max);
                        }
                        numericCompareFn = (curNum: number, min: number, max: number) => {
                            return curNum < min || curNum > max;
                        }
                        break;
                    }

                default:
                    this.errorHandlingService.handleError(null, `Unknown validator MinMax type: ${type}.`);
            }

            if (controlTemplateModel.controlType === FormControlType.Date) {
                const curDate = moment(control.value, this.config.dateFormat, true);
                let templateDate: moment.Moment;
                let templateDate1: moment.Moment;
                if (template.value instanceof Array) {
                    templateDate = moment(template.value[0], this.config.dateFormat);
                    templateDate1 = moment(template.value[1], this.config.dateFormat);
                } else {
                    templateDate = moment(template.value, this.config.dateFormat);
                    templateDate1 = null;
                }

                if (!curDate.isValid() || dateCompareFn(curDate, templateDate, templateDate1)) {
                    return errorResult;
                }
            } else {
                const curNum = commonMethods.toNumber(control.value);
                let templateNum: number;
                let templateNum1: number;
                if (template.value instanceof Array) {
                    templateNum = commonMethods.toNumber(template.value[0]);
                    templateNum1 = commonMethods.toNumber(template.value[1]);
                } else {
                    templateNum = commonMethods.toNumber(template.value);
                    templateNum1 = null;
                }

                if (isNaN(curNum) || numericCompareFn(curNum, templateNum, templateNum1)) {
                    return errorResult;
                }
            }

            return null;
        }
    }
}



