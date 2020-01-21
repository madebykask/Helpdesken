import { Component, Input, ViewChild, ElementRef, Renderer2 } from '@angular/core';
import { BaseControl } from '../base-control';
import { takeUntil } from 'rxjs/operators';
import { untilDestroyed } from 'ngx-take-until-destroy';

@Component({
    selector: 'case-textbox-control',
    templateUrl: './case-textbox-control.component.html',
    styleUrls: ['./case-textbox-control.component.scss']
  })
  export class CaseTextboxComponent extends BaseControl<string> {
    @ViewChild('input', { static: true }) control: any;
    @Input() disabled = false;

    constructor(private elem: ElementRef, private renderer: Renderer2) {
      super();
    }

    ngOnInit(): void {
      this.init(this.fieldName);
      this.updateDisabledState();

      this.initEvents();
    }

    ngAfterViewInit(): void {
      if (this.formControl && this.formControl.fieldInfo.maxLength) {
        const inputControl = this.elem.nativeElement.querySelector('input');
        if (inputControl) {
          this.renderer.setAttribute(inputControl, 'maxlength', this.formControl.fieldInfo.maxLength.toString());
          // inputControl.maxLength = this.field.maxLength;
        }
      }
    }

    ngOnDestroy(): void {
    }

    private updateDisabledState() {
      this.control.disabled = this.formControl.disabled || this.disabled;
    }

    private initEvents() {
      this.formControl.statusChanges // track disabled state in form
        .pipe(
          untilDestroyed(this)
        )
        .subscribe(e => {
          if (this.control.disabled !== this.isFormControlDisabled) {
            this.updateDisabledState();
          }
        });
    }
  }
