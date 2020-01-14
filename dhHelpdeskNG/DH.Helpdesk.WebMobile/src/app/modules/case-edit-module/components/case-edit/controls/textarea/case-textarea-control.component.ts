import { Component, Input, ViewChild, ElementRef, Renderer2, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { BaseControl } from '../base-control';
import { MbscTextarea } from '@mobiscroll/angular';
import { untilDestroyed } from 'ngx-take-until-destroy';

@Component({
    selector: 'case-textarea-control',
    templateUrl: './case-textarea-control.component.html',
    styleUrls: ['./case-textarea-control.component.scss']
  })
  export class CaseTextareaComponent extends BaseControl<string> implements OnInit, AfterViewInit, OnDestroy {
    @ViewChild('input', { static: true }) control: MbscTextarea;
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
        const inputControl = this.elem.nativeElement.querySelector('textarea');
        if (inputControl) {
          this.renderer.setAttribute(inputControl, 'maxlength', this.formControl.fieldInfo.maxLength.toString());
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
        .subscribe((e) => {
          if (this.control.disabled !== this.formControl.isDisabled) {
            this.updateDisabledState();
          }
        });
    }
  }
