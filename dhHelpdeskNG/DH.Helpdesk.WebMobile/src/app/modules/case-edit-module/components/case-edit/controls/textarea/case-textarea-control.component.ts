import { Component, Input, ViewChild } from "@angular/core";
import { BaseCaseField } from "../../../../models";
import { BaseControl } from "../base-control";
import { FormStatuses } from "src/app/modules/shared-module/constants";
import { switchMap, takeUntil } from "rxjs/operators";
import { of } from "rxjs";

@Component({
    selector: 'case-textarea-control',
    templateUrl: './case-textarea-control.component.html',
    styleUrls: ['./case-textarea-control.component.scss']
  })
  export class CaseTextareaComponent extends BaseControl {
    @ViewChild('input') control: any;
    @Input() field: BaseCaseField<string>;
    @Input() disabled = false;

    ngOnInit(): void {
      this.init(this.field);
      this.updateDisabledState();

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
    }
  }