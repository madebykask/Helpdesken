import { Component, Input, ViewChild, OnChanges, SimpleChanges } from "@angular/core";
import { BaseControl } from "../base-control";
import { takeUntil } from "rxjs/operators";
import { BehaviorSubject } from "rxjs";
import { MbscSelectOptions, MbscSelect } from "@mobiscroll/angular";
import { CommunicationService, Channels, DropdownValueChangedEvent } from "src/app/services/communication";
import { OptionItem } from "src/app/modules/shared-module/models";
import { FormStatuses } from "src/app/modules/shared-module/constants";
import { TranslateService } from '@ngx-translate/core';

@Component({
    selector: 'case-dropdown-control',
    templateUrl: './case-dropdown-control.component.html',
    styleUrls: ['./case-dropdown-control.component.scss']
  })
export class CaseDropdownComponent extends BaseControl<number> {
    @ViewChild('control') selectControl: MbscSelect;
    @Input() dataSource: BehaviorSubject<OptionItem[]>;
    @Input() disabled = false;
    
    // select settings
    settings: any = {
      filter: true,
      filterPlaceholderText: this.ngxTranslateService.instant('Skriv för att filtrera'),
      filterEmptyText: this.ngxTranslateService.instant('Inget resultat'),
      display: 'center',
      theme: 'mobiscroll',
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

    get getHeader(): string {
      const defaultValue = '';
      if (!this.field) {
        return defaultValue;
      }
      return this.formControl.label || defaultValue;
    }  

    ngOnInit(): void {
      // apply translations
      this.selectControl.setText = this.ngxTranslateService.instant('Välj');
      this.selectControl.cancelText = this.ngxTranslateService.instant('Avbryt');

      this.initDataSource();
      this.init(this.field);
      this.updateDisabledState();
      this.initEvents();
    }

    private initDataSource() {
      if (!this.dataSource) {
        this.localDataSource = [];
      };
    }

    private initEvents() {
      // track disabled state in form
      this.formControl.statusChanges.pipe(
          takeUntil(this.destroy$)
        ).subscribe((e: any) => {
          if (this.selectControl.disabled != this.isFormControlDisabled) {
            this.updateDisabledState();
          }
        });
        
        this.dataSource.pipe(
          takeUntil(this.destroy$)
        ).subscribe((options) => {
          
          //clear prev selected values
          if (this.selectControl && this.selectControl.instance)
            this.selectControl.instance.clear(); 

          options = options || [];
          // if (!this.formControl.value || (this.formControl.value && !this.isRequired)) {
            this.addEmptyIfNotExists(options);
          // }
          this.localDataSource = options;
          this.resetValueIfNeeded(options);
        });
    }

    private updateDisabledState() {
      this.selectControl.disabled = this.formControl.disabled || this.disabled;
    }

    private get isFormControlDisabled() {
      return this.formControl.status == FormStatuses.DISABLED;
    }

    private addEmptyIfNotExists(options) {
      if (!options.some((i) => i.value === '')) {
        this.addEmptyItem(options);
      }
    }

    private resetValueIfNeeded(options: OptionItem[]) {
      if (options.filter((i) => String(i.value) == String(this.formControl.value)).length == 0) {
        this.formControl.setValue('');
      }
    }

    private addEmptyItem(options: OptionItem[]): any {
      options.unshift(new OptionItem('',''));
    }

    ngOnDestroy(): void {
      this.onDestroy();
    }

    trackByFn(index, item: OptionItem) {
      return item.value;
    }
}
