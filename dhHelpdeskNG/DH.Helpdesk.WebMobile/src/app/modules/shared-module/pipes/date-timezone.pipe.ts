import { Pipe, PipeTransform } from '@angular/core';
import { DateUtil } from '../utils/date-util';
import { DateTime } from 'luxon';

@Pipe({name: 'dateTimeFormat'})
export class DateTimeFormatPipe implements PipeTransform {
  transform(date?: Date | string): string {
    if (date == null) return "";
    return DateUtil.formatDate(date, DateTime.DATETIME_SHORT_WITH_SECONDS);
  }
}

@Pipe({name: 'dateFormat'})
export class DateFormatPipe implements PipeTransform {
  transform(date?: Date | string): string {
    if (date == null) return "";
    return DateUtil.formatDate(date, DateTime.DATE_SHORT);
  }
}