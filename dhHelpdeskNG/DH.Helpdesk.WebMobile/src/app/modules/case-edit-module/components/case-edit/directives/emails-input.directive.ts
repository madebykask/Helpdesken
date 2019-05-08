import { Directive, HostListener, ElementRef, Renderer2, Host, Optional, OnInit } from '@angular/core';
import { NgControl } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { MbscTextarea } from '@mobiscroll/angular';
//import { MbscInput } from '@mobiscroll/angular';

@Directive({
  selector: '[emails-input]'
})
export class EmailsInputDirective implements OnInit {

  private destroy$ = new Subject<any>();

  constructor(
    @Host() @Optional() private cmp: MbscTextarea,
    private ngControl: NgControl,
    private el: ElementRef,
    private renderer: Renderer2) {
     //check form control value
    const val = this.formControl ? this.formControl.value : null;
  }

  ngOnInit(): void {

    if (this.cmp) {
      this.cmp.onChange = (e) => {
        console.log(`>>> [emails-input]: onChange. Value: ${e}`);
      };

      this.cmp.valueChangeEmitter.pipe(
        takeUntil(this.destroy$)
      ).subscribe(e => {
        console.log(`>>> [emails-input]: MbscTextarea value changed. Value: ${e}`);
      });
    }

    if (this.formControl) {
      this.formControl.valueChanges.pipe(
        takeUntil(this.destroy$)
      ).subscribe(e => {
        console.log(`>>> [emails-input]: form control value changed. Value: ${e}`);
      });
    }
  }

  get formControl() {
    return this.ngControl.control;
  }

  @HostListener('blur', ['$event'])
  onKeypress(e: FocusEvent) {
    console.log('>> blur event ');
    const element = this.el.nativeElement;
    const elementValue = element.value;
    const val = this.formControl ? this.formControl.value : null;
  }

  @HostListener('paste', ['$event'])
  onPaste(event: ClipboardEvent) {
    event.preventDefault();
    const pastedInput = event.clipboardData.getData('text/plain');

    //todo: format input text to valid emails
    const formattedText = pastedInput.replace(/[^\d]/g, ''); // get a digit-only string
    //this.formControl.setValue(this.formControl.value.toLowerCase());
    document.execCommand('insertText', false, pastedInput);
  }

  isValidEmailAddress(val) {
    const pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(val.trim());
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

}
