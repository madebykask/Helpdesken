import { Component, ViewContainerRef, ComponentFactoryResolver, Input, Type, OnInit, OnDestroy } from '@angular/core';
import { CaseAction, CaseLogActionData, CaseHistoryActionData, GenericActionData, CaseActionDataType } from '../../models';
import { FieldChangeActionComponent } from './actions/field-change-action/field-change-action.component';
import { LogNoteActionComponent } from './actions/log-note-action/log-note-action.component';
import { GeneralActionComponent } from './actions/general-action/general-action.component';
import { CaseActionBaseComponent } from './actions/case-action-base.component';

@Component({
  selector: 'case-action-container',
  template: `
    <ng-container case-action-host></ng-container>
  `
})
export class CaseActionContainerComponent implements OnInit, OnDestroy {

    @Input('caseKey') caseKey: string;
    @Input('caseAction') caseAction: CaseAction<CaseActionDataType>;

    private viewContainer: ViewContainerRef;

    setViewContainer(vc: ViewContainerRef): any {
      this.viewContainer = vc;
    }

    constructor(private componentFactoryResolver: ComponentFactoryResolver) {
    }

    ngOnInit(): void {
      if (this.viewContainer) {
        this.renderComponent(this.viewContainer);
      }
    }

    ngOnDestroy(): void {
      if (this.viewContainer && this.viewContainer.length) {
        this.viewContainer.detach(0);
      }
    }

    renderComponent(vc: ViewContainerRef) {
      const cmp = this.resolveComponent();
      const componentFactory = this.componentFactoryResolver.resolveComponentFactory(cmp);
      const componentRef = vc.createComponent(componentFactory);

      const componentInst = <CaseActionBaseComponent<CaseActionDataType>>componentRef.instance;
      //set component properties
      componentInst.caseKey = this.caseKey;
      componentInst.caseAction = this.caseAction;
  }

  private resolveComponent(): Type<CaseActionBaseComponent<CaseActionDataType>> {
    const actionData = this.caseAction.data;
    if (actionData instanceof CaseLogActionData) {
        return LogNoteActionComponent;
    } else if (actionData instanceof CaseHistoryActionData) {
        return FieldChangeActionComponent;
    } else if (actionData instanceof GenericActionData) {
        return GeneralActionComponent;
    }
    return GeneralActionComponent;
  }

}
