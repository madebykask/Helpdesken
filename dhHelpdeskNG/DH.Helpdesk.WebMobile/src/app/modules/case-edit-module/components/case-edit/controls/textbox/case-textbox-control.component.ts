import { Component, Input, ViewChild } from "@angular/core";
import { BaseCaseField } from "../../../../models";
import { BaseControl } from "../base-control";
import { switchMap, takeUntil } from "rxjs/operators";
import { of, Subject } from "rxjs";
import { FormStatuses } from "src/app/modules/shared-module/constants";

@Component({
    selector: 'case-textbox-control',
    templateUrl: './case-textbox-control.component.html',
    styleUrls: ['./case-textbox-control.component.scss']
  })
  export class CaseTextboxComponent extends BaseControl<string> {
    @ViewChild('input') control: any;
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