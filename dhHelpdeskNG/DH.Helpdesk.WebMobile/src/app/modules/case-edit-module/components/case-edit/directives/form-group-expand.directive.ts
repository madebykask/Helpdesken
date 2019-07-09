import { Directive, EventEmitter, ElementRef, Renderer2, Output } from '@angular/core';

@Directive({
  selector: 'mbsc-form-group-title'
})
export class MbscFormGroupExpandDirective {
  private isOpened?: boolean;

  @Output() onClick = new EventEmitter<any>();

  constructor(private elem: ElementRef, private renderer: Renderer2) {
    // subcribe on html element click
    this.renderer.listen(this.elem.nativeElement, 'click', (ev) => {
      if (this.isOpened == null) {
        this.isOpened = this.elem.nativeElement.getAttribute('aria-expanded') === 'true';
      }
      this.elem.nativeElement.focus();
      this.onClick.emit({ isOpening: !this.isOpened, event: ev});
      this.isOpened = !this.isOpened;
    });
  }
}
