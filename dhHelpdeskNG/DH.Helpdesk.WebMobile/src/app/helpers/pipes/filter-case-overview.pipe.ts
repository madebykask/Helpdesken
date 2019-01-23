import { Pipe, PipeTransform } from '@angular/core';
import { CaseOverviewColumn } from 'src/app/models';

@Pipe({name: 'getByKey'})
export class GetByKeyPipe implements PipeTransform {
  transform(cols: CaseOverviewColumn[], key: string ): string {
    const defaultValue = null;
    if (key) {
       var items = cols.filter(el => el.key === key);
       return items && items.length ?
        (items[0].dateTimeValue != null ? items[0].dateTimeValue : items[0].stringValue) :
         defaultValue;
    }    
    return defaultValue;
  }
}