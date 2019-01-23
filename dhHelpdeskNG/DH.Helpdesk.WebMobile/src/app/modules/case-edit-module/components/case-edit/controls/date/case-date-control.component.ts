import { OnInit, OnDestroy, Component, Input, ViewChild, Inject, LOCALE_ID } from "@angular/core";
import { BaseCaseField } from "../../../../models";
import { BaseControl } from "../base-control";
import { MbscDatetimeOptions, MbscDate } from "@mobiscroll/angular";
import { FormatWidth, getLocaleDateFormat } from "@angular/common";
import { UserSettingsApiService } from "src/app/services/api/user/user-settings-api.service";

@Component({
    selector: 'case-date-control',
    templateUrl: './case-date-control.component.html',
    styleUrls: ['./case-date-control.component.scss']
  })
  export class CaseDateComponent extends BaseControl implements OnInit, OnDestroy {
    @ViewChild('control') control: any;
    // @ViewChild('date') control: MbscDate;
    @Input() field: BaseCaseField<string>;
    value?: Date;
    options: MbscDatetimeOptions = {
      readOnly: true,
      disabled: true,
      returnFormat: 'iso8601',
      // dateFormat: getLocaleDateFormat(this.locale, FormatWidth.Medium)
    }

    constructor(@Inject(LOCALE_ID) locale: string,
                private userSettingsService: UserSettingsApiService) {
      super();
      
      this.options.dateFormat = 
          getLocaleDateFormat(locale, FormatWidth.Short)
            .replace(new RegExp('M', 'g'), 'm');// different format letters;
    }

    ngOnInit(): void {
      this.control.disabled = true;
      this.value = this.field.value == null ? null : new Date(this.field.value);
    }

    ngOnDestroy(): void {
    }

  }