import { Component, Input, ViewChild } from "@angular/core";
import { BaseCaseField, OptionItem } from "../../../../models";
import { BaseControl } from "../base-control";
import { takeUntil, switchMap } from "rxjs/operators";
import { Subject, of } from "rxjs";
import { FormStatuses } from "src/app/helpers/constants";

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

    ngOnInit(): void {
      // if(this.readOnly) set disabled/reaonly mode
      // this.text = this.getText(this.field.value);
      let formControl = this.getFormControl(this.field.name);
      this.control.disabled = formControl.disabled || this.disabled;
      formControl.statusChanges // track disabled state in form
        .pipe(
          switchMap((e: any) => {
            if(this.control.disabled != (formControl.status == FormStatuses.DISABLED)) {
              this.control.disabled = formControl.status == FormStatuses.DISABLED;
            }
            return of(e);
          }),
          takeUntil(this.destroy$)
        ).subscribe();
    }

    ngOnDestroy(): void {
      this.destroy$.next();
    }

    getText(id: any) {
      if (this.dataSource == null || this.dataSource.length === 0) {
         return ""
        };
      let items = this.dataSource.filter((elem: OptionItem) => elem.value == id);
      return  items.length > 0 ? items[0].text : "";
    }

    trackByFn(index, item: OptionItem) {
      return item.value;
    }
}
