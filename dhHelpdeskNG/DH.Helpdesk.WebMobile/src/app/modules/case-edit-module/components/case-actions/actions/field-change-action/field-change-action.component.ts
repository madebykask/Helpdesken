import { Component, OnInit, Input } from '@angular/core';
import { CaseActionBaseComponent } from '../case-action-base.component';
import { CaseHistoryActionData, CaseEventType } from 'src/app/modules/case-edit-module/models';
import { StringUtil } from 'src/app/modules/shared-module/Utils/string-util';
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
    let currentValue = (this.data.currentValue || '').toString()
    var dateValue = moment(currentValue);
    if (dateValue.isValid())
        return dateValue.format("L LTS");
    
    //process as a text
    currentValue = StringUtil.convertToHtml(currentValue);
    return currentValue;
  } 

  get showField(){
    //show field label if its not know case field change
    return this.caseAction.type === CaseEventType.OtherChanges;
  }

  get data(): CaseHistoryActionData {
    return this.caseAction != null ? this.caseAction.data : null;
  }  
}
