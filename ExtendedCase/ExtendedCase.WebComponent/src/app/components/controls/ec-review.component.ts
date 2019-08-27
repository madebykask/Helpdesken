import { Component, Input, ElementRef, ViewChild } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { BaseControlTemplateModel } from '../../models/template.model';
import { SingleControlFieldModel } from '../../models/form.model';

import { WindowWrapper } from  '../../shared/window-wrapper';

@Component({
    selector: 'ec-review',
    templateUrl: './ec-review.component.html'
})

export class ExtendedCaseReviewComponent {
    @Input() fieldTemplate: BaseControlTemplateModel;
    @Input() fieldModel: SingleControlFieldModel;
    @Input() form: FormGroup;

    @ViewChild('valueSpan', {static: false}) valueSpan: ElementRef;

    constructor(private windowWrapper: WindowWrapper) {
    }

    getValue1() {
        return this.fieldModel.control.value && this.fieldModel.control.value.length > 0 ? this.fieldModel.control.value[0] : '';
    }

    getValue2() {
        return this.fieldModel.control.value && this.fieldModel.control.value.length > 1 ? this.fieldModel.control.value[1] : '';
    }

    selectValue() {
        let wnd = this.windowWrapper.nativeWindow;
        let range = wnd.document.createRange();
        range.selectNodeContents(this.valueSpan.nativeElement);

        let selection = wnd.getSelection();
        selection.removeAllRanges();
        selection.addRange(range);
    }
}
