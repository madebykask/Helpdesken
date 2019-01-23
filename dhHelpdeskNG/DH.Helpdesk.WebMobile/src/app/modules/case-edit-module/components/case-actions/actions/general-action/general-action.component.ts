import { Component, OnInit } from '@angular/core';
import { CaseActionBaseComponent } from '../case-action-base.component';
import { GenericActionData, CaseActionEvents } from 'src/app/modules/case-edit-module/models';

@Component({
  selector: 'app-general-action',
  templateUrl: './general-action.component.html',
  styleUrls: ['./general-action.component.scss']
})
export class GeneralActionComponent extends CaseActionBaseComponent<GenericActionData> implements OnInit {

  constructor() {
    super();
  }

  ngOnInit() {
  }
  
  get data(): GenericActionData {
     return this.caseAction != null ? this.caseAction.Data : null;
  }
}


