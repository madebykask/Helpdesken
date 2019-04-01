import { Component, Input, OnInit, AfterViewInit, ChangeDetectionStrategy, ChangeDetectorRef, ElementRef, Inject, ViewEncapsulation } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import {SingleControlFieldModel} from '../../models/form.model';
import { ComponentCommService } from '../../services/component-comm.service';
import * as moment from 'moment';
import { IAppConfig, AppConfig } from '../../shared/app-config/app-config';
import { BaseControl } from './base-control';

@Component({
    selector: 'ec-date',
    templateUrl: './ec-date.component.html',
    styleUrls: ['./ec-date.component.css'],
    host: {
        '(document:click)': 'closeOutside($event)'
    }
})

export class ExtendedCaseDateComponent extends BaseControl implements OnInit, AfterViewInit {
    @Input() fieldModel: SingleControlFieldModel;
    @Input() form: FormGroup;
    
    private showDatepicker = false;

    datepickerHeight = 275;
    inputHeight: string;
    dateModel: string;
    dateFormat: string;
    parentSelectors: Array<any>;
    private renderDatePicker = false;

    constructor(componentCommService: ComponentCommService, public element: ElementRef,
            private changeDetector: ChangeDetectorRef, @Inject(AppConfig) private config: IAppConfig) {
            super(componentCommService, changeDetector);
    }

    ngOnInit(): void {
        this.dateFormat = this.fieldModel.template.mode === 'year' ? this.config.yearFormat : this.config.dateFormat;
        this.parentSelectors = [
            this.element.nativeElement
        ];
    }

    ngAfterViewInit(): void {
        var self = this;
        setTimeout(() => {
            self.renderDatePicker = true;
            self.changeDetector.detectChanges();
        }, 100);
    }

    togglePopup(show?: boolean) {
        if (this.fieldModel.control.disabled) return;

        let newVal = !this.showDatepicker;
        if (typeof show === 'boolean') {
            newVal = show;
        }
        if (newVal) {
            const inputClientRect = this.element.nativeElement.querySelector('input').getBoundingClientRect();
            if (inputClientRect.top < this.datepickerHeight) {
                this.inputHeight = '-250px';
            } else {
                this.inputHeight = inputClientRect.height + 'px';
            }
        }
        this.showDatepicker = newVal;
    }

    closeOutside(event: any) {
        if (!this.showDatepicker) return;
        if (this.element.nativeElement.contains(event.target) || !document.body.contains(event.target)) {
            return;
        }
        this.togglePopup(false);
    }


    onDateChange(event: string) {
        this.togglePopup(false);
        this.dateModel = event;
        this.fieldModel.control.setValue(moment(event).format(this.dateFormat));
    }
}
