import { Component, Input, OnInit, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import {SingleControlFieldModel} from '../../../models/form.model';
import { ComponentCommService } from '../../../services/component-comm.service';
import { BaseControl } from '../base-control';

@Component({
    selector: 'ec-radio',
    templateUrl: './ec-radio.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})

export class ExtendedCaseRadioComponent extends BaseControl implements OnInit {
    @Input() fieldModel: SingleControlFieldModel;
    @Input() form: FormGroup;

    constructor(componentCommService: ComponentCommService,
        changeDetector: ChangeDetectorRef) {
        super(componentCommService, changeDetector);
    }

    ngOnInit(): void {

    }

}
