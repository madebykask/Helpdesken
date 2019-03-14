import { Component, Input, ViewChild } from "@angular/core";
import { BaseCaseField } from "../../../../models";
import { BaseControl } from "../base-control";
import { takeUntil, switchMap } from "rxjs/operators";
import { Subject, of, BehaviorSubject } from "rxjs";
import { MbscSelectOptions } from "@mobiscroll/angular";
import { CommunicationService, Channels, DropdownValueChangedEvent } from "src/app/services/communication";
import { OptionItem } from "src/app/modules/shared-module/models";
import { FormStatuses } from "src/app/modules/shared-module/constants";
import { TranslateService } from '@ngx-translate/core';

@Component({
    selector: 'case-dropdown-control',
    templateUrl: './case-dropdown-control.component.html',
    styleUrls: ['./case-dropdown-control.component.scss']
  })
export class CaseDropdownComponent extends BaseControl {
    @ViewChild('control') control: any;
    @Input() field: BaseCaseField<number>;
    @Input() dataSource: BehaviorSubject<OptionItem[]>;
    @Input() disabled = false;
    settings: MbscSelectOptions = {
      display: 'center',
      headerText: () => this.getHeader,
      onInit: (event, inst) => {
        if (this.field.isReadonly) {
          inst.disable();
        }
      },
      onItemTap: (event, inst) => {
        if (event.selected) {
          inst.select();
        }
      },
      onSet: (event, inst) => {
        const value = inst.getVal();
        this.commService.publish(Channels.DropdownValueChanged, new DropdownValueChangedEvent(value, event.valueText, this.field.name));
      }
    }
    localDataSource: OptionItem[] = [];

    constructor(private commService: CommunicationService,
      private ngxTranslateService: TranslateService) {
      super();
    }

    ngOnInit(): void {
      // apply translations
      this.control.setText = this.ngxTranslateService.instant('Välj');
      this.control.cancelText = this.ngxTranslateService.instant('Avbryt');

      this.initDataSource();
      this.init(this.field);
      this.updateDisabledState();
      this.initEvents();
    }

    ngOnDestroy(): void {
      this.onDestroy();
    }

    getText(id: any) {
      if (this.dataSource == null || this.localDataSource.length === 0) {
         return ''
        };
      let items = this.localDataSource.filter((elem: OptionItem) => elem.value == id);
      return  items.length > 0 ? items[0].text : '';
    }

    trackByFn(index, item: OptionItem) {
      return item.value;
    }

    public get getHeader(): string {
      const defaultValue = '';
      if (!this.field) {
        return defaultValue;
      }
      return this.field.label || defaultValue;
    }

    private initDataSource() {
      if (!this.dataSource) {
        this.localDataSource = [];
      };
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

        this.dataSource.pipe(
          switchMap((options) => {
            options = options || [];
            if (!this.formControl.value || (this.formControl.value && !this.field.isRequired)) {
              this.addEmptyItem(options);
            }
            this.localDataSource = options;

            this.resetValueIfNeeded(options);
            return of(options);
          }),
          takeUntil(this.destroy$)
        )
        .subscribe();
    }

  private resetValueIfNeeded(options: OptionItem[]) {
    if (options.filter((i) => String(i.value) == String(this.formControl.value)).length == 0) {
      this.formControl.setValue('');
    }
  }

  private addEmptyItem(options: OptionItem[]): any {
    options.unshift(new OptionItem('',''));
  }

}
