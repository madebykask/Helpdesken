import { OnInit, OnDestroy, Component, Input, ViewChild, Inject, LOCALE_ID } from "@angular/core";
import { BaseCaseField } from "../../../../models";
import { BaseControl } from "../base-control";
import { MbscDatetimeOptions } from "@mobiscroll/angular";
import { FormatWidth, getLocaleDateFormat } from "@angular/common";
import { UserSettingsApiService } from "src/app/services/api/user/user-settings-api.service";
import { switchMap, takeUntil } from "rxjs/operators";
import { Subject, of } from "rxjs";
import { DateTime } from "luxon";

@Component({
    selector: 'case-date-control',
    templateUrl: './case-date-control.component.html',
    styleUrls: ['./case-date-control.component.scss']
  })
  export class CaseDateComponent extends BaseControl implements OnInit, OnDestroy {
    @ViewChild('control') control: any;
    // @ViewChild('date') control: MbscDate;
    @Input() field: BaseCaseField<string>;
    DateTime: DateTime;
    value?: Date;
    options: MbscDatetimeOptions = {
      readOnly: true,
      disabled: true,
      returnFormat: 'iso8601',
      // dateFormat: getLocaleDateFormat(this.locale, FormatWidth.Medium)
    }
    private destroy$ = new Subject();

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
      this.init(this.field);
      this.initEvents()
    }

    ngOnDestroy(): void {
      this.destroy$.next();
      this.destroy$.complete();
    }

    private initEvents() {
      this.formControl.valueChanges 
        .pipe(switchMap((v?: any) => {
            this.value = v == null ? null : new Date(v);
            return of(v);
          }),
          takeUntil(this.destroy$)
        )
        .subscribe();

        // this.formControl.valueChanges // TODO: when date field is ready

    }
  }