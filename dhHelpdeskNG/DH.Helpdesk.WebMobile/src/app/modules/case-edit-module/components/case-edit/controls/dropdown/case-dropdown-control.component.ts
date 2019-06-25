import { Component, Input, ViewChild, OnChanges, SimpleChanges } from '@angular/core';
import { BaseControl } from '../base-control';
import { takeUntil, map } from 'rxjs/operators';
import { BehaviorSubject } from 'rxjs';
import { MbscSelectOptions, MbscSelect } from '@mobiscroll/angular';
import { CommunicationService, Channels, CaseFieldValueChangedEvent } from 'src/app/services/communication';
import { OptionItem } from 'src/app/modules/shared-module/models';
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
    settings: MbscSelectOptions = {
      filter: true,
      filterPlaceholderText: this.ngxTranslateService.instant('Skriv för att filtrera'),
      filterEmptyText: this.ngxTranslateService.instant('Inget resultat'),
      setText: this.ngxTranslateService.instant('Välj'),
      cancelText: this.ngxTranslateService.instant('Avbryt'),
      display: 'center',
      theme: 'mobiscroll',
      height: 30,
      buttons: ['cancel'],
      headerText: () => this.getHeader,
      onInit: (event, inst) => {
        if (this.formControl && this.formControl.fieldInfo.isReadonly) {
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
        this.commService.publish(Channels.CaseFieldValueChanged, new CaseFieldValueChangedEvent(value, event.valueText, this.fieldName));
      }
    };

    localDataSource: OptionItem[] = [];

    constructor(private commService: CommunicationService,
      private ngxTranslateService: TranslateService) {
      super();
    }

    get getHeader(): string {
      const defaultValue = '';
      return this.formControl ? this.formControl.label || defaultValue : defaultValue;
    }

    ngOnInit(): void {
      this.initDataSource();
      this.init(this.fieldName);
      this.updateDisabledState();
      if (this.disabled) { // will be removed latter, when all fields are implemented
        this.formControl.disable({onlySelf: true, emitEvent: false});
      }
      this.initEvents();
    }

    private initDataSource() {
      if (!this.dataSource) {
        this.localDataSource = [];
      }
    }

    private initEvents() {
      // track disabled state in form
      this.formControl.statusChanges.pipe(
          takeUntil(this.destroy$)
        ).subscribe((e: any) => {
          if (this.selectControl.disabled !== this.isFormControlDisabled) {
            this.updateDisabledState();
          }
        });

      this.dataSource.pipe(
        map(((options) => {
          options = options || [];
          this.addEmptyIfNotExists(options);
          return options;
        })),
          takeUntil(this.destroy$)
        ).subscribe((options) => {
          this.localDataSource = options;
          this.resetValueIfNeeded(options);
        });
    }

    private updateDisabledState() {
      this.selectControl.disabled = this.formControl.disabled || this.disabled;
    }

    private addEmptyIfNotExists(options) {
      if (!options.some((i) => i.value === '')) {
        this.addEmptyItem(options);
      }
    }

    private resetValueIfNeeded(options: OptionItem[]) {
      if (options.filter((i) => String(i.value) === String(this.formControl.value)).length === 0) {
        this.formControl.setValue('');
      }
    }

    private addEmptyItem(options: OptionItem[]): any {
      options.unshift(new OptionItem('', '--'));
    }

    ngOnDestroy(): void {
      this.onDestroy();
    }

    trackByFn(index, item: OptionItem) {
      return item.value;
    }
}
