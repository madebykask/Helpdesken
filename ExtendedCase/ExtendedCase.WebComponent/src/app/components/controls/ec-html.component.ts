import { Component, Input, OnInit, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { BaseControlTemplateModel } from '../../models/template.model';
import { SingleControlFieldModel } from '../../models/form.model';
import { ComponentCommService } from '../../services/component-comm.service';
import { BaseControl } from './base-control';

@Component({
    selector: 'ec-html',
    templateUrl: './ec-html.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})

export class ExtendedCaseHtmlComponent extends BaseControl implements OnInit {
    @Input() fieldModel: SingleControlFieldModel;
    @Input() form: FormGroup;

    constructor(componentCommService: ComponentCommService,
        private changeDetector: ChangeDetectorRef) {
        super(componentCommService, changeDetector);
    }

    ngOnInit() {
    }
}
