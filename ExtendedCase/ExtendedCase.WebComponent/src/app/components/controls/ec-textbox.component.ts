import { Component, Input, OnInit, OnChanges, DoCheck, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { SingleControlFieldModel, FormControlType } from '../../models/form.model';
import { ComponentCommService } from '../../services/component-comm.service';
import { MaskType } from '../../directives/masks/mask.directive';
import { BaseControl } from './base-control';

@Component({
    selector: 'ec-textbox',
    templateUrl: './ec-textbox.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})

export class ExtendedCaseTextBoxComponent extends BaseControl implements OnInit, OnChanges, DoCheck {
    @Input() fieldModel: SingleControlFieldModel;
    @Input() form: FormGroup;
    @Input() subtype = '';
    maskTypes = MaskType;
    controlType = new FormControlType(); // to  use in html static string

    constructor(componentCommService: ComponentCommService,
        private changeDetector: ChangeDetectorRef) {
        super(componentCommService, changeDetector);
    }

    ngOnChanges(): void {
        if (this.subtype === FormControlType.Unit) {
            this.fieldTemplate.addonText = this.fieldTemplate.addonText || 'Unit';
        }
        // console.log("ngOnChanges " + this.fieldModel.id);
    }

    ngDoCheck(): void {
        // console.log("ngDoCheck " +this.fieldModel.id);
    }

    ngOnInit(): void {
    }

}
