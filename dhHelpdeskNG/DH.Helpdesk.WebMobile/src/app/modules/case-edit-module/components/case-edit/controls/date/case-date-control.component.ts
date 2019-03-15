import { Component, Input, ViewChild, Inject, LOCALE_ID } from "@angular/core";
import { BaseCaseField } from "../../../../models";
import { BaseControl } from "../base-control";
import { MbscCalendarOptions, MbscCalendar } from "@mobiscroll/angular";
import { FormatWidth, getLocaleDateFormat } from "@angular/common";
import { switchMap, takeUntil } from "rxjs/operators";
import { of } from "rxjs";
import { DateTime } from "luxon";
import { FormStatuses } from "src/app/modules/shared-module/constants";
import { TranslateService } from "@ngx-translate/core";
import { DateUtil } from "src/app/modules/shared-module/utils/date-util";

@Component({
    selector: 'case-date-control',
    templateUrl: './case-date-control.component.html',
    styleUrls: ['./case-date-control.component.scss']
  })
  export class CaseDateComponent extends BaseControl<string> {
    // @ViewChild('control') control: any;
    @ViewChild('date') control: MbscCalendar;
    @Input() disabled = false;
    value?: string;
    options: MbscCalendarOptions = {
      display: 'center',
      returnFormat: 'iso8601',
      formatValue: (data) => {
        // default format is mm/dd/yy so data contains [m,d,y]
        let dateIso = data[2] + '-' + (data[0] < 9 ? '0' : '') + (data[0] + 1) + '-' + (data[1] < 10 ? '0' : '') + data[1];
        return DateUtil.formatDate(dateIso, DateTime.DATE_SHORT);
      },
      parseValue: (value?: string) => {
        let d = value ? DateTime.fromISO(value) : DateTime.local() // Default value
        return [d.month, d.day, d.year];
      },
      onInit: (event, inst) => {
        if (this.formControl.disabled || this.disabled) {
          inst.disable();
        }
      },
      onSet: (event, inst) => {
        this.formControl.setValue(inst.getVal());
      }
    }

    constructor(@Inject(LOCALE_ID) locale: string, private ngxTranslateService: TranslateService) {
      super();
    }

    ngOnInit(): void {
      this.control.setText = this.ngxTranslateService.instant('Välj');
      this.control.cancelText = this.ngxTranslateService.instant('Avbryt');

      this.value = this.field.value;
      // this.updateDisabledState();
      this.init(this.field);
      this.initEvents()
    }

    ngOnDestroy(): void {
      this.onDestroy();
    }

    private updateDisabledState() {
      this.control.disabled = this.formControl.disabled || this.disabled;
    }

    private get isFormControlDisabled() {
      return this.formControl.status == FormStatuses.DISABLED;
    }

    private initEvents() {
      this.formControl.statusChanges // track disabled state in form
      .pipe(switchMap((e: any) => {
          if (this.control.disabled != this.isFormControlDisabled) {
            this.updateDisabledState();
          }
          return of(e);
        }),
        takeUntil(this.destroy$)
      )
      .subscribe();

      this.formControl.valueChanges 
        .pipe(switchMap((v?: any) => {
            this.value = v;
            return of(v);
          }),
          takeUntil(this.destroy$)
        )
        .subscribe();
    }
  }