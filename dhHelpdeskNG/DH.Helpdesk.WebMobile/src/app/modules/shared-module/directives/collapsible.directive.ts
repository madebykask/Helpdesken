import { Input, Directive, ElementRef, Renderer2, SimpleChanges } from '@angular/core';

@Directive({
  selector: '[collapsible]',
  exportAs: "collapsible"
})
export class CollapsibleDirective {

  @Input() isCollapsed = true;

  @Input() collapsedEl: HTMLElement;
  @Input() expandedEl: HTMLElement;

  constructor(private el: ElementRef, private renderer: Renderer2) {
  }

  ngOnChanges(changes: SimpleChanges): void {
    // since we reference child divs via input properties they get available late after all other events
    if (changes.collapsedEl && changes.collapsedEl.firstChange) {
      if (this.isCollapsed) {
        setTimeout(() => this.collapse(), 300);
      } else {
        setTimeout(() => this.expand(), 300);
      }
    }
  }

  collapse() {
      this.showElement(this.collapsedEl);
      this.hideElement(this.expandedEl);
      this.isCollapsed = true;
  }

  expand() {
    this.showElement(this.expandedEl);
    this.hideElement(this.collapsedEl);
    this.isCollapsed = false;
  }

  private showElement(el: HTMLElement) {
    this.renderer.setStyle(el, 'display', '');
  }

  private hideElement(el: HTMLElement) {
    this.renderer.setStyle(el, 'display', 'none');
  }
}
