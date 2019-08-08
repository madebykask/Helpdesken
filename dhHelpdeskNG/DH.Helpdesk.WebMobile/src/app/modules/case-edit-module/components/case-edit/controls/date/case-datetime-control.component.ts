import { Component, ViewChild, LOCALE_ID, Inject } from '@angular/core';
import { BaseControl } from '../base-control';
//import { MbscDatetimeOptions, MbscDatetime } from "@mobiscroll/angular";
//import { getLocaleDateFormat, FormatWidth, getLocaleTimeFormat } from "@angular/common";

@Component({
    selector: 'case-datetime-control',
    templateUrl: './case-datetime-control.component.html',
    styleUrls: ['./case-datetime-control.component.scss']
  })
  export class CaseDateTimeComponent extends BaseControl<string> {
    @ViewChild('control', { static: true }) control: any;
    value?: Date;
    /*
    // @ViewChild('datetime') control: MbscDatetime
    options: MbscDatetimeOptions = {
      readOnly: true,
      disabled: true,
      returnFormat: 'iso8601',
      //dateFormat: getLocaleDateFormat(this.locale, FormatWidth.Medium),
      //timeFormat: getLocaleTimeFormat(this.locale, FormatWidth.Short)
    }
    */

    constructor(@Inject(LOCALE_ID) locale: string) {
      super();
      /*
      this.options.dateFormat = getLocaleDateFormat(locale, FormatWidth.Short)
        .replace(new RegExp('M', 'g'), 'm');//different format letters
      this.options.timeFormat = getLocaleTimeFormat(locale, FormatWidth.Short)
        .replace(new RegExp('m', 'g'), 'i');//different format letters 
      */
    }

    ngOnInit(): void {
      this.init(this.fieldName);
      this.control.disabled = true;
      //todo: shall we better use dateUtil.isDate and dateUtil.tryConvertToDate for code below?
      this.value = this.formControl && this.formControl.value ? new Date(this.formControl.value) : null;
      //this.control.instance.setVal(new Date(this.formControl.value));
    }

    ngOnDestroy(): void {
      this.onDestroy();
    }
  }
