import { Pipe, PipeTransform } from '@angular/core';
import { CaseOverviewColumn } from '../../models';
/*
 * Raise the value exponentially
 * Takes an exponent argument that defaults to 1.
 * Usage:
 *   value | exponentialStrength:exponent
 * Example:
 *   {{ 2 | exponentialStrength:10 }}
 *   formats to: 1024
*/
@Pipe({name: 'getByKey'})
export class GetByKeyPipe implements PipeTransform {
  transform(cols: CaseOverviewColumn[], key: string ): string {
    if (key){
       var items = cols.filter(el => el.Key === key);
       return items && items.length ? items[0].StringValue : null;
    }    
    return null;
  }
}