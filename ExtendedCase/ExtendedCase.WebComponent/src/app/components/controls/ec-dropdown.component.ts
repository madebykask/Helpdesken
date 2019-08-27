import { Component, Input, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { SingleControlFieldModel} from '../../models/form.model';
import { ComponentCommService } from '../../services/component-comm.service';
import { BaseControl } from './base-control';

@Component({
    selector: 'ec-dropdown',
    templateUrl: './ec-dropdown.component.html'
})

export class ExtendedCaseDropdownComponent extends BaseControl implements OnInit {
    @Input() fieldModel: SingleControlFieldModel;
    @Input() form: FormGroup;

    constructor(componentCommService: ComponentCommService,
                changeDetector: ChangeDetectorRef) {
        super(componentCommService, changeDetector);
    }

    ngOnInit(): void {
    }
}
