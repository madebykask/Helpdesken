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
    return cols.filter(el => el.Key === key)[0].StringValue;
  }
}