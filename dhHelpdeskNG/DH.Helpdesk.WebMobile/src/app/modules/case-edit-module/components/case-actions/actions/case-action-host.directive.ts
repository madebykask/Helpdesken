import { Directive, ViewContainerRef } from '@angular/core';
import { CaseActionContainerComponent } from '../case-action-container.component';

@Directive({
  selector: '[case-action-host]'
})
export class CaseActionHostDirective {
    constructor(public viewContainerRef: ViewContainerRef,
                private hostComponent: CaseActionContainerComponent) {
        this.hostComponent.setViewContainer(viewContainerRef);
    }
}
