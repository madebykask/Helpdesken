import { Component, Input, ViewChild } from '@angular/core';
import { BaseControl } from '../base-control';
import { takeUntil } from 'rxjs/operators';
import { untilDestroyed } from 'ngx-take-until-destroy';

@Component({
    selector: 'case-switch-control',
    templateUrl: './case-switch-control.component.html',
    styleUrls: ['./case-switch-control.component.scss']
  })
  export class CaseSwitchComponent extends BaseControl<boolean> {
    @ViewChild('control', { static: true }) control: any;
    @Input() description = '';
    @Input() disabled = false;

    ngOnInit(): void {
      this.init(this.fieldName);
      this.updateDisabledState();

      this.initEvents();
    }

    ngOnDestroy(): void {
    }

    private updateDisabledState() {
      this.control.disabled = this.formControl.disabled || this.disabled;
    }

    private initEvents() {
      // track disabled state in form
      this.formControl.statusChanges.pipe(
        untilDestroyed(this)
        ).subscribe(e => {
          if (this.control.disabled !== this.isFormControlDisabled) {
            this.updateDisabledState();
          }
        });
    }
  }
