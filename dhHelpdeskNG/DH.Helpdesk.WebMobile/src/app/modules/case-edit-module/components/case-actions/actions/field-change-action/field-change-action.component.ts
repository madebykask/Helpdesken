import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { CaseActionBaseComponent } from '../case-action-base.component';
import { CaseHistoryActionData, CaseEventType } from 'src/app/modules/case-edit-module/models';
import { StringUtil } from 'src/app/modules/shared-module/utils/string-util';
import { DateUtil } from 'src/app/modules/shared-module/utils/date-util';

@Component({
  selector: 'app-field-change-action',
  templateUrl: './field-change-action.component.html',
  styleUrls: ['./field-change-action.component.scss']
})
export class FieldChangeActionComponent extends CaseActionBaseComponent<CaseHistoryActionData> {

  constructor() { 
    super();
  }
  
  ngOnChanges(changes: SimpleChanges): void {
    // note: is not called for dynamic components!
  }

  get formattedValue(): string {
    let currentValue = this.data.currentValue;
            
    //check date type first
    if (DateUtil.isDate(currentValue)) {
        return DateUtil.formatToLocalDate(currentValue);
    }
    
    //process as a text
    let formattedValue = currentValue !== null && currentValue !== undefined ? currentValue.toString() : '';
    formattedValue = StringUtil.convertToHtml(formattedValue);
    return formattedValue;
  }

  get showField() {
    //show field label if its not know case field change
    return this.caseAction.type === CaseEventType.OtherChanges;
  }

  get data(): CaseHistoryActionData {
    return this.caseAction != null ? this.caseAction.data : null;
  }  
}
