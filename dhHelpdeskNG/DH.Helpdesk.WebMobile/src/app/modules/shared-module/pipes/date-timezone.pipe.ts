import { Pipe, PipeTransform } from '@angular/core';
import { DateUtil } from '../Utils/date-util';

@Pipe({name: 'dateTz'})
export class DateTimezonePipe implements PipeTransform {
  transform(date?: Date, format?: string): string {
    if (date == null) return "";
    return DateUtil.formatDate(date, format);
  }
}