import { DateTime } from 'luxon';

export class DateUtil {

  static formatToLocalDate(date: Date): string {
    return DateTime.fromJSDate(date).toLocaleString(DateTime.DATE_SHORT);
  }

  static formatDate(date: Date | string, format: Intl.DateTimeFormatOptions = null) {
    if (DateUtil.isDate(date)) {
      return format == null ?
       DateTime.fromJSDate(date as Date).toLocaleString(DateTime.DATETIME_SHORT) : DateTime.fromJSDate(date as Date).toLocaleString(format);
    }
    return format == null ?
     DateTime.fromISO(date as string).toLocaleString(DateTime.DATETIME_SHORT) : DateTime.fromISO(date as string).toLocaleString(format);
  }

  static isDate(val: any) {
     return val && Object.prototype.toString.call(val) === '[object Date]' && !isNaN(val);
  }

  static tryConvertToDate(value: any, isUtc: boolean = false) {
    if (!value) { return value; }

    let dateValue = isUtc ? DateTime.fromISO(value, { zone: 'utc' }) : DateTime.fromISO(value);
    if (dateValue.isValid) {
      return dateValue.toJSDate();
    }

    dateValue = isUtc ? DateTime.fromSQL(value, { zone: 'utc' }) : DateTime.fromSQL(value);
    if (dateValue.isValid) {
      return dateValue.toJSDate();
    }

    return value;
  }

}
