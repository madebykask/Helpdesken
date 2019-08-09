import { Component, Input, OnInit, Output, OnChanges, ChangeDetectorRef, ElementRef, Renderer2, ViewChild } from '@angular/core';
import { SectionTemplateModel, BaseControlTemplateModel } from '../../models/template.model';
import { FieldModelBase, FormControlType, SectionInstanceModel, SingleControlFieldModel } from '../../models/form.model';
import { ProxyModel, ProxySectionInstance, ProxyControl } from '../../models/proxy.model';
import { WindowWrapper } from  '../../shared/window-wrapper';
import { DigestResult } from '../../models/digest.model';
import { FormControlsManagerService } from '../../services/form-controls-manager.service';
import { LogService } from '../../services/log.service';
import * as cm from '../../utils/common-methods';

@Component({
    selector: 'ec-review-ex',
    templateUrl: './ec-review-ex.component.html'
})
export class ExtendedCaseReviewComponentEx implements OnChanges {

    @Input() fieldTemplate: BaseControlTemplateModel;
    @Input() reviewFieldModel: FieldModelBase;
    @Input() proxyModel: ProxyModel;

    @ViewChild('valueSpan', {static: true}) valueSpan: ElementRef;

    value1: any;
    value2: any;
    isHidden: boolean;

    private reviewFieldTemplate: BaseControlTemplateModel;
    private reviewProxyControl: ProxyControl;

    get reviewSectionTemplate() : SectionTemplateModel {
        return this.reviewFieldModel.sectionInstance.section.template;
    }

    constructor(
        private changeDetector: ChangeDetectorRef,
        private formControlsManagerService: FormControlsManagerService,
        private logService: LogService,
        private windowWrapper: WindowWrapper) {
    }

    ngOnChanges() {
        let fieldPath = this.reviewFieldModel.getFieldPath();
        this.reviewProxyControl = this.proxyModel.findProxyControl(fieldPath);
        this.reviewFieldTemplate = this.reviewFieldModel.template;
    }

    setVisibility() {
        if (this.reviewFieldModel.hidden ||
            (cm.isUndefinedNullOrEmpty(this.value1) && cm.isUndefinedNullOrEmpty(this.value2))) {
            this.isHidden = true;
        }
        else {
            this.isHidden = false;
        }
    }

    getLabel() {
        let label = '';
        if (this.fieldTemplate) {
            label = this.fieldTemplate.label;
        }

        if (!label) {
            label = this.reviewFieldTemplate.label;
        }

        return label;
    }

    updateValues(result: DigestResult) {
        //this.logService.debug(`Updating review control values. ReviewFieldId: ${this.reviewFieldTemplate.id}, ReviewSectionId: ${this.reviewSectionTemplate.id}`);
        let values =
            this.fieldTemplate && this.fieldTemplate.valueBinding
                ? this.getValeBindingValues(result)
                : this.getReviewControlValues(result);

        if (values && values.length === 2) {
            this.value1 = values[0];
            this.value2 = values[1];
        } else {
            this.value1 = '';
            this.value2 = '';
        }

        this.setVisibility();

        this.logService.debugFormatted('review control (Id:{0}) values changed. Value1: {1}, Value2: {2}, Hidden: {3}',
            this.reviewFieldTemplate.id, this.value1, this.value2, this.isHidden);
    }

    private getValeBindingValues(result: DigestResult): string[] {
        let values = this.fieldTemplate.valueBinding.call(this.reviewProxyControl, this.proxyModel, result ? result.digestUpdateLog : null);
        return values;
    }

    private getReviewControlValues(result: DigestResult): string[] {
        let value = this.reviewProxyControl.value;
        let secondaryValue = '';

        if (this.reviewFieldTemplate.controlType === FormControlType.CheckboxList) {
            let fieldValueModel = this.formControlsManagerService.getMultiControlValueForSave(this.reviewFieldModel, this.reviewProxyControl);
            value = fieldValueModel.Value;
        }
        else if (this.reviewFieldTemplate.controlType === FormControlType.Search ||
            this.reviewFieldTemplate.controlType === FormControlType.Radio ||
            this.reviewFieldTemplate.controlType === FormControlType.Dropdown ||
            this.reviewFieldTemplate.controlType === FormControlType.Multiselect) {
            secondaryValue = value === this.reviewProxyControl.secondaryValue
                ? ''
                : this.reviewProxyControl.secondaryValue;
        }

        return [value, secondaryValue].map(item => cm.isUndefinedOrNull(item) ? '' : item.toString());
    }
    
    selectValue() {
        this.getSelectedValue(this.windowWrapper.nativeWindow, this.valueSpan.nativeElement);
    }
    
    protected getSelectedValue(wnd: any, valueSpanElement: any) {

        let range = wnd.document.createRange();
        range.selectNodeContents(valueSpanElement);

        let selection = wnd.getSelection();
        selection.removeAllRanges();
        selection.addRange(range);
    }
}

