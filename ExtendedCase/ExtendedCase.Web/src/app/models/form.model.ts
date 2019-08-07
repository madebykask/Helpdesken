import { FormGroup, FormControl } from '@angular/forms';
import { ProxyModel } from './proxy.model';
import { TabTemplateModel, FormTemplateModel, SectionTemplateModel, BaseControlTemplateModel } from '../models/template.model';
import { IMap } from '../shared/common-types'
import { FormFieldPathModel } from './form-field-path.model';
import { FormFieldsIterator } from '../models/form-fields-iterator';
import { ValidateOn } from '../shared/validation-types';
// import { FormStateModel, FormStateItem, FormItemPath } from './form-state.model';

export class FormModel {
    tabs: { [id: string]: TabModel; };
    proxyModel: ProxyModel;
    dataSources: { [id: string]: CustomDataSourceModel };
    template: FormTemplateModel;
    // formState: FormStateModel;

    constructor(template:FormTemplateModel) {
        this.template = template;
        this.dataSources = {};
    }

    createFieldsIterator() {
        return new FormFieldsIterator(this);
    }

    acceptChanges() {
        this.createFieldsIterator()
            .forEach((fieldModel, fieldPath) => fieldModel.acceptChanges());
    }

    findSectionSafe(tabId: string, sectionId: string) : SectionModel {
        let section: SectionModel = null;

        if (this.tabs.hasOwnProperty(tabId)) {
            let tab = this.tabs[tabId];
            if (tab.sections.hasOwnProperty(sectionId)) {
                section = tab.sections[sectionId];
            }
        }
        return section;
    }

    findFormField(fieldPath: FormFieldPathModel): FieldModelBase {
        let fieldModel: FieldModelBase = undefined;

        try {
            fieldModel =
                this.tabs[fieldPath.tabId].sections[fieldPath.sectionId].instances[fieldPath.sectionInstanceIndex].fields[fieldPath.fieldId];
        } catch (err) {
            console.error(`Failed to find form field: ${JSON.stringify(fieldPath)}`);
        }
        return fieldModel;
    }
}

export class CustomDataSourceModel {
    id: string;
    private data: any[] = [];

    constructor(id: string) {
        this.id = id;
    }

    getData() : any[] {
        return this.data;
    }

    setData(data: any[]) : void {
        this.data = data;
    }

    clearData() : void {
        this.data = [];
    }
}

export class TabModel {
    id: string;
    sections: { [id: string]: SectionModel; };
    group: FormGroup;
    hidden: boolean;
    disabled: boolean;
    valid: boolean; // using new variable instead of group.valid to avoid multiply update of UI
    template: TabTemplateModel;

    constructor(id: string, template: TabTemplateModel){
        this.id = id;
        this.sections = {};
        this.hidden = false;
        this.disabled = false;
        this.valid = true;
        this.template = template;
    }
}

export class SectionModel {
    id: string;
    instances: SectionInstanceModel[] = [];
    group: FormGroup;
    template: SectionTemplateModel;
    tab: TabModel;
    disabled: boolean;
    hidden: boolean;

    private _lastInstanceId: number = -1;

    constructor(id: string, template: SectionTemplateModel, tab: TabModel) {
        this.id = id;
        this.instances = [];
        this.template = template;
        this.tab = tab;
    }

    getNextInstanceId(): number {
        this._lastInstanceId += 1;
        return this._lastInstanceId;
    }

    getSectionInstanceIndex(instance: SectionInstanceModel) {
        return this.instances.indexOf(instance);
    }
    get lastInstanceId():number {
        return this._lastInstanceId;
    }
}

export class SectionInstanceModel {
    private _instanceId: number;

    section: SectionModel;
    fields: { [id: string]: FieldModelBase; };
    group: FormGroup;

    hidden: boolean;
    disabled: boolean;
    sectionEnableStateSelection: boolean;
    dataSources: { [id: string]: CustomDataSourceModel };

    constructor(instanceId: number, section: SectionModel) {
        this._instanceId = instanceId;
        this.section = section;
        this.fields = {};
        this.dataSources = {};
        this.hidden = false;
        this.disabled = false;
        this.section = section;
    }

    get id():string {
        return this._instanceId.toString();
    }

    get numericId(): number {
        return this._instanceId;
    }
}

/* Important notes:
  1. LastValue field: a concept of lastValue is used in two cases:
    - Raise controlValueChanged event: lastValue is used to check if it was a ui change or digest change (programmatic).
      During digest value change (valueBinding func) lastValue is set to new control value which indicates that controlValueChanged event is not required since value was changed programatically.
      During ui change values (lastValue and control.value) will be different and event will be raised to trigger new digest to run since the change was trigger by a user on ui.
   - Restore to previous value: lastValue is also used as the source of field's previous value when handling controlValueChanged event since the event has only new value in the args and before lastValue is set to new control value the prevValue field is assigned the lastValue (see ExtendedCaseComponent.ts\subscribeValueChangedEvents);
  2. PreviousValue field: field is used to store field previous value and its updated automatically when lastValue is changed via setLastValue method.
     PreviousValue is used to be able to restore section values during disabling section (one of the behaviors)
 */
export abstract class FieldModelBase {
    id: string;
    hidden: boolean;
    isRequired?: ValidateOn;
    preFilteredItems: ItemModel[];
    items: ItemModel[];
    warnings: string[];

    // state fields
    private _originalValue: any;
    private _lastValue: any;
    private _previousValue: any;

    private _originalAdditionalData = '';
    private _additionalData = '';
    private _prevAdditionalData = '';

    template: BaseControlTemplateModel;
    sectionInstance: SectionInstanceModel;

    constructor(id: string, template: BaseControlTemplateModel, sectionInstance: SectionInstanceModel) {
        this.id = id;
        this.template = template;
        this.hidden = false;
        this.warnings = new Array<string>();
        this.template = template;
        this.sectionInstance = sectionInstance;
    }

    //////// Getters ///////////

    get originalValue(): any {
        return this._originalValue;
    }

    get lastValue(): any {
        return this._lastValue;
    }

    get previousValue(): any {
        return this._previousValue;
    }

    get originalAdditionalData() {
        return this._originalAdditionalData;
    }

    get additionalData(): string {
        return this._additionalData;
    }

    get prevAdditionalData(): string {
        return this._prevAdditionalData;
    }

    /////////////////////////////

    abstract getControlGroup(): FormControl | FormGroup;

    protected abstract changeControlValue(value: any): void;

    setPristine(isPristine?: boolean) {
        if (typeof isPristine !== 'boolean') return;
        if (isPristine) {
            this.getControlGroup().markAsPristine();
        } else {
            this.getControlGroup().markAsDirty();
        }
    }

    acceptChanges() {
        this._originalValue = this.lastValue;
        this._originalAdditionalData = this.additionalData;
    }

    setControlValue(value: any): void {
        this.setLastValue(value);
        this.changeControlValue(value);
    }

    setLastValue(value: any) {
        this._previousValue = this._lastValue;
        this._lastValue = value;
    }

    setAdditionalData(value: string) {
        this._prevAdditionalData = this._additionalData;
        this._additionalData = value;
    }

    enable(opts?: { onlySelf?: boolean, emitEvent?: boolean}, noValidation?: boolean) {
        const ctrl = this.getControlGroup();
        if (noValidation) {
            const tempValidators = ctrl.validator;
            const tempAsyncValidators = ctrl.asyncValidator;
            ctrl.clearValidators();
            ctrl.clearAsyncValidators();
            ctrl.enable(opts);
            ctrl.setValidators(tempValidators);
            ctrl.setAsyncValidators(tempAsyncValidators);
        } else {
            ctrl.enable(opts);
        }
    }

    getFieldPath(): FormFieldPathModel {
        let tabModel = this.sectionInstance.section.tab;
        let sectionModel = this.sectionInstance.section;
        let sectionInstanceIndex = sectionModel.getSectionInstanceIndex(this.sectionInstance);

        return new FormFieldPathModel(
            tabModel.id,
            sectionModel.id,
            sectionInstanceIndex,
            this.id
        );
    }

    getUiPath(): string {
        let fieldPath = this.getFieldPath();
        return `${fieldPath.tabId}_${fieldPath.sectionId}_${fieldPath.sectionInstanceIndex}_${this.id}`;
    }

    abstract get isReview(): boolean;
}

export class SingleControlFieldModel extends FieldModelBase {
    control: FormControl;

    constructor(id: string, template: BaseControlTemplateModel, sectionInstance: SectionInstanceModel) {
        super(id, template, sectionInstance);
    }

    getControlGroup(): FormControl {
        return this.control;
    }

    protected changeControlValue(value: any): void {
        this.control.setValue(value, { emitEvent: false });
    }

    get isReview() {
        return this.template.controlType === 'review';
    }
}

export class MultiValueSingleControlFieldModel extends SingleControlFieldModel {
    constructor(id: string, template: BaseControlTemplateModel, sectionInstance: SectionInstanceModel) {
        super(id, template, sectionInstance);
    }

    get isReview() {
        return false;
    }
}

export class MultiControlFieldModel extends FieldModelBase {

    controls: FormGroup;

    constructor(id: string, template: BaseControlTemplateModel, sectionInstance: SectionInstanceModel) {
        super(id, template, sectionInstance);
    }

    getControlGroup(): FormGroup {
        return this.controls;
    }

    changeControlValue(value: any): void {
        this.controls.setValue(value, { emitEvent: false });
    }

    getMultiControlValues(): IMap<any> {
        let values: IMap<any> = {};
        for (let controlKey of Object.keys(this.controls.controls)) {
            values[controlKey] = this.controls.controls[controlKey].value;
        }
        return values;
    }

    get isReview() {
        return false;
    }
}

export class ItemModel {
    constructor(public value: string, public text: string) { }

    static createEmptyElement(text: string) {
        return new ItemModel('', text);
    }
}

export class FormControlType {

    // constructor() { // removed oto use in html
    //    throw new Error('Cannot new this class');
    // }

    static CheckboxList = 'checkbox-list';
    static Search = 'search';
    static Textbox = 'textbox';
    static Amount = 'amount';
    static Percentage = 'percentage';
    static Unit = 'unit';
    static Label = 'label';
    static Textarea = 'textarea';
    static Dropdown = 'dropdown';
    static Multiselect = 'multiselect';
    static Date = 'date';
    static Checkbox = 'checkbox';
    static Radio = 'radio';
    static Review = 'review';
    static Number = 'number';
    static AltNumber = 'altnumber';
    static Html = 'html';

    type = FormControlType;// workaroud to  use in html template
}
