import { Component, OnInit, Input } from '@angular/core';
import { CaseActionBaseComponent } from '../case-action-base.component';
import { CaseHistoryActionData } from 'src/app/modules/case-edit-module/models';

@Component({
  selector: 'app-field-change-action',
  templateUrl: './field-change-action.component.html',
  styleUrls: ['./field-change-action.component.scss']
})
export class FieldChangeActionComponent extends CaseActionBaseComponent<CaseHistoryActionData> implements OnInit {

  constructor() { 
    super();
  }

  ngOnInit() {
  }

  get data(): CaseHistoryActionData {
    return this.caseAction != null ? this.caseAction.Data : null;
  }
}
