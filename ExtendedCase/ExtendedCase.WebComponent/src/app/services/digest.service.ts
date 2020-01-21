import { throwError, forkJoin, of, Observable, Subject } from 'rxjs';
import { mergeMap, catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { DigestUpdateLog, DigestResult } from '../models/digest.model';
import { ControlValueChangedParams } from './component-comm.service';
import { FormTemplateModel, TabTemplateModel, BaseControlTemplateModel,
    CustomQueryDataSourceTemplateModel,  OptionsDataSourceTemplateModel, ControlCustomDataSourceTemplateModel,
    ControlSectionDataSourceTemplateModel, SectionType, IDataSourceParameter} from '../models/template.model';
import { TabModel, FormModel, SectionModel, SectionInstanceModel, SingleControlFieldModel, FieldModelBase, FormControlType } from '../models/form.model';
import { FormModelService } from './form-model.service';
import { TemplateService } from './template.service';
import { FormControlsManagerService } from './form-controls-manager.service';
import { DataSourcesLoaderService } from './datasources-loader.service';
import { FormFieldPathModel } from '../models/form-field-path.model';
import { ProxyModel, ProxySectionInstance, ProxyControl } from '../models/proxy.model';
import { UuidGenerator } from '../utils/uuid-generator'
import * as commonMethods from '../utils/common-methods';
import { ErrorHandlingService } from './error-handling.service';
import { LogService } from './log.service'
import { SubscriptionManager } from '../shared/subscription-manager';
import { ChangedFieldItem } from '../shared/common-types';
import { KeyedCollection } from '../shared/keyed-collection';

@Injectable()
export class DigestService {

    private _processes = new KeyedCollection<DigestResult>();

    private digestStatusChangedSubject: Subject<boolean>;
    digestStatusChanged: Observable<boolean>;

    constructor(
        private formModelService: FormModelService,
        private templateService: TemplateService,
        private dataSourcesLoaderService: DataSourcesLoaderService,
        private formControlsManagerService: FormControlsManagerService,
        private errorHandlingService: ErrorHandlingService,
        private logService: LogService) {

        this.digestStatusChangedSubject = new Subject<boolean>();
        this.digestStatusChanged = this.digestStatusChangedSubject.asObservable();
    }

    runDigest(
        templateModel: FormTemplateModel,
        formModel: FormModel,
        params: ControlValueChangedParams,
        forceLoadDataSources?: boolean): Observable<DigestResult> {

        let process =
            new DigestProcess(templateModel, formModel, params,
                this.formModelService, this.templateService, this.dataSourcesLoaderService,
                this.formControlsManagerService, this.errorHandlingService,
                this.logService, forceLoadDataSources);

        return this.runProcess(process);
    }

    runDigestAfterFieldsUpdate(
        templateModel: FormTemplateModel,
        formModel: FormModel,
        changedFields: ChangedFieldItem[],
        isNewFields: boolean = false): Observable<DigestResult> {

        let process =
            new DigestProcess(templateModel, formModel, null,
                this.formModelService, this.templateService, this.dataSourcesLoaderService,
                this.formControlsManagerService,
                this.errorHandlingService,
                this.logService,
                false);

         // process new section fields
        if (isNewFields) {
            process.setNewFields(changedFields.map(item => item.fieldPath));
        }

        process.initUpdateLog(changedFields);

        return this.runProcess(process);
    }

    hasRunningProcesses() {
        let runningIds = this.getRunningProcesses();
        return runningIds.length > 0;
    }

    private runProcess(process: DigestProcess): Observable<DigestResult> {
        let runningIds = this.getRunningProcesses();

        if (runningIds.length > 0) {
            let val = runningIds.join(',');
            this.logService.warningFormatted('@@@ Digest has running processes: {0}', val);
        }

        this.logService.debugFormatted('@@@ Running new digest process. Existing: {0}', runningIds.length);
        let processId = process.getProcessId();
        this._processes.add(processId, null);

        let result$ = process.runDigestProcess();

        this.raiseDigestStatusChanged(true);

        result$.subscribe(res => {
            this._processes.setItemSafe(processId, res);
            let status = this.hasRunningProcesses();
            // this.logService.debugFormatted('@@@ Digest completed. Has processes: {0} @@@', status);
            this.raiseDigestStatusChanged(status);
        });

        return result$;
    }

    private raiseDigestStatusChanged(res: boolean) {
        this.digestStatusChangedSubject.next(res);
    }

    private getRunningProcesses(): string[] {
        let processIds: string[] = [];

        let entries = this._processes.getEntries();

        for (let key of Object.keys(entries)) {
            if (!entries[key]) {
                processIds.push(key);
            }
        }

        return processIds;
    }
}

class DigestProcess {
    private digestMaxCount = 5000;
    private digestCounter = 0;

    private digestUpdateLog: DigestUpdateLog;
    private parametersUpdateLog: DigestUpdateLog;
    private runCyclesCount = 0;
    private processId:string;
    private processingCompleteSubject: Subject<DigestResult>;
    private subscriptionManager = new SubscriptionManager();
    private newFields: FormFieldPathModel[] = [];

    constructor(private templateModel: FormTemplateModel,
        private formModel: FormModel,
        private params: ControlValueChangedParams,
        private formModelService: FormModelService,
        private templateService: TemplateService,
        private dataSourcesLoaderService: DataSourcesLoaderService,
        private formControlsManagerService: FormControlsManagerService,
        private errorHandlingService: ErrorHandlingService,
        private logService: LogService,
        private forceLoadDataSources?: boolean) {

        this.digestUpdateLog = new DigestUpdateLog();
        this.parametersUpdateLog = new DigestUpdateLog();

        this.processId = UuidGenerator.createUuid();
        this.processingCompleteSubject = new Subject<DigestResult>();
    }

    getProcessId() {
        return this.processId;
    }

    runDigestProcess(): Observable<DigestResult> {
        if (this.params && this.params.control.noDigest) {
            this.logDebug('digest not started, control has noDigest');
            return of(new DigestResult(true, this.digestUpdateLog));
        }

        setTimeout(() => { // run cycle after return to avoid subject next() before any subscription;
            this.runCycle(true);
        }, 0);

        return this.processingCompleteSubject.asObservable();
    }

    private runCycle(firstRun: boolean) {
        if (this.digestCounter >= this.digestMaxCount) {
            this.logWarning(`Digest failed to complete in ${this.digestCounter} cycles`);
        }

        if (this.runCyclesCount > 0) {
            // clean parameters change log on each subsequent run
            this.parametersUpdateLog.clear();
        }

        this.runCyclesCount += 1;
        this.logDebug(`@@@ Running new digest cycle: ${this.runCyclesCount}`);

        // reset initial flags if recursive call
        if (firstRun === false) {
            this.forceLoadDataSources = false;
            this.newFields = [];
        }

        // process params only on the firstRun
        if (this.params && firstRun) {
            this.saveUpdateLog(this.params.formModel.getFieldPath(), null, this.params.value);
        }

        let hasChanges = false;

        this.processElements(firstRun);

        // chain processings of data sources
        this.subscriptionManager.addSingle('processCustomDataSources',
            this.processCustomDataSources().pipe(
                catchError((error: any) => {
                    console.error(error);
                    return throwError(error);
                }),
                mergeMap((x: boolean[]) => {
                    this.logService.infoFormatted('processCustomDataSources results: {0}', x);
                    if (x.indexOf(true) > -1) {
                        hasChanges = true;
                    }

                    return this.processControlsDataSources();
                }),)
                .subscribe((x: boolean[]) => {
                    this.logService.infoFormatted('processControlsDataSources results: {0}', x);
                    if (x.indexOf(true) > -1) {
                            hasChanges = true;
                        }

                        // run if there were any changes
                        if (hasChanges) {
                            this.logInfo('datasources returned hasChanges: true, run cycle()');
                            this.runCycle(false);
                        } else {
                            this.logInfo(`Digest completed with SUCCESS: ${this.digestCounter} cycles`);
                            this.processingCompleteSubject.next(new DigestResult(true, this.digestUpdateLog));
                        }
                    },
                    (error: any) => {
                            this.errorHandlingService.handleError(error, `Controls dataSources processing failed with error`);
                            this.logWarning(`Digest completed with Error: ${this.digestCounter} cycles`);
                            this.processingCompleteSubject.next(new DigestResult(false, this.digestUpdateLog));
                    },
                    () => {
                        this.logInfo('dataSources processing complete!');
                    }));
    }

    setNewFields(fields: FormFieldPathModel[]) {
        this.newFields = fields;
    }

    initUpdateLog(fields: ChangedFieldItem[]) {
        if (fields && fields.length) {
            this.logService.debugFormatted('Init digest update log with fields: {0}', fields);
            fields.forEach((item: ChangedFieldItem) =>
                this.saveUpdateLog(item.fieldPath, item.oldValue, item.newValue));
        }
    }

    private saveUpdateLog(fieldPath: FormFieldPathModel, oldValue: any, newValue: any) {
        this.parametersUpdateLog.add(fieldPath, oldValue, newValue);
        this.digestUpdateLog.add(fieldPath, oldValue, newValue);
        this.logDebug(`Digest update log changed: Path=${fieldPath.buildFormFieldPath()}, OldValue = ${oldValue}, NewValue = ${newValue}`);
    }

    private processElements(firstRun: boolean) {

        this.logDebug(`processElements. firstRun: ${firstRun}`);
        let lastRun = false;
        let iteration = -1;

        while (this.digestCounter < this.digestMaxCount) {
            iteration++;
            this.digestCounter++;
            let modelChanged = false;

            this.logDebug(`@@@ new digest iteration: ${iteration}. Cycle: ${this.runCyclesCount}`);

            for (let tabId of Object.keys(this.formModel.tabs)) {
                let tabModel = this.formModel.tabs[tabId];

                modelChanged = this.processTabBindings(tabModel);

                // sections
                for (let sectionId of Object.keys(tabModel.sections)) {
                    let sectionModel = tabModel.sections[sectionId];

                    let isSectionChanged = this.processSectionBindings(sectionModel);
                    modelChanged = isSectionChanged ? true : modelChanged;

                    // process sectionInstances controls
                    let sectionInstanceIndex = -1;
                    for (let sectionInstance of sectionModel.instances) {
                        sectionInstanceIndex++;

                        let proxySectionInstance =
                            this.formModel.proxyModel.tabs[tabId].sections[sectionId].instances
                                .find((item: ProxySectionInstance) => item.uniqueId === sectionInstance.id);

                        for (let fieldKey of Object.keys(sectionInstance.fields)) {
                            let fieldModel = sectionInstance.fields[fieldKey];

                            let isControlChanged =
                                this.processControlBindings(sectionInstanceIndex, sectionInstance, proxySectionInstance, fieldModel);

                            modelChanged = isControlChanged ? true : modelChanged;
                        }

                        this.updateReviewSectionVisibility(sectionInstance);
                    }
                }
            }

            if (!modelChanged) {
                if (!lastRun) { // add final digest to avoid valueBinding, disableBinding order bug.
                    lastRun = true;
                    this.logDebug('Run last digest iteration.');
                    continue;
                }
                break; // exit loop if it was last run and model was not changed
            } else {
                lastRun = false;
            }
        }
    }
    private updateReviewSectionVisibility(sectionInstance: SectionInstanceModel) {

        let sectionModel = sectionInstance.section;
        let sectionTemplate = sectionModel.template;

        if (sectionTemplate.hasReview() && sectionTemplate.type !== SectionType.review) {

            // set up review control visibility: hide if both values are empty
            let allHidden = true;
            for (let fieldKey of Object.keys(sectionInstance.fields)) {

                let fieldModel = sectionInstance.fields[fieldKey];
                let values = this.formControlsManagerService.getReviewControlStringValues(<SingleControlFieldModel>fieldModel);
                let isHidden = fieldModel.hidden || (commonMethods.isUndefinedNullOrEmpty(values[0]) && commonMethods.isUndefinedNullOrEmpty(values[1]));
                fieldModel.hidden = isHidden;

                if (!fieldModel.hidden) {
                  allHidden = false;
                }
            }
            sectionInstance.hidden = allHidden;
        }
    }
    private processTabBindings(tabModel: TabModel): boolean {
        let modelChanged = false;

        let tab: TabTemplateModel = tabModel.template;

        if (tab.hiddenBinding instanceof Function) {
            this.logDebug(`hiddenBinding called for ${tab.id}`);
            let newValue = tab.hiddenBinding.call(this.formModel.proxyModel.tabs[tab.id], this.formModel.proxyModel, this.digestUpdateLog);
            if (tabModel.hidden !== newValue) {
                tabModel.hidden = newValue;
                modelChanged = true;
            }
        }

        let disabledValue = tabModel.disabled;
        if (this.formModel.proxyModel.formInfo.isCaseLocked) {
            disabledValue = true;
        } else if (tab.disabledBinding instanceof Function) {
            disabledValue = tab.disabledBinding.call(this.formModel.proxyModel.tabs[tab.id], this.formModel.proxyModel, this.digestUpdateLog);
        }

        // check if disabled value changed
        if (tabModel.disabled !== disabledValue) {
            this.logService.debugFormatted('disabledBinding changed for tab: ' + tab.id);
            tabModel.disabled = disabledValue;
            modelChanged = true;
        }

        return modelChanged;
    }

    private processSectionBindings(sectionModel: SectionModel): boolean {
        let modelChanged = false;
        let tab = sectionModel.tab.template;
        let section = sectionModel.template;
        let sectionProxy = this.formModel.proxyModel.tabs[tab.id].sections[section.id];

        {
            let isHidden = sectionModel.tab.hidden;
            if (!isHidden && section.hiddenBinding instanceof Function) {
                this.logInfo(`hiddenBinding called for ${section.id}`);
                isHidden = section.hiddenBinding.call(sectionProxy, this.formModel.proxyModel, this.digestUpdateLog);
            }

            if (sectionModel.hidden !== isHidden) {
                sectionModel.hidden = isHidden;

                // set hidden for all section instances
                for (let sectionInstance of sectionModel.instances) {
                    sectionInstance.hidden = isHidden;
                }

                modelChanged = true;
            }
        }

        {
            // set tab disableBinding as default value
            let isDisabled = this.formModel.tabs[tab.id].disabled;

            if (!isDisabled && section.disabledBinding instanceof Function) {
                this.logDebug(`call disabledBinding for ${section.id}. `);
                isDisabled = section.disabledBinding.call(sectionProxy, this.formModel.proxyModel, this.digestUpdateLog);
            }

            if (sectionModel.disabled !== isDisabled) {
                sectionModel.disabled = isDisabled;
                modelChanged = true;
            }

            // process section instances
            for (let sectionInstance of sectionModel.instances) {
                let sectionInstanceDisabled = isDisabled;

                let changedByUser = false;

                // check user selection only if a template has enableAction set
                if (!sectionInstanceDisabled && section.enableAction) {
                    sectionInstanceDisabled = !sectionInstance.sectionEnableStateSelection;
                    changedByUser = sectionInstance.disabled !== sectionInstanceDisabled;
                }

                if (sectionInstance.disabled !== sectionInstanceDisabled) {
                    const prevSectionDisabled = sectionInstance.disabled;
                    this.logDebug(`sectionInstance.disabled changed for ${section.id}. OldValue: ${prevSectionDisabled}, NewValue: ${isDisabled}`);
                    sectionInstance.disabled = sectionInstanceDisabled;

                    if (sectionInstance.disabled) {
                        const changedItems =
                            this.formControlsManagerService.resetDisabledSectionState(sectionInstance, changedByUser);

                        if (changedItems && changedItems.length) {
                            this.logService.debugFormatted('[~]Disabled section fields: {0}', changedItems);
                            for (let item of changedItems) {
                                this.parametersUpdateLog.add(item.fieldPath, item.oldValue, item.newValue);
                            }
                        }
                    }

                    // keep user selection in sync with disabled binding - reset to false only if disabled but true is to be set only by clicking checkbox on ui
                    if (section.enableAction && sectionInstance.disabled) {
                        sectionInstance.sectionEnableStateSelection = false;
                    }

                    modelChanged = true;
                }
            }
        }

        return modelChanged;
    }

    private processControlBindings(sectionIndex: number, sectionInstance: SectionInstanceModel, proxySectionInstance: ProxySectionInstance,
                                   fieldModel: FieldModelBase): boolean {
        let modelChanged = false;
        this.logInfo(`processControlBindings for field: '${fieldModel.id}'`);

        let sectionTpl = sectionInstance.section.template;
        let tabTpl = sectionTpl.tab;
        let controlTpl = fieldModel.template;

        let proxyControl = proxySectionInstance.controls[controlTpl.id];
        let fieldPath = new FormFieldPathModel(tabTpl.id, sectionTpl.id, sectionIndex, fieldModel.id);

        if (!this.params || this.params.formModel !== fieldModel) {

            let res1 = this.processValueBinding(proxyControl, controlTpl, sectionInstance, fieldPath);
            let res2 = this.processHiddenBinding(proxyControl, controlTpl, sectionInstance);
            let res3 = this.processDisabledBinding(proxyControl, controlTpl, sectionInstance);
            let res4 = controlTpl.controlType === FormControlType.Search ?
             this.processSearchControlBinding(proxyControl, controlTpl, sectionInstance) :
             false;
            this.logService.debugFormatted('processControlBindings result : {0}, {1}, {2}, {3}', res1, res2, res3, res4);

            modelChanged = res1 || res2 || res3 || res4;

            // run setControlDataSourceOptions only for datasources with static data. In other cases setControlDataSourceOptions is
            // called later after data is recieved.
            if (controlTpl.dataSourceFilterBinding instanceof Function && controlTpl.dataSource instanceof Array) {
                let hasChanged =
                    this.formControlsManagerService.setControlDataSourceOptions(
                        this.formModel.proxyModel,
                        fieldModel,
                        controlTpl,
                        this.digestUpdateLog);

                modelChanged = hasChanged || modelChanged;
            }
        }

        if (modelChanged) {
            this.logInfo(`processControlBindings: modelChanged in control ${controlTpl.id}`);
        }

        // ignore model changes for review controls
        if (fieldModel.isReview) {
            return false;
        }

        return modelChanged;
    }

    private processValueBinding(proxyControl: ProxyControl, controlTemplate: BaseControlTemplateModel, sectionInstance: SectionInstanceModel, fieldPath:FormFieldPathModel): boolean {
        let hasChanged = false;

        if (controlTemplate.valueBinding instanceof Function) {
            let fieldModel = sectionInstance.fields[controlTemplate.id];
            this.logDebug(`exec valueBinding() for ${controlTemplate.id}`);

            // todo: valueBinding is implemented only for SingleControl?
            if (fieldModel instanceof SingleControlFieldModel) {

                let newValue: any;

                try {
                    newValue = controlTemplate.valueBinding.call(proxyControl, this.formModel.proxyModel, this.digestUpdateLog);
                    this.logDebug(`valueBinding() called for ${controlTemplate.id}. Value: ${JSON.stringify(newValue)}`);

                } catch (err) {
                    this.errorHandlingService.handleError(`Unknown error while executing valueBinding function for control '${fieldModel.getFieldPath().buildFormFieldPath()}'`);
                    return false;
                }

                if (newValue === undefined) {
                    return false;
                }

                // fix value to be same as control
                newValue = this.formModelService.fixValueForControl(newValue, fieldModel);

                let areEqual =
                    commonMethods.areEqualsDeep(fieldModel.control.value, newValue) ||
                    commonMethods.areUndefinedNullOrEmpty(fieldModel.control.value, newValue);

                if (!areEqual) {
                    // ignore review control changes
                    if (!fieldModel.isReview) {
                        this.logDebug(`valueBinding() returned a different value. Change control value to new. ControlId: ${controlTemplate.id}, NewValue: ${JSON.stringify(newValue)}`);
                        this.saveUpdateLog(fieldPath, fieldModel.control.value, newValue);
                        hasChanged = true;
                    }

                    this.formModelService.setFieldValue(fieldModel, newValue);
                }
            }
        }

        return hasChanged;
    }

    private processDisabledBinding(proxyControl: ProxyControl, controlTemplate: BaseControlTemplateModel,
                                   sectionInstance: SectionInstanceModel): boolean {
        let hasChanged = false;
        let fieldModel: FieldModelBase = sectionInstance.fields[controlTemplate.id];
        let isDisabled = sectionInstance.disabled;

        if (!isDisabled && controlTemplate.disabledBinding instanceof Function) {
            this.logDebug(`exec disabledBinding() for ${controlTemplate.id}`);
            isDisabled = controlTemplate.disabledBinding.call(proxyControl, this.formModel.proxyModel, this.digestUpdateLog);
        }

        // enable/disable controls
        hasChanged = this.formModelService.setDisabled(fieldModel, isDisabled);
        return hasChanged;
    }

    private processHiddenBinding(proxyControl: ProxyControl, controlTemplate: BaseControlTemplateModel,
                                   sectionModel: SectionInstanceModel): boolean {
        let hasChanged = false;
        let fieldModel = sectionModel.fields[controlTemplate.id];
        let isHidden = sectionModel.hidden;
        if (!isHidden && controlTemplate.hiddenBinding instanceof Function) {
            this.logDebug(`exec hiddenBinding() for ${controlTemplate.id}`);
            isHidden = controlTemplate.hiddenBinding.call(proxyControl, this.formModel.proxyModel, this.digestUpdateLog);
        }

        if (fieldModel.hidden !== isHidden) {
            fieldModel.hidden = isHidden;
            hasChanged = true;
        }

        return hasChanged;
    }

    private processSearchControlBinding(proxyControl: ProxyControl, controlTemplate: BaseControlTemplateModel,
                                        sectionModel: SectionInstanceModel) {
        let hasChanged = false;
        let fieldModel = sectionModel.fields[controlTemplate.id];
        let showSearchResults = true;
        if (controlTemplate.showSearchResultsBinding instanceof Function) {
          this.logDebug(`exec showSearchResultsBinding() for ${controlTemplate.id}`);
          showSearchResults = controlTemplate.showSearchResultsBinding.call(proxyControl, this.formModel.proxyModel, this.digestUpdateLog);
        }

        if (fieldModel.showSearchResults !== showSearchResults) {
            fieldModel.showSearchResults = showSearchResults;
            hasChanged = true;
        }

        return hasChanged;
      }

    /// DATASOURCES
    private processCustomDataSources(): Observable<Array<boolean>> {
        let dsObservation: Observable<boolean>[] = [];
        this.logInfo('processCustomDataSources: begin');
        for (let dsTemplate of this.templateModel.dataSources) {
            if (dsTemplate instanceof CustomQueryDataSourceTemplateModel) {

                let requiresUpdate =
                    this.forceLoadDataSources ||
                    this.checkIfParameterHasChanged(dsTemplate.parameters);

                if (requiresUpdate) {
                    this.logInfo(`processCustomDataSource: dataSourceId: ${dsTemplate.id} requires update. force: ${this.forceLoadDataSources}`);
                    let result$ =
                        this.processCustomDataSource(this.formModel,
                            dsTemplate.id,
                            dsTemplate.parameters,
                            this.formModel.proxyModel);

                    if (result$) {
                        dsObservation.push(result$);
                    }
                }
            }
        }

        // some sections instance also may have custom dataSources
        this.processSectionsDataSources(dsObservation);

        // return at least one to proceed with other processings... false means there were no changes
        if (dsObservation.length === 0) {
            return of([false]);
        }

        // wait for all observables to complete
        let res = forkJoin(dsObservation);
        this.logInfo('processCustomDataSources: end');
        return res;
    }

    private processSectionsDataSources(dsObservation: Observable<boolean>[])
    {
        this.logInfo('processSectionDataSources called');
        let formIterator = this.formModel.createFieldsIterator();

        // iterate over all sections instances and load datasources if required
        formIterator.forEach(null, (sec, secIndex) => this.processSectionInstanceDataSources(sec, secIndex, dsObservation));
    }

    private processSectionInstanceDataSources(
        sectionInstance: SectionInstanceModel,
        sectionInstanceIndex: number,
        dsObservation: Observable<boolean>[]) {
        let sectionTemplate = sectionInstance.section.template;

        if (sectionTemplate.dataSources && sectionTemplate.dataSources.length) {
            this.logInfo(`processing sectionInstance dataSources. sectionInstance: '${sectionTemplate.id}[${sectionInstanceIndex}]'`);
            for (let dsTemplate of sectionTemplate.dataSources) {
                if (dsTemplate instanceof CustomQueryDataSourceTemplateModel) {
                    let parameters = this.templateService.expandParametersForSection(sectionInstanceIndex, dsTemplate.parameters);
                    let requiresUpdate =
                        this.forceLoadDataSources ||
                        this.checkIfParameterHasChanged(parameters) ||
                        this.checkIfNewSectionInstance(sectionInstance, sectionInstanceIndex);

                    // todo: check if section instance has been added!
                    if (requiresUpdate) {
                        this.logInfo(`Load sectionInstance dataSource. sectionInstance: '${sectionTemplate.id}[${sectionInstanceIndex}]', dataSource: '${dsTemplate.id}'.`);

                        let result$ =
                            this.processCustomDataSource(sectionInstance, dsTemplate.id, parameters, this.formModel.proxyModel);

                        if (result$) {
                            dsObservation.push(result$);
                        }
                    }
                }
            }
        }
    }

    private checkIfNewSectionInstance(sectionInstance: SectionInstanceModel, sectionInstanceIndex:number):boolean {
        let sectionId = sectionInstance.section.id;
        let tabId = sectionInstance.section.tab.id;
        let isNewSection = false;

        if (this.newFields && this.newFields.length) {
            isNewSection =
                this.newFields.some(
                    (field: FormFieldPathModel) =>
                        field.tabId === tabId &&
                        field.sectionId === sectionId &&
                        field.sectionInstanceIndex === sectionInstanceIndex);
        }

        if (isNewSection) {
            this.logService.debugFormatted('checkIfNewSectionInstance: section intance {0}[{1}] is a new instance.', sectionId, sectionId);
        }

        return isNewSection;
    }

    private processCustomDataSource(
        dataSourcesOwner: any,
        dataSourceId: string,
        parameters: IDataSourceParameter[],
        proxyModel: ProxyModel): Observable<any> {

        this.logInfo(`processCustomDataSource: dataSource '${dataSourceId}.`);
        let result$ =
            this.dataSourcesLoaderService.loadCustomQueryDataSourceData(proxyModel, dataSourceId, parameters).pipe(
                catchError((err: any) => {
                    this.errorHandlingService.handleError(err, `Unknown error on executing custom datasource '${dataSourceId}'`);
                    return of(false);
                }),
                mergeMap((data: any) => {
                    dataSourcesOwner.dataSources[dataSourceId].setData(data);
                    this.logInfo(`Custom data source '${dataSourceId}' has been loaded successfully.`);
                    return of(true);
                }));

        return result$;
    }

    private processControlsDataSources(): Observable<Array<boolean>> {
        let dsObservation: Observable<boolean>[] = [];
        this.logInfo('processControlsDataSources: begin');

        let fieldsIterator = this.formModel.createFieldsIterator();

        // iterate over all fields and refresh fields data sources
        fieldsIterator.forEach((fieldModel, fieldPath) => {

            let sectionInstanceIndex = fieldPath.sectionInstanceIndex;
            let controlTpl = fieldModel.template;
            let controlDataSource = controlTpl.dataSource;
            let result$: Observable<boolean> = null;

            // options datasource handling
            if (controlDataSource && controlDataSource instanceof OptionsDataSourceTemplateModel) {

                let dsParameters = this.templateService.expandParametersForSection(sectionInstanceIndex, controlDataSource.parameters);
                let dependsOnFields = this.templateService.expandFieldsForSection(sectionInstanceIndex, controlDataSource.dependsOn);

                let requiresUpdate = (this.checkIfControlDataSourceUpdateRequired(fieldPath) ||
                    this.checkIfParameterHasChanged(dsParameters) ||
                    this.checkIfDataSourceDependsOn(dependsOnFields));

                if (requiresUpdate) {
                    this.logInfo(`processControlsDataSources: updating options control dataSource. ControlId: ${controlTpl.id}, DataSourceTemplate: ${controlDataSource.id}, paramChanged: ${requiresUpdate}, forceLoadDataSources: ${this.forceLoadDataSources}`);
                    result$ =
                        this.formControlsManagerService.refreshOptionsDataSource(this.formModel, controlDataSource, fieldModel, controlTpl, this.digestUpdateLog);
                }
            }

            // custom datasource handling
            else if (controlDataSource && controlDataSource instanceof ControlCustomDataSourceTemplateModel) {
                let requiresUpdate = this.checkIfControlDataSourceUpdateRequired(fieldPath);
                if (requiresUpdate === false) {

                    // check if parameters in referenced custom datasource (template) has been changed - exist in parametersUpdateLog
                    let dsTemplate = this.templateService.findCustomDataSourceTemplateById(this.formModel.template.dataSources, controlDataSource.id);
                    if (dsTemplate && dsTemplate.parameters) {
                        requiresUpdate = this.checkIfParameterHasChanged(dsTemplate.parameters);
                    }
                }

                if (requiresUpdate) {
                    this.logInfo(`processControlsDataSources updating custom control dataSource. ControlId: ${controlTpl.id}, DataSourceTemplate: ${controlDataSource.id}, paramChanged: ${requiresUpdate}, forceLoadDataSources: ${this.forceLoadDataSources}`);
                    result$ = this.formControlsManagerService.refreshControlCustomDataSource(this.formModel, controlDataSource, fieldModel, controlTpl, this.digestUpdateLog);
                }
            }
            else if (controlDataSource && controlDataSource instanceof ControlSectionDataSourceTemplateModel) {

                let sectionInstance = fieldModel.sectionInstance;

                let requiresUpdate = this.checkIfControlDataSourceUpdateRequired(fieldPath);
                if (requiresUpdate === false) {

                    let sectionTemplate = sectionInstance.section.template;
                    // check if parameters in referenced custom datasource (template) has been changed - exist in parametersUpdateLog
                    let dsTemplate = this.templateService.findCustomDataSourceTemplateById(sectionTemplate.dataSources, controlDataSource.id);
                    if (dsTemplate && dsTemplate.parameters) {
                        let sectionParameters = this.templateService.expandParametersForSection(fieldPath.sectionInstanceIndex, dsTemplate.parameters);
                        requiresUpdate = this.checkIfParameterHasChanged(sectionParameters);
                    }
                }

                if (requiresUpdate) {
                    this.logInfo(`processControlsDataSources updating custom control dataSource. ControlId: ${controlTpl.id}, DataSourceTemplate: ${controlDataSource.id}, paramChanged: ${requiresUpdate}, forceLoadDataSources: ${this.forceLoadDataSources}`);
                    result$ = this.formControlsManagerService.refreshControlCustomSectionDataSource(sectionInstance, this.formModel, controlDataSource, fieldModel, controlTpl, this.digestUpdateLog);
                }
            }

            if (result$) {
                dsObservation.push(result$);
            }
        });


        if (dsObservation.length === 0) {
            return of([false]);
        }

        let res = forkJoin(dsObservation);
        this.logDebug('processControlsDataSources: end');
        return res;
    }

    private checkIfControlDataSourceUpdateRequired(fieldPath: FormFieldPathModel): boolean {

        if (typeof this.forceLoadDataSources === 'boolean' && this.forceLoadDataSources) {
            return true;
        }

        // check if field is from new section instance fields
        if (this.newFields && this.newFields.length) {
            let res = this.newFields.find((item: FormFieldPathModel) => item.equals(fieldPath));
            if (res) {
                return true;
            }
        }

        return false;
    }

    private checkIfParameterHasChanged(parameters: IDataSourceParameter[]): boolean {
        let hasChanged =
            parameters.findIndex((paramTemplate: IDataSourceParameter) => {
                    let fieldPath = FormFieldPathModel.parse(paramTemplate.field);
                    return this.parametersUpdateLog.containsPath(fieldPath)
                        // todo: check if required
                        /*|| this.parametersUpdateLog.contains(paramTemplate.field)*/;
                }) > -1;
        return hasChanged;
    }

    private checkIfDataSourceDependsOn(parameters: Array<string>): boolean {
        if (parameters == null || parameters.length === 0) { return false; }

        return parameters.findIndex((path: string) => {
            let fieldPath = FormFieldPathModel.parse(path);
            return this.digestUpdateLog.containsPath(fieldPath);
        }) > -1;

    }

    // Digest Logging
    private logDebug(msg: string) {
        let s = this.prepareLogMessage(msg);
        this.logService.debug(s);
    }

    private logInfo(msg: string) {
        let s = this.prepareLogMessage(msg);
        this.logService.info(s);
    }

    private logWarning(msg: string) {
        let s = this.prepareLogMessage(msg);
        this.logService.warning(s);
    }

    private prepareLogMessage(msg:string) {
        return `[digest][${this.processId}]:${msg}`;
    }
}
