import { Component, Input, ViewChild } from "@angular/core";
import { BaseCaseField, OptionItem } from "../../../../models";
import { BaseControl } from "../base-control";
import { takeUntil, switchMap } from "rxjs/operators";
import { Subject, of } from "rxjs";
import { FormStatuses } from "src/app/helpers/constants";
import { MbscSelectOptions } from "@mobiscroll/angular";
import { TranslateService } from "@ngx-translate/core";
import { FormControl, AbstractControl } from "@angular/forms";

@Component({
    selector: 'case-dropdown-control',
    templateUrl: './case-dropdown-control.component.html',
    styleUrls: ['./case-dropdown-control.component.scss']
  })
  export class CaseDropdownComponent extends BaseControl {
    @ViewChild('control') control: any;
    @Input() field: BaseCaseField<number>;
    @Input() dataSource: OptionItem[] = [];
    @Input() disabled = false;
    text: string = "";
    
    private destroy$ = new Subject();
    private settings: MbscSelectOptions = {
      buttons: ['set'],
      setText: '',
      headerText: () => this.getHeader,
      onItemTap: (event, inst) => {
          if (event.selected) {
            inst.select();
          }
        }
    }

    constructor(private _ngxTranslateService: TranslateService) {
      super();
    }

    ngOnInit(): void {
      //apply translations
      //this.control.setText = this._ngxTranslateService.instant("Välj");
      // this.control.cancelText  = this._ngxTranslateService.instant("Avbryt");

      if (!this.dataSource) {
        this.dataSource = [];
      };
      this.init(this.field.name);
      this.control.disabled = this.formControl.disabled || this.disabled;
      if (!this.formControl.value || (this.formControl.value && !this.isRequired)) {
        this.addEmptyItem();
      }
      this.formControl.statusChanges // track disabled state in form
        .pipe(
          switchMap((e: any) => {
            if(this.control.disabled != (this.formControl.status == FormStatuses.DISABLED)) {
              this.control.disabled = this.formControl.status == FormStatuses.DISABLED;
            }
            return of(e);
          }),
          takeUntil(this.destroy$)
        ).subscribe();
    }

    ngOnDestroy(): void {
      this.destroy$.next();
    }

    addEmptyItem(): any {
      this.dataSource.unshift(new OptionItem('',''));
    }

    getText(id: any) {
      if (this.dataSource == null || this.dataSource.length === 0) {
         return ''
        };
      let items = this.dataSource.filter((elem: OptionItem) => elem.value == id);
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
}
