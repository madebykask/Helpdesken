import { Component, Input, ViewChild, ElementRef, Renderer2, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { BaseControl } from '../base-control';
import { MbscTextarea } from '@mobiscroll/angular';
import { takeUntil } from 'rxjs/operators';

@Component({
    selector: 'case-textarea-control',
    templateUrl: './case-textarea-control.component.html',
    styleUrls: ['./case-textarea-control.component.scss']
  })
  export class CaseTextareaComponent extends BaseControl<string> implements OnInit, AfterViewInit, OnDestroy {
    @ViewChild('input') control: MbscTextarea;
    @Input() disabled = false;

    constructor(private elem: ElementRef, private renderer: Renderer2) {
      super();
    }

    ngOnInit(): void {
      this.init(this.field);
      this.updateDisabledState();

      this.initEvents();
    }

    ngAfterViewInit(): void {
      if (this.field.maxLength) {
        const inputControl = this.elem.nativeElement.querySelector('textarea');
        if (inputControl) {
          this.renderer.setAttribute(inputControl, 'maxlength', this.field.maxLength.toString());
        }
      }
    }

    ngOnDestroy(): void {
      this.onDestroy();
    }

    private updateDisabledState() {
      this.control.disabled = this.formControl.disabled || this.disabled;
    }

    private initEvents() {
      this.formControl.statusChanges // track disabled state in form
        .pipe(
          takeUntil(this.destroy$)
        )
        .subscribe((e) => {
          if (this.control.disabled !== this.formControl.isDisabled) {
            this.updateDisabledState();
          }
        });
    }
  }
