import { Pipe, PipeTransform } from '@angular/core';
import { CaseActionDataType, CaseAction, CaseEventType } from '../models';

@Pipe({
  name: 'actionsFilter',
  pure: false
})
export class ActionsFilterPipe implements PipeTransform {
  transform(value: any, filter:string): any {
    let actions = value as CaseAction<CaseActionDataType>[];
    if (actions && actions.length)  {
        if (filter === 'main') {
           value = actions.filter(x => x.type !== CaseEventType.OtherChanges);
        } else if (filter === 'other'){
           value = actions.filter(x => x.type === CaseEventType.OtherChanges);
        }        
    }
    return value;
  }
}
