import { Pipe, PipeTransform } from '@angular/core';
import { CaseOverviewColumn } from '../models/cases-overview/cases-overview-item.model';

@Pipe({name: 'getByKey'})
export class GetByKeyPipe implements PipeTransform {
  transform(cols: CaseOverviewColumn[], key: string ): string {
    const defaultValue = null;
    if (key) {
       const items = cols.filter(el => el.key === key);
       return items && items.length ?
        (items[0].dateTimeValue != null ? items[0].dateTimeValue : items[0].stringValue) :
         defaultValue;
    }
    return defaultValue;
  }
}

@Pipe({name: 'caseHasValue'})
export class CaseHasValuePipe implements PipeTransform {
  transform(cols: CaseOverviewColumn[], key: string ): boolean {
    if (key) {
       const items = cols.filter(el => el.key === key);
       return items && items.length ?
        (items[0].dateTimeValue != null || (items[0].stringValue != null && items[0].stringValue.trim() != '')) :
         false;
    }
    return false;
  }
}