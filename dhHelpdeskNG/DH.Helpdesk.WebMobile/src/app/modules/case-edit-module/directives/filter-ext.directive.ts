import { Directive, Host, Self, Optional, ViewContainerRef, ElementRef, Renderer, OnInit } from '@angular/core';
import { MbscSelect, MbscPopup, MbscSelectComponent } from '@mobiscroll/angular';

@Directive({
  selector: '[filterExt]'
})
export class FilterExtDirective implements OnInit {

  constructor(
    @Host() @Optional() private hostSel : MbscSelectComponent,
    private viewContainer: ViewContainerRef,
    private el: ElementRef, 
    private renderer: Renderer) {
  }

  ngOnInit(): void {
    if (this.hostSel && this.hostSel.options) {
      let showCallback = this.hostSel.options.onShow;
      this.hostSel.options.onShow = (event, inst) => {
        //todo: do customisations here

        //call handler if exists
        if (showCallback)
          showCallback(event, inst);
      };
    }
  }
}
