import { Component, Input, OnChanges, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { SingleControlFieldModel } from '../../../models/form.model';
import { ComponentCommService } from '../../../services/component-comm.service';
import { BaseControl } from '../base-control';

@Component({
    selector: 'ec-multiselect',
    templateUrl: './ec-multiselect.component.html'
})

export class ExtendedCaseMultiselectComponent extends BaseControl implements OnChanges {
    @Input() fieldModel: SingleControlFieldModel;
    @Input() form: FormGroup;

    constructor(componentCommService: ComponentCommService,
                changeDetector: ChangeDetectorRef) {
        super(componentCommService, changeDetector);
    }

    ngOnChanges(): void {
    }
}
