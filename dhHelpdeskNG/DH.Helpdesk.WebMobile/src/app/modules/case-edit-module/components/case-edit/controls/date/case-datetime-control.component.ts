import { OnInit, OnDestroy, Component, Input, OnChanges, ViewChild, LOCALE_ID, Inject } from "@angular/core";
import { BaseCaseField } from "../../../../models";
import { BaseControl } from "../base-control";
import { MbscDatetimeOptions, MbscDatetime } from "@mobiscroll/angular";
import { getLocaleDateFormat, FormatWidth, getLocaleTimeFormat } from "@angular/common";

@Component({
    selector: 'case-datetime-control',
    templateUrl: './case-datetime-control.component.html',
    styleUrls: ['./case-datetime-control.component.scss']
  })
  export class CaseDateTimeComponent extends BaseControl implements OnInit, OnDestroy {
    @ViewChild('control') control: any;
    //@ViewChild('datetime') control: MbscDatetime
    @Input() field: BaseCaseField<string>;
    value?: Date;
    options: MbscDatetimeOptions = {
      readOnly: true,
      disabled: true,
      returnFormat: 'iso8601',
      //dateFormat: getLocaleDateFormat(this.locale, FormatWidth.Medium),
      //timeFormat: getLocaleTimeFormat(this.locale, FormatWidth.Short)
    }

    constructor(@Inject(LOCALE_ID) locale: string) {
      super();
      this.options.dateFormat = getLocaleDateFormat(locale, FormatWidth.Short)
        .replace(new RegExp('M', 'g'), 'm');//different format letters
      this.options.timeFormat = getLocaleTimeFormat(locale, FormatWidth.Short)
        .replace(new RegExp('m', 'g'), 'i');//different format letters
    }

    ngOnInit(): void {
      this.control.disabled = true;
      //this.control.instance.setVal(new Date(this.field.value)); 
      this.value = this.field.value == null ? null : new Date(this.field.value);
    }

    ngOnDestroy(): void {
    }

  }