import { Injectable, Inject, forwardRef, Output, EventEmitter } from '@angular/core';
import {Subject, Observable} from 'rxjs';
import { AbstractControl } from '@angular/forms';
import { SectionTemplateModel, BaseControlTemplateModel } from '../models/template.model';
import { SectionModel, SectionInstanceModel, FieldModelBase, FormControlType, ItemModel } from '../models/form.model';
import { DigestResult } from '../models/digest.model';
import * as commonMethods from '../utils/common-methods';
import { LogService } from './log.service';
import { ValidateOn } from '../shared/validation-types';
import { IAppConfig, AppConfig } from '../shared/app-config/app-config';

@Injectable()
export class ComponentCommService {

    // Observable string sources
    private controlValueChanged = new Subject<ControlValueChangedParams>();
    private controlDataSourceChange = new Subject<ControlDataSourceChangeParams>();
    private validationModeChange = new Subject<ValidateOn>();
    private populateSectionSubject = new Subject<PopulateSectionParams>();
    private enableSectionSubject = new Subject<EnableSectionParams>();
    private addSectionInstanceSubject = new Subject<AddSectionInstanceParams>();
    private deleteSectionInstanceSubject = new Subject<DeleteSectionInstanceParams>();
    private digestCompletedSubject = new Subject<DigestResult>();

    // Observable string streams
    controlValueChanged$ = this.controlValueChanged.asObservable();
    controlDataSourceChange$ = this.controlDataSourceChange.asObservable();
    validationModeChange$ = this.validationModeChange.asObservable();
    populateSectionSubject$ = this.populateSectionSubject.asObservable();
    enableSectionSubject$ = this.enableSectionSubject.asObservable();
    addSectionInstanceSubject$ = this.addSectionInstanceSubject.asObservable();
    deleteSectionInstanceSubject$ = this.deleteSectionInstanceSubject.asObservable();
    digestCompletedSubject$ = this.digestCompletedSubject.asObservable();

    constructor(
        public logService: LogService,
        @Inject(forwardRef(() => AppConfig)) private config: IAppConfig) {
    }

    announceControlValueChanged(params: ControlValueChangedParams) {
        let lastValue = params.formModel.lastValue || '';
        let controlNewValue = params.value || '';
        this.logService.debugFormatted('[VALUE_CHANGED_EVENT] announceControlValueChanged: check if value has changed. Control: {0}, LastValue: {1}, ControlNewValue: {2}', params.control.id, lastValue, controlNewValue);

        let areEqual =
            commonMethods.areEqualsDeep(lastValue, controlNewValue) ||
            (commonMethods.isUndefinedNullOrEmpty(lastValue) && commonMethods.isUndefinedNullOrEmpty(controlNewValue));

        if (!areEqual) {
            this.logService.infoFormatted('[VALUE_CHANGED_EVENT] Raising digest valueChanged event! Control: {0}, LastValue:{1}, ControlNewValue: {2}', params.control.id, lastValue, controlNewValue);
            this.controlValueChanged.next(params);
        } else {
            this.logService.debug('[VALUE_CHANGED_EVENT] value has not changed.');    
        }
    }

    announceControlDataSourceChange(params: ControlDataSourceChangeParams) {
        this.logService.debugFormatted('ComponentCommService: announceControlDataSourceChange: {0}', params.controlId);
        this.controlDataSourceChange.next(params);
    }

    announceValidationModeChange(value: ValidateOn) {
        this.logService.debugFormatted('ComponentCommService: announceValidationModeChange: {0}', value);
        this.validationModeChange.next(value);
    }

    raisePopulateSectionEvent(args: PopulateSectionParams) {
        this.populateSectionSubject.next(args);
    }

    raiseEnableSectionEvent(args: EnableSectionParams) {
        this.enableSectionSubject.next(args);
    }

    raiseAddNewSectionInstance(args: AddSectionInstanceParams) {
        this.addSectionInstanceSubject.next(args);
    }

    raiseDeleteSectionInstance(args: DeleteSectionInstanceParams) {
        this.deleteSectionInstanceSubject.next(args);
    }

    raiseDigestCompleted(result:DigestResult) {
        this.digestCompletedSubject.next(result);
    }
}

export class ControlValueChangedParams {
    constructor(public control: BaseControlTemplateModel, public formControl: AbstractControl, public value: any, public formModel: FieldModelBase) {
    }
}

export class ControlDataSourceChangeParams {
    constructor(public controlId: string, public items: ItemModel[], public isChanged: boolean) {
    }
}

export class PopulateSectionParams {
    constructor(public section: SectionInstanceModel, public sectionTemplate: SectionTemplateModel) {
    }
}

export class EnableSectionParams {
    constructor(public section: SectionInstanceModel, public sectionTemplate: SectionTemplateModel, public state: boolean) {
    }
}

export class AddSectionInstanceParams {
    constructor(public section: SectionModel) {
    }
}

export class DeleteSectionInstanceParams {
    constructor(public index:number, public sectionInstance: SectionInstanceModel) {
    }
}