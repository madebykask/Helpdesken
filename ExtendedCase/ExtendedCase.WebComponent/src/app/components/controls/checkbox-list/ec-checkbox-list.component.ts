import { Component, Input, OnInit, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { ComponentCommService } from '../../../services/component-comm.service';
import { MultiControlFieldModel } from '../../../models/form.model';
import { BaseControl } from '../base-control';

@Component({
    selector: 'ec-checkbox-list',
    templateUrl: './ec-checkbox-list.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})

export class ExtendedCaseCheckboxListComponent extends BaseControl implements OnInit {
    @Input() fieldModel: MultiControlFieldModel;
    @Input() form: FormGroup;

    constructor(componentCommService: ComponentCommService,
        changeDetector: ChangeDetectorRef) {
        super(componentCommService, changeDetector);
    }

    ngOnInit(): void {
    }

    generateId(value: string): string {
        value = value || '';
        return `${this.fieldModel.getUiPath()}_${value}`;
    }

}
