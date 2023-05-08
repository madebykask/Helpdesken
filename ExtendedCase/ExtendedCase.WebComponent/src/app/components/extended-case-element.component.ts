
import { of, Observable, Subject } from 'rxjs';
import { catchError, first, switchMap, take} from 'rxjs/operators';
import { Inject, Component, ChangeDetectorRef, NgZone, ViewChild } from '@angular/core';
import { AbstractControl } from '@angular/forms';
import { FormTemplateModel, TabTemplateModel } from '../models/template.model';
import { TemplateService } from '../services/template.service';
import { ProxyModelService } from '../services/proxy-model.service';
import { ProxyModelBuilder } from '../services/proxy-model-builder';
import { FormModelService } from '../services/form-model.service';
import {Form} from '../models/form-public.model'
import { MetaDataService } from '../services/data/meta-data.service';
import { DataSourceService } from '../services/data/data-source.service';
import {
    ComponentCommService, ControlValueChangedParams, PopulateSectionParams, EnableSectionParams,
    AddSectionInstanceParams, DeleteSectionInstanceParams
} from '../services/component-comm.service';

import { FormModel, SectionInstanceModel } from '../models/form.model';
import { FormStateService } from '../services/form-state/form-state.service';
import { QueryParamsService } from '../services/query-params.service';
import { ValidatorsService } from '../services/validators-service';
import { ValidateOn, ValidatorError } from '../shared/validation-types';
import { FormInfo } from '../models/proxy.model';
import { DigestService } from '../services/digest.service';
import { DigestUpdateLogItem, DigestResult, DigestResultContext } from '../models/digest.model';
import { DataSourcesLoaderService } from '../services/datasources-loader.service';
import { FormControlsManagerService } from '../services/form-controls-manager.service';
import { LoadSaveFormDataService } from '../services/load-save-form-data.service';
import { AppConfig } from '../shared/app-config/app-config';
import * as commonMethods from '../utils/common-methods';
import { IMap, ChangedFieldItem } from '../shared/common-types';
import { KeyedCollection } from '../shared/keyed-collection';
import { StorageService } from '../services/storage.service';
import { LogService } from '../services/log.service';
import { FormDataService } from '../services/data/form-data.service';
import { FormDataModel, FormDataSaveResult, FormMetaDataResponse, FieldValueModel } from '../models/form-data.model';
import { WindowWrapper } from '../shared/window-wrapper';
import { FormParametersModel } from '../models/form-parameters.model';
import { SubscriptionManager } from '../shared/subscription-manager';
import { ErrorHandlingService } from '../services/error-handling.service';
import { ProgressComponent } from './shared/progress.component';
import * as moment from 'moment';
import { IAppConfig } from '../shared/app-config/app-config.interface';
import { VERSION } from '../../../config/version';
import { CaseFileModel } from '@app/models/case-file.model';

@Component({
    selector: 'extended-case-element',
    templateUrl: './extended-case-element.component.html',
    providers: [
        MetaDataService, TemplateService, ComponentCommService, ProxyModelService, DigestService, FormModelService, ValidatorsService,
        LoadSaveFormDataService, DataSourcesLoaderService, DataSourceService, FormDataService, StorageService,
        QueryParamsService, FormControlsManagerService, ProxyModelBuilder, FormStateService]
})
export class ExtendedCaseElementComponent {

    public get version() {
      return VERSION.fullVersion;
    }
    @ViewChild(ProgressComponent, {static: false}) private progressComponent: ProgressComponent;

    formParameters: FormParametersModel; // parameters initially received from query string, possible to change from parent window
    dbModel = new Array<any>(); // db data
    templateModel: FormTemplateModel; // parsed template model
    formModel: FormModel; // ui form model
    selectedTabId = ''; // nactive tab id
    formData: FormDataModel = new FormDataModel(); // loaded case and extended case data
    isLoaded = false; // true is app is ready to render form, setting to true runs rendering
    showFormsList = false;
    showDebugPanel = false;

    private subscriptionManager = new SubscriptionManager();
    private caseLoadCompleteSubject: Subject<any> = new Subject<any>();
    private initialData: ICaseInitialData;

    ///////////////////////////////////////////////////////////////////////////// Playground below

    private toggleProgress = false;

    constructor(
        private templateService: TemplateService,
        private metaDataService: MetaDataService,
        private proxyModelService: ProxyModelService,
        private componentCommService: ComponentCommService,
        private formModelService: FormModelService,
        private validatorsService: ValidatorsService,
        private loadSaveFormDataService: LoadSaveFormDataService,
        private digestService: DigestService,
        private formControlsManagerService: FormControlsManagerService,
        private queryParamsService: QueryParamsService,
        private errorHandlingService: ErrorHandlingService,
        private logService: LogService,
        private storageService: StorageService,
        private formStateService: FormStateService,
        @Inject(AppConfig) private config: IAppConfig,
        private window: WindowWrapper,
        private ngZone: NgZone,
        private changeDetector: ChangeDetectorRef) {

            this.logService.debug('ex-component: inited!');
            this.window.extendedCaseComponentRef = { component: this, zone: ngZone };
            this.window.setGlobal('moment', moment); // set moment to window for function bindings
    }

    ngOnInit() {
        this.subscribeEvents();

        this.formParameters = this.queryParamsService.getFormParamenters();

        let isAutoLoad = this.queryParamsService.getAutoloadValue();
        if (isAutoLoad) { // to load form without external code
            this.loadForm();
        }
    }

    ngOnDestroy() {
        if (this.window.extendedCaseComponentRef !== undefined) {
            this.window.extendedCaseComponentRef = null;
        }

        this.subscriptionManager.removeAll();
    }

    getFormParameters(): FormParametersModel {
        return commonMethods.clone(this.formParameters);
    }

    getFormData(): Form {
        let form = this.formControlsManagerService.getFormData(this.formModel);
        return form;
    }

    getCaseValues(): { [id: string]: FieldValueModel } {
        let formData = this.formControlsManagerService.getFormFieldValues(this.formModel, this.formData, true);
        return formData.caseFieldsValues as any;
    }

    setInitialData(options: ICaseInitialData) {
        this.initialData = options;
    }

    setNextStep(step: string, isNextValidation?: boolean) {
        this.setStepInner(step);
        this.setValidationModeInner(isNextValidation);
        this.runDigest(false);
    }

    updateCaseFieldValues(caseValues: { [id: string]: FieldValueModel }) {

        let changedFields: ChangedFieldItem[] = [];
        try {
            changedFields = this.formControlsManagerService.updateCaseFieldValues(caseValues, this.formModel);
        } catch (err) {
            this.errorHandlingService.handleError('Unknown error on updating form with case field values.');
        }

        if (changedFields && changedFields.length) {
            this.logService.debug('Form model has been updated with the case field values. Running digest after fields update.');
            this.runDigestAfterFieldsUpdate(changedFields);
        }
    }

    loadForm(options?: { formParameters?: FormParametersModel, caseValues?: { [id: string]: FieldValueModel; } }): Promise<any> {
        this.isLoaded = false;
        this.caseLoadCompleteSubject = new Subject<any>();

        options = options || {};

        if (options.formParameters) {
            this.formParameters = commonMethods.clone(options.formParameters);
        }

        this.openCase(options.caseValues/*this.commonMethods.clone(options.caseValues)*/);

        // return promise result
        return this.caseLoadCompleteSubject.pipe(first()).toPromise();
    }

    getTabStyle(tabTemplate: TabTemplateModel): string {
        let style = this.formModel.tabs[tabTemplate.id].hidden ? 'hiddenTab' : '';
        if (style.length === 0) {
            style += this.formModel.tabs[tabTemplate.id].valid ? 'ng-valid' : 'ng-invalid';
        }

        return style;
    }

    validate(isOnNext?: boolean, finishingType?: number): Array<ValidatorError> {
        this.config.isManualValidation = true;

        if (isOnNext !== undefined && isOnNext !== null) {
            this.config.validationMode = isOnNext ? ValidateOn.OnNext : ValidateOn.OnSave;
        }


        this.validatorsService.setupValidators(this.formModel, finishingType)


        let allErrors = new Array<ValidatorError>();
        Object.keys(this.formModel.tabs).forEach((tabId: string) => {
            this.validatorsService.validateFormControls(this.formModel.tabs[tabId].group);
            this.formModel.tabs[tabId].group.updateValueAndValidity({ onlySelf: false, emitEvent: false });
            this.formModel.tabs[tabId].valid = !this.formModel.tabs[tabId].group.invalid;

            // todo: check why tabs[tabId].valid is valid with invalid controls!
            if (!this.formModel.tabs[tabId].valid) {
                allErrors = allErrors.concat(this.validatorsService.getAllTabErrors(this.formModel.tabs[tabId]));
            }
        });


        // force to update * in label for required fields
        this.componentCommService.announceValidationModeChange(this.config.validationMode);
        this.config.isManualValidation = false;

        return allErrors.length > 0 ? allErrors : null;
    }

    save(isOnNext?: boolean, finishingType?: number): Promise<any> {


        let validationResult = this.validate(isOnNext, finishingType);

        if (validationResult && validationResult.length > 0) {
            this.logService.warning('Case has validation errors.');
            return Promise.reject({ error: 'validation errors', validations: validationResult });
        }

        if (this.digestService.hasRunningProcesses()) {
            this.logService.warning('@@@ Digest is not complete. Case cannot be saved. @@@');
            return Promise.reject({ error: 'Form actions are not complete. Please try again.', validations: validationResult });
        }

        let authToken = this.storageService.getAuthToken();
        let caseId = this.formModel.proxyModel.formInfo.caseId; // this.storageService.getCaseId();

        // update form fields values
        let fieldsValues = this.formControlsManagerService.getFormFieldValues(this.formModel, this.formData);
        this.formData.ExtendedCaseFieldsValues = new KeyedCollection(fieldsValues.fieldsValues);
        this.formData.CaseFieldsValues = new KeyedCollection(fieldsValues.caseFieldsValues);

        // update form state
        this.formStateService.updateFormState(this.formModel, this.formData.formState);

        return this.loadSaveFormDataService.saveFormData(this.formData, caseId, authToken).pipe(
          take(1),
          catchError(err => {
              return new Promise((resolve, reject) => {
                  return reject(new Error(`Unknown error. ${err.Message}`));
              });
          }),
          switchMap(r => {
              this.formModel.acceptChanges();
              let result = this.processFormDataSaveResult(<FormDataSaveResult>r);
              return of(result);
          })
      ).toPromise();
    }

    trackById(index: any, item: TabTemplateModel) {
        return item.id;
    }

    selectTabFn(tab: any, tabTemplate: TabTemplateModel) {
        this.selectedTabId = tabTemplate.id;
    }

    ///// SECTION event handlers

    private subscribeEvents() {
        this.subscriptionManager.addSingle('populateSectionSubject$',
            this.componentCommService.populateSectionSubject$.subscribe((args) => this.populateSectionHandler(args)));

        this.subscriptionManager.addSingle('enableSectionSubject$',
            this.componentCommService.enableSectionSubject$.subscribe((args) => this.enableSectionHandler(args)));

        this.subscriptionManager.addSingle('addSectionInstanceSubject$',
            this.componentCommService.addSectionInstanceSubject$.subscribe((args) => this.addSectionInstanceHandler(args)));

        this.subscriptionManager.addSingle('deleteSectionInstanceSubject$',
            this.componentCommService.deleteSectionInstanceSubject$.subscribe((args) => this.deleteSectionInstanceHandler(args)));

        this.subscriptionManager.addSingle('digestStatusChanged$',
            this.digestService.digestStatusChanged.subscribe((status) => this.processDigestStatusChanged(status)));
    }

    private enableSectionHandler(args: EnableSectionParams) {
        let isEnabled = args.state;
        let section = args.section;

        this.logService.debugFormatted('Exec enable section action. Section: {0}, State: {1}', section.id, isEnabled);
        section.sectionEnableStateSelection = isEnabled;

        this.runDigest(false);
    }

    private populateSectionHandler(args: PopulateSectionParams) {
        let changedItems: ChangedFieldItem[] = [];
        this.logService.debugFormatted('Exec populating section action. Section: {0}', args.section.id);

        try {
            let values = args.sectionTemplate.populateAction.populateBinding.call(args.section, this.formModel.proxyModel);
            changedItems = this.formControlsManagerService.populateSectionWithValues(args.section, values);
        } catch (err) {
            this.errorHandlingService.handleError(err, `Unknown error on populating section '${args.section.id}'.`);
            return;
        }

        if (changedItems.length > 0) {
            this.logService.debug('Section has been populated. Running digest after fields update.');
            this.runDigestAfterFieldsUpdate(changedItems);
        }
    }

    private addSectionInstanceHandler(args: AddSectionInstanceParams) {
        let sectionModel = args.section;
        let sectionTemplate = sectionModel.template;
        let newSectionInstance: SectionInstanceModel = null;

        this.logService.debugFormatted('Adding new section instance. SectionId: {0}',  args.section.id);
        try {
            newSectionInstance = this.formModelService.addSectionInstance(sectionTemplate, sectionModel, this.formModel.proxyModel);
        } catch (err) {
            this.errorHandlingService.handleError(err, `Failed to create section '${sectionModel.id}'`);
        }

        if (newSectionInstance) {

            // prepare change set for new section fields
            let fieldsChangeSet = this.formControlsManagerService.getSectionFieldsChangeSet(newSectionInstance);

            // run digest with new fields in update log (to load control datasources) but without validation
            this.runDigestAfterFieldsUpdate(fieldsChangeSet, true);

            // setup section validators
            setTimeout(() => {
                this.validatorsService.setupSectionInstanceValidators(newSectionInstance, this.formModel.proxyModel, 0);
                this.componentCommService.announceValidationModeChange(this.config.validationMode);
            }, 0);
        }
    }

    private deleteSectionInstanceHandler(args: DeleteSectionInstanceParams) {
        let hasRemoved = false;
        try {
            hasRemoved = this.formModelService.removeSectionInstance(args.sectionInstance, this.formModel);
        } catch (err) {
            this.errorHandlingService.handleError(`Failed to delete section instance (id=${args.sectionInstance.id}).`);
        }

        if (hasRemoved) {
            this.runDigest(false);
        }
    }

    ///// END OF Section events handlers

    private openCase(caseValues?: IMap<FieldValueModel>) {

        let caseValuesKeyedCollection = new KeyedCollection<FieldValueModel>();
        caseValuesKeyedCollection.init(caseValues);

        if (caseValues !== undefined) {
          this.initCaseFiles(caseValues);
        }

        if (this.formParameters.extendedCaseGuid && this.formParameters.extendedCaseGuid.length > 0) {

            let authToken = this.storageService.getAuthToken();
            let helpdeskCaseId = this.formParameters.caseId; // this.storageService.getCaseId();

            this.subscriptionManager.addSingle('loadFormData',
                this.loadSaveFormDataService.loadFormData(this.formParameters.extendedCaseGuid, helpdeskCaseId, authToken)
                .subscribe(data => {
                        this.formData = data;

                        if (caseValues) {
                          this.formData.CaseFieldsValues = caseValuesKeyedCollection;
                        }

                        this.loadMetaData();
                    },
                    err => {
                        // todo: handle auth token expiration error...
                        this.errorHandlingService
                            .handleError(err, `Failed to load case data for extendedCaseGuid=${this.formParameters.extendedCaseGuid}.`);
                    }));
        } else {
            // load metdata only, since it is a new extended case
            if (caseValues) {
                this.formData.CaseFieldsValues = caseValuesKeyedCollection;
            }

            this.loadMetaData();
        }
    }

    private initCaseFiles(caseValues: IMap<FieldValueModel>) {
      // This method now used only on initial open, but not on syncronization. If update of files required on sync action use it also in doUpdateCaseFieldValues
      if (caseValues.case_files != null) {
        let caseValuesObj = null;
        try {
          caseValuesObj = JSON.parse(caseValues.case_files.Value) as Array<{ Id: number, FileName: string }>;
        } catch (error) {
          this.logService.debug('CaseFiles: case_files is not valid JSON. leaving null')
        }
        if (caseValuesObj != null) {
          this.formParameters.caseFiles = caseValuesObj.map(cv => new CaseFileModel(cv.Id, cv.FileName));
        } else {
          this.formParameters.caseFiles = new Array<CaseFileModel>();
        }
      }
      if (caseValues.whiteFilesList != null) {
        let whiteFilesListObj = null;
        try {
          whiteFilesListObj = JSON.parse(caseValues.whiteFilesList.Value) as Array<string>;
        } catch (error) {
          this.logService.debug('whiteFilesList: whiteFilesList is not valid JSON. leaving null')
        }
        this.formParameters.whiteFilesList = whiteFilesListObj != null ? whiteFilesListObj : null;
      }
    }

    private loadMetaData() {
        try {
            let metaDataLoadStrategy = this.getMetaDataLoadStrategy();

            this.subscriptionManager.addSingle('metaDataLoadStrategy',
                metaDataLoadStrategy()
                    .subscribe(
                        (metaData: FormMetaDataResponse) => {
                            let res = this.processFormMetaDataResponse(metaData);
                            if (!res) {
                                this.handleCaseLoadError(null, 'Unknown error on metadata processing');
                            }
                        },
                        err => this.handleCaseLoadError(err, 'Failed to load form meta data.')));

        } catch (err) {
            this.handleCaseLoadError(err, 'Unknown error on loading metadata');
        }
    }

    private getMetaDataLoadStrategy(): () => Observable<any> {
        if (!isNaN(this.formParameters.formId)) {
            return () => this.metaDataService.getMetaDataById(this.formParameters.formId, this.formParameters.languageId);
        } else if (this.formParameters.assignmentParameters) {
            return () => this.metaDataService.getMetaDataByAssignment(this.formParameters.assignmentParameters);
        } else if (this.formData) {
            return () => this.metaDataService.getMetaDataById(this.formData.ExtendedCaseFormId, this.formParameters.languageId);
        } else {
            throw new Error('Failed to resolve loadMetaData strategy.');
        }
    }

    private processFormMetaDataResponse(response: FormMetaDataResponse): boolean {
        try {
            this.formData.ExtendedCaseFormId = response.Id;
            const correctedMetadata = response.MetaData.replace(/[\\]+/ig, '\\\\'); // eval removes single backslash(\) symbol, duplicate to avoid it
            const metaData = eval(`(${correctedMetadata})`);
            this.logService.debug('Metadata has been parsed.');

            this.setupSettings(metaData);
            this.setupModels(metaData);

        } catch (err) {
            this.errorHandlingService.handleError(err, 'Unknown error on meta data processing.');
            return false;
        }

        // bind form data only if it was loaded
        if (this.formParameters.extendedCaseGuid && this.formParameters.extendedCaseGuid.length > 0) {
            try {
                // preparate formData
                this.setFormData();
            } catch (err) {
                this.errorHandlingService.handleError(err, 'Unknown error on setting form data.');
                return false;
            }
        }

        // check if initial data has been provided by external client (helpdesk)
        if (this.initialData) {
            this.setStepInner(this.initialData.step);
            this.setValidationModeInner(this.initialData.isNextValidation);
        }

        // run initial digest to calc form values
        this.runDigest(true, true);
        return true;
    }

    private setFormData() {
        if (this.formData) {
            try {
                this.proxyModelService.createCaseData(this.formModel.proxyModel, this.formData.CaseFieldsValues);
                this.formControlsManagerService.setFormData(this.formData, this.formModel);

                // set formState
                // this.formModel.formState = this.formData.formState;

                this.formStateService.applyFormState(this.formModel, this.formData.formState);

            } catch (err) {
                this.errorHandlingService.handleError(err, 'Unknown error on setting form data.');
            }
        }
    }

    private runDigest(initial: boolean, forceLoadDatasources?: boolean) {
        this.logService.debugFormatted('[!]runDigest. initial: {0}, forceLoadDataSources: {1}', initial, forceLoadDatasources);
        this.changeDetector.detach();

        this.subscriptionManager.addSingle('runDigest_digestService',
            this.digestService.runDigest(this.templateModel, this.formModel, null, forceLoadDatasources)
                .subscribe((result: DigestResult) => {
                    let resultContext = DigestResultContext.create(initial, result);
                    this.processDigestComplete(resultContext);
                }));
    }

    // method is required to run digest after multiple fields have been updated with prefilled updateLog with changes.
    private runDigestAfterFieldsUpdate(changedFields: ChangedFieldItem[], isNewFields = false) {
        this.logService.debugFormatted('[!]runDigestAfterFieldsUpdate. Changed fields: {0}', changedFields);
        this.changeDetector.detach();

        this.subscriptionManager.addSingle('runDigestAfterFieldsUpdate_digestService',
            this.digestService.runDigestAfterFieldsUpdate(this.templateModel, this.formModel, changedFields, isNewFields)
                .subscribe((result: DigestResult) => {
                    let resultContext = DigestResultContext.create(false, result, changedFields, isNewFields);
                    this.processDigestComplete(resultContext);
            }));
    }

    private updateNewSectionInstanceValidators(changedFields: ChangedFieldItem[]) {
        let fieldsIterator = this.formModel.createFieldsIterator();
        fieldsIterator.forEach((fieldModel, fieldPath) => {
            if (changedFields.findIndex((changedFieldItem: ChangedFieldItem) => changedFieldItem.fieldPath.equals(fieldPath)) !== -1) {
                this.logService.debug('[@]Updating validator for field: ' + fieldPath.buildFormFieldPath());
                this.validatorsService.updateIsRequired(fieldModel, this.formModel.proxyModel);
            }
        });
    }

    private subscribeValueChangedEvents() {
        this.subscriptionManager.addSingle('controlValueChanged$',
            this.componentCommService.controlValueChanged$
                .subscribe((params: ControlValueChangedParams) => {
                    // to prevent digest start if value hasn't changed actually, for instance when dropdown options are updated, formControl fires changes event with the same value

                    // set model current value to newValue
                    this.logService.debugFormatted('subscribeValueChangedEvents: changing model current value. FieldId: {0}. NewValue: {1}', params.formModel.id, params.value);
                    let fieldModel = params.formModel;
                    fieldModel.setLastValue(params.formControl.value);

                    if (params && params.control.processControlDataSourcesOnly) { // for search control
                        this.logService.info('controlValueChanged$: run processDataSourcesForControl');
                        this.formControlsManagerService
                            .processDataSourcesForControl(this.formModel, params.control, params.formModel)
                            .subscribe(() => this.logService.info('processDataSourcesForControl: end'));
                    } else {
                        this.logService.infoFormatted('controlValueChanged$: run digest() FormControl: {0}', params.control.id);
                        this.subscriptionManager.addSingle('digestService',
                            this.digestService.runDigest(this.templateModel, this.formModel, params)
                                .subscribe((result: DigestResult) => {
                                    let resultContext = DigestResultContext.create(false, result);
                                    this.processDigestComplete(resultContext);
                                }));
                    }
                }));
    }

    private processDigestComplete(resultContext: DigestResultContext) {

        let digestResult = resultContext.result;
        this.logService.debugFormatted('processing DigestComplete event. Result: {0}', digestResult.result);

        if (resultContext.isInitial) {
            this.subscribeValueChangedEvents();
            setTimeout(() => {
                this.validatorsService.setupValidators(this.formModel, 0);
                this.componentCommService.announceValidationModeChange(this.config.validationMode);
            }, 0);
            this.isLoaded = true;
        }

        if (resultContext.isNewFields) {
            let changedFields = resultContext.changedFields;
            if (changedFields && changedFields.length) {
                this.updateNewSectionInstanceValidators(changedFields);
            }
        } else {
            this.runDigestValidation(digestResult);
        }

        this.logService.debug('Processing digest result. Run digest complete event');
        this.changeDetector.reattach();
        // notify other components interested in digest complete event
        this.componentCommService.raiseDigestCompleted(digestResult);
        setTimeout(() => this.changeDetector.detectChanges(), 0);

        // raise complete event for external clients (ex.: helpdesk website)
        if (resultContext.isInitial) {
            this.logService.debug('[EVENT] Complete event!!!');
            this.caseLoadCompleteSubject.next({ success: true });
        }
    }

    private processDigestCompleteOld(initial: boolean, result: DigestResult, runValidation: boolean = true) {
        this.logService.debugFormatted('processing DigestComplete event. Result: {0}', result.result);

        if (runValidation) {
            this.runDigestValidation(result);
        }

        this.logService.debug('Processing digest result. Run digest complete event');
        this.changeDetector.detectChanges();
        this.changeDetector.reattach();
        // notify other components interested in digest complete event
        this.componentCommService.raiseDigestCompleted(result);

        // raise complete event for external clients (ex.: helpdesk website)
        if (initial) {
            this.logService.debug('[EVENT] Complete event!!!');
            this.caseLoadCompleteSubject.next({ success: true });
        }
    }

    private processDigestStatusChanged(status: boolean) {
        if (status) {
            this.progressComponent.show();
        } else {
            this.progressComponent.hide();
        }
    }

    private runDigestValidation(result: DigestResult): any {
        const controlsToValidate = new Array<AbstractControl>();
        if (result.result && result.digestUpdateLog) {

            // revalidate only updated controls
            result.digestUpdateLog.logs.forEach((log: DigestUpdateLogItem) => {
                let fieldModel = this.formModel.findFormField(log.fieldPath);
                if (fieldModel) {
                    controlsToValidate.push(fieldModel.getControlGroup());
                    this.formControlsManagerService.processFieldWarnings(log.fieldPath, this.formModel, result.digestUpdateLog);
                }
            });

            let fieldsIterator = this.formModel.createFieldsIterator();
            fieldsIterator.forEach((fieldModel, fieldPath) => {
                let ctrl = fieldModel.getControlGroup();
                this.validatorsService.updateIsRequired(fieldModel, this.formModel.proxyModel);

                if ((ctrl.dirty || fieldModel.template.isEverValidated) && controlsToValidate.indexOf(ctrl) < 0) {
                    controlsToValidate.push(ctrl);
                }
            });

            // process validations
            this.validatorsService.updateFieldsValidationStatus(controlsToValidate);

            this.validatorsService.updateTabsValidationStatus(this.formModel);

            // force to update * in label for required fields
            this.componentCommService.announceValidationModeChange(this.config.validationMode);
        }
    }

    private setupSettings(metaData: any): void {
        if (metaData.localization) {
            if (metaData.localization.dateFormat) {
                this.config.dateFormat = metaData.localization.dateFormat;
            }
            if (metaData.localization.decimalSeparator) {
                this.config.decimalSeparator = metaData.localization.decimalSeparator;
            }
        }
    }

    private setupModels(metaData: any): void {


        this.logService.debug('setupModels');
        this.ngZone.runOutsideAngular(() => {
            this.templateModel = this.templateService.toTemplateModel(metaData);


            this.formModel = this.formModelService.buildForm(this.templateModel, new FormInfo(this.formParameters));
            this.selectedTabId = metaData.tabs[0].id;
        });

        this.applyStyles();
    }

    private applyStyles() {
        this.ngZone.runOutsideAngular(() => {
            let head = document.head || document.getElementsByTagName('head')[0],
                style = document.createElement('style');

            style.type = 'text/css';
            style.appendChild(document.createTextNode(this.templateModel.styles));

            head.appendChild(style);

            this.ngZone.run(() => { this.logService.info('Outside Done!') });
        });
    }

    private processFormDataSaveResult(saveResult: FormDataSaveResult): any {

        // set form data values
        this.formData.Id = parseInt(saveResult.Id);
        this.formData.ExtendedCaseGuid = saveResult.ExtendedCaseGuid;
        this.formData.ExtendedCaseFormId = parseInt(saveResult.ExtendedCaseFormId);

        // set form parameters
        this.formParameters.extendedCaseGuid = this.formData.ExtendedCaseGuid;
        this.proxyModelService.createCaseData(this.formModel.proxyModel, this.formData.CaseFieldsValues);

        return { extendedCaseGuid: saveResult.ExtendedCaseGuid };
    }

    private handleCaseLoadError(err: any, msg: string) {
        let errorMsg = msg || 'Unknown error on extended case loading';
        this.caseLoadCompleteSubject.error(new Error(errorMsg));

        // handle error with handler only in case its an unhandled system error, otherwise it should be handled by the called method
        if (err && err instanceof Error) {
            this.errorHandlingService.handleError(err, errorMsg);
        }
    }

    private setStepInner(step: string) {
        this.formModel.proxyModel.nextStep = step;
    }

    private setValidationModeInner(isNextValidation?: boolean) {
        if (isNextValidation !== undefined && isNextValidation != null) {
            this.config.validationMode = isNextValidation ? ValidateOn.OnNext : ValidateOn.OnSave;
        }
    }

    test(): void {
        // this.toggleProgress = !this.toggleProgress;

        // if (this.toggleProgress) {
        //     this.progressComponent.show();
        // } else {
        //     this.progressComponent.hide();
        // }

        let f = this.formModel.tabs['tab1'].sections['sec1'].instances[0].fields['select1'];
        f.items = [{ value: '1', text: '11' }, { value: '2', text: '22' }, { value: '3', text: '33' }];

        f.setControlValue('1');


        /*let promise = new Promise((resolve, reject) => {
            setTimeout(() => {
                resolve({ Success: true, UniqueId: '11B2EE2C-79F1-4F13-9F00-71572702E5CE' });
            }, 1000);
        });
        return promise;*/
    }

    do() {

        throw new Error('this is a test');
        // test
        // this.errorHandlingService.handleError(new Error('Unknown error'), 'User error message');

        // this.setNextStep('5', true);
    }
}


interface ICaseInitialData {
    step: string;
    isNextValidation?: boolean;
}
