import { FormArray, FormGroup, FormControl } from '@angular/forms';

import { Observable } from 'rxjs/Rx';
import { IMap } from '../shared/common-types'
import { ProxyModel } from './proxy.model'
import { ValidateOn } from '../shared/validation-types';

export type CustomDataSourceTypes = CustomStaticDataSourceTemplateModel | CustomQueryDataSourceTemplateModel;
export type ControlDataSourceTemplateModelTypes = DataSourceItemTemplateModel[] | OptionsDataSourceTemplateModel |
    ControlCustomDataSourceTemplateModel | ControlSectionDataSourceTemplateModel;

export class FormTemplateModel {
    id: string;
    name: string;
    tabs: TabTemplateModel[];
    dataSources: CustomDataSourceTypes[] = [];
    globalFunctions: {};
    localization: {};
    styles: '';

}

export abstract class CustomDataSourceTemplateModelBase {
}

export interface IWithDataSourceParameters {
    parameters: IDataSourceParameter[];
}

export class CustomStaticDataSourceTemplateModel
        extends CustomDataSourceTemplateModelBase {
    id: string;
    data: any[];

    constructor(id: string, data: any[]) {
        super();

        this.id = id;
        this.data = data || [];
    }
}

export class CustomQueryDataSourceTemplateModel
        extends CustomDataSourceTemplateModelBase
        implements IWithDataSourceParameters {
    id: string;
    parameters: IDataSourceParameter[] = [];

    constructor(id: string, parameters: any[]) {
        super();
        this.id = id;

        if (parameters !== undefined && parameters !== null) {
            this.parameters = parameters.map(item => new DataSourceParameterTemplateModel(item.name, item.field));
        } else {
            this.parameters = [];
        }
    }
}


export class TabTemplateModel {
    id: string;
    name: string;
    hiddenBinding: (m: any, l?: any) => any;
    disabledBinding: (m: any, l?: any) => any;
    sections: SectionTemplateModel[];
    columnCount: number;

    constructor(id: string,
        name: string,
        columnCount?: number,
        hiddenBinding?: (m: any, l?: any) => any,
        disabledBinding?: (m: any, l?: any) => any) {
        this.id = id;
        this.name = name;
        this.columnCount = columnCount;
        this.hiddenBinding = hiddenBinding;
        this.disabledBinding = disabledBinding;
    }
}


export enum SectionType {
    forms,
    review
}

export class SectionTemplateModel {
    id: string;
    name: string;
    hiddenBinding: (m: any, l?: any) => any;
    disabledBinding: (m: any, l?: any) => any;
    controls: BaseControlTemplateModel[] = [];
    tab: TabTemplateModel;
    column: number;
    multiSectionAction: MultiSectionAction; 
    populateAction: PopulateAction;
    enableAction: EnableAction;
    disabledStateBehavior: DisabledStateBehavior;
    
    type: SectionType = SectionType.forms;
    reviewSectionId: string = '';
    reviewControls: string = '';

    dataSources: CustomQueryDataSourceTemplateModel[];

    constructor(id: string,
        name: string,
        tab: TabTemplateModel,
        column?: number,
        hiddenBinding?: (m: any, l?: any) => any,
        disabledBinding?: (m: any, l?: any) => any,
        multiSectionAction? : MultiSectionAction,
        populateAction?: PopulateAction,
        enableAction?: EnableAction,
        disabledStateBehavior?: DisabledStateBehavior,
        type?: SectionType) {
        this.id = id;
        this.name = name;
        this.tab = tab;
        this.hiddenBinding = hiddenBinding;
        this.disabledBinding = disabledBinding;
        this.column = column;
        this.multiSectionAction = multiSectionAction; 
        this.populateAction = populateAction;
        this.enableAction = enableAction;
        this.disabledStateBehavior = disabledStateBehavior;
        this.type = type || SectionType.forms;
    }

    hasReview() {
        return this.controls && this.controls.some((item: BaseControlTemplateModel) => item.controlType === 'review');
    }
}

export class MultiSectionAction {
    allowMultipleSections : boolean;
    actionName: string;
    maxCount: number = 0;
}

export class PopulateAction {
    actionName: string;
    populateBinding: (m: any) => IMap<any>;
}

export class EnableAction {
    initialState: boolean;
    label: string;
}

export class DisabledStateBehavior {
    constructor(
        public action: DisabledStateAction,
        public condition: DisabledStateActionCondition = DisabledStateActionCondition.Any
    ){}
}

export enum DisabledStateAction {
    Clear,
    Keep,
    RestorePrev,
    ClearOnUserOnly
}

export enum DisabledStateActionCondition {
    Any,
    UserOnly
}

export class TemplateValidator {
    type: string;
    message?: string;
    enabled?: (proxyModel: ProxyModel) => boolean;
    valid?: (proxyModel: ProxyModel) => boolean;
    value?: any;
    validationMode: ValidateOn;
    emptyValues: Array<string>;

    constructor(options: {
        type: string,
        message?: string,
        enabled?: (proxyModel: ProxyModel) => boolean,
        valid?: (proxyModel: ProxyModel) => boolean,
        value?: any,
        validationMode?: ValidateOn;
    }) {
        this.type = options.type;
        this.message = options.message;
        this.enabled = options.enabled;
        this.valid = options.valid;
        this.value = options.value;
        this.validationMode = options.validationMode || ValidateOn.OnSave;
    }
}

export class TemplateValidators {
    onSave: Array<TemplateValidator>;
    onNext?: Array<TemplateValidator>;

    getValidatorsByType(type: string): Array<TemplateValidator> {
        const result = new Array<TemplateValidator>();
        if (this.onSave) {
            this.onSave.forEach((validator: TemplateValidator) => {
                if (validator.type === type) result.push(validator);
            });
        }

        if (this.onNext) {
            this.onNext.forEach((validator: TemplateValidator) => {
                if (validator.type === type) result.push(validator);
            });
        }

        return result;
    }
}

export class BaseControlTemplateModel {
    id: string;
    label: string;
    isLabelHtml: boolean;
    addonText: string;
    order: number;
    controlType: string; // FormControlType
    noDigest: boolean;
    processControlDataSourcesOnly: boolean;
    caseBinding: string;
    resetValueOnItemsUpdate: boolean;
    shouldNotSave: boolean;
    dataSource: ControlDataSourceTemplateModelTypes;
    valueBinding: (m: any, l?: any) => any;
    hiddenBinding: (m: any, l?: any) => any;
    disabledBinding: (m: any, l?: any) => any;
    dataSourceFilterBinding: (m: any, l?: any, d?: any) => any;
    validators?: TemplateValidators;
    warningBinding: (m: any, l?: any) => string;
    isRequired?: ValidateOn;//Depricated - use fieldModel.isRequired instead;
    section: SectionTemplateModel;
    isEverValidated: boolean;
    mode: string;
    reviewControlId: string;
    emptyElement: string;

    constructor(options: {
        id?: string,
        label?: string,
        isLabelHtml?: boolean,
        addonText?: string,
        order?: number,
        controlType?: string,
        noDigest?: boolean,
        processControlDataSourcesOnly?: boolean,
        caseBinding?: string,
        resetValueOnItemsUpdate?: boolean,
        shouldNotSave?: boolean,
        section?: SectionTemplateModel,
        dataSource?: ControlDataSourceTemplateModelTypes,
        valueBinding?: (m: any, l?: any) => any,
        hiddenBinding?: (m: any, l?: any) => any,
        disabledBinding?: (m: any, l?: any) => any,
        dataSourceFilterBinding?: (m: any, l?: any, d?: any) => any,
        validators?: TemplateValidators,
        warningBinding?: (m: any, l?: any, d?: any) => string,
        mode?: string,
        reviewControlId?: string,
        emptyElement?:any;
    } = {}) {
        this.id = options.id || '';
        this.label = options.label || '';
        this.isLabelHtml = options.isLabelHtml || false;
        this.order = options.order === undefined ? 1 : options.order;
        this.controlType = options.controlType || '';
        this.noDigest = options.noDigest || false;
        this.processControlDataSourcesOnly = options.processControlDataSourcesOnly || false;
        this.caseBinding = options.caseBinding || '';
        this.resetValueOnItemsUpdate = options.resetValueOnItemsUpdate || false;
        this.shouldNotSave = options.shouldNotSave || false;
        this.section = options.section;
        this.valueBinding = options.valueBinding;
        this.hiddenBinding = options.hiddenBinding;
        this.disabledBinding = options.disabledBinding;
        this.dataSource = options.dataSource;
        this.dataSourceFilterBinding = options.dataSourceFilterBinding;
        this.validators = options.validators;
        this.warningBinding = options.warningBinding;
        this.addonText = options.addonText || '';
        this.mode = options.mode || '';
        this.reviewControlId = options.reviewControlId || '';
        this.isEverValidated = false;
        this.emptyElement = options.emptyElement;
    }

    get isReview() {
        return this.section.type === SectionType.review;
    }
}

export class DataSourceItemTemplateModel {
    value: string;
    text: string;
    constructor(value: string,
                text: string) {
        this.value = value;
        this.text = text;
    }
}

export abstract class ControlCustomDataSourceTemplateModelBase {
    id: string;
    valueField: string;
    textField: string;

    protected constructor(
        id: string,
        valueField: string,
        textField: string) {

        this.id = id;
        this.valueField = valueField;
        this.textField = textField;
    }
}

export class ControlCustomDataSourceTemplateModel
    extends ControlCustomDataSourceTemplateModelBase{
    constructor(id: string,
        valueField: string,
        textField: string) {
        super(id, valueField, textField);
    }
}

export class ControlSectionDataSourceTemplateModel
    extends ControlCustomDataSourceTemplateModelBase {

    constructor(id: string,
        valueField: string,
        textField: string) {
        super(id, valueField, textField);
    }
}

export class OptionsDataSourceTemplateModel
    implements IWithDataSourceParameters{
    id: string;
    parameters: IDataSourceParameter[] = [];
    dependsOn: Array<string>;

    constructor(id: string,
        parameters: any[],
        dependsOn: string[]) {
        this.id = id;

        if (parameters && parameters.length) {
            this.parameters = parameters.map((parameter: any) => {
                return new DataSourceParameterTemplateModel(parameter.name, parameter.field);
            });
        } else {
            this.parameters = [];
        }

        this.dependsOn = dependsOn || new Array<string>();
    }
}

export interface IDataSourceParameter {
    name: string;
    field: string;
}

export class DataSourceParameterTemplateModel
    implements  IDataSourceParameter {

    name: string;
    field: string;
    constructor(name: string,
                field: string) {
        this.name = name;
        this.field = field;
    }
}