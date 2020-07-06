import { Component, Input, OnInit, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { ComponentCommService } from '../../../services/component-comm.service';
import { SingleControlFieldModel } from '../../../models/form.model';
import { BaseControl } from '../base-control';

@Component({
    selector: 'ec-checkbox',
    templateUrl: './ec-checkbox.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})

export class ExtendedCaseCheckboxComponent extends BaseControl implements OnInit {
    @Input() fieldModel: SingleControlFieldModel;
    @Input() form: FormGroup;

    constructor(componentCommService: ComponentCommService,
        private changeDetector: ChangeDetectorRef) {
        super(componentCommService, changeDetector);
    }

    ngOnInit(): void {

    }

}
