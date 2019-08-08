import { Component, Input, ViewChild } from '@angular/core';
import { BaseControl } from '../base-control';
import { takeUntil } from 'rxjs/operators';

@Component({
    selector: 'case-switch-control',
    templateUrl: './case-switch-control.component.html',
    styleUrls: ['./case-switch-control.component.scss']
  })
  export class CaseSwitchComponent extends BaseControl<boolean> {
    @ViewChild('control', { static: false }) control: any;
    @Input() description = '';
    @Input() disabled = false;

    ngOnInit(): void {
      this.init(this.fieldName);
      this.updateDisabledState();

      this.initEvents();
    }

    ngOnDestroy(): void {
      this.onDestroy();
    }

    private updateDisabledState() {
      this.control.disabled = this.formControl.disabled || this.disabled;
    }

    private initEvents() {
      // track disabled state in form
      this.formControl.statusChanges.pipe(
          takeUntil(this.destroy$)
        ).subscribe(e => {
          if (this.control.disabled !== this.isFormControlDisabled) {
            this.updateDisabledState();
          }
        });
    }
  }
