import { Component, OnInit, Input } from '@angular/core';
import { CaseActionBaseComponent } from '../case-action-base.component';
import { CaseHistoryActionData, CaseEventType } from 'src/app/modules/case-edit-module/models';
import * as moment from 'moment-timezone';

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

get formattedValue() {
  //format date
  if (moment.isDate(this.data.currentValue))
    return moment(this.data.currentValue).format("L LTS");

  return this.data.currentValue.toString();
}

get showField(){
  //show field label if its not know case field change
  return this.caseAction.type === CaseEventType.OtherChanges;
}

  get data(): CaseHistoryActionData {
    return this.caseAction != null ? this.caseAction.data : null;
  }  
}
