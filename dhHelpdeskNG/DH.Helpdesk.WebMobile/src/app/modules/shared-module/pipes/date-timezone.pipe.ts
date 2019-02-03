import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment-timezone';

@Pipe({name: 'dateTz'})
export class DateTimezonePipe implements PipeTransform {
  transform(date?: Date, format?: string): string {
    if (date == null) return "";
    return format == null ? moment(date).format() : moment(date).format(format);
  }
}