import { Component, Input, ViewChild } from '@angular/core';
import { BaseControl } from '../base-control';
import { MbscCalendarOptions, MbscCalendar } from '@mobiscroll/angular';
import { takeUntil } from 'rxjs/operators';
import { DateTime } from 'luxon';
import { TranslateService } from '@ngx-translate/core';
import { DateUtil } from 'src/app/modules/shared-module/utils/date-util';
import { untilDestroyed } from 'ngx-take-until-destroy';

@Component({
    selector: 'case-date-control',
    templateUrl: './case-date-control.component.html',
    styleUrls: ['./case-date-control.component.scss']
  })
  export class CaseDateComponent extends BaseControl<string> {
    @ViewChild('date', { static: true }) control: MbscCalendar;
    @Input() disabled = false;
    value?: string;

    options: MbscCalendarOptions = {
      theme: 'mobiscroll',
      display: 'center',
      returnFormat: 'iso8601',
      firstDay: 1,
      weekDays: 'short',
      setText: this.ngxTranslateService.instant('Välj'),
      clearText: this.ngxTranslateService.instant('Klar'),
      cancelText: this.ngxTranslateService.instant('Avbryt'),
      buttons: ['cancel'],
      setOnDayTap: true,
      formatValue: (data) => {
        // default format is mm/dd/yy so data contains [m,d,y]
        const dateIso = data[2] + '-' + (data[0] < 9 ? '0' : '') + (data[0] + 1) + '-' + (data[1] < 10 ? '0' : '') + data[1];
        return DateUtil.formatDate(dateIso, DateTime.DATE_SHORT);
      },
      parseValue: (value?: string) => {
        const d = value ? DateTime.fromISO(value) : DateTime.local(); // Default value
        return [d.month - 1, d.day, d.year];
      },
      onInit: (event, inst) => {
        if (this.formControl.disabled || this.disabled) {
          inst.disable();
        }
      },
      onSet: (event, inst) => {
        this.formControl.setValue(inst.getVal());
      }
    };

    constructor(private ngxTranslateService: TranslateService) {
      super();
    }

    ngOnInit(): void {
      this.init(this.fieldName);
      this.value = this.formControl.value;
      // this.updateDisabledState();

      this.initEvents();
    }

    ngOnDestroy(): void {}

    private updateDisabledState() {
      this.control.disabled = this.isFormControlDisabled || this.disabled;
    }

    private initEvents() {
      // track disabled state in form
      this.formControl.statusChanges.pipe(
        untilDestroyed(this)
      ).subscribe(e => {
        if (this.control.disabled !== this.isFormControlDisabled) {
          this.updateDisabledState();
        }
      });

      this.formControl.valueChanges.pipe(
        untilDestroyed(this)
        ).subscribe((v: any) => {
          this.value = v;
        });
    }
  }
