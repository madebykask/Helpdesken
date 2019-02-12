import * as moment from 'moment'
import { MomentBuiltinFormat } from 'moment';

export class DateUtil {
  
  static formatToLocalDate(date:Date):string {
    return moment(date).format("L");
  }

  static formatDate(date:Date, format:string = null){
    return format == null ? moment(date).format() : moment(date).format(format);
  }

  static isDate(val:any) {
     return val && Object.prototype.toString.call(val) === "[object Date]" && !isNaN(val);
  }

  static isDateString(value: string, format:string | MomentBuiltinFormat = null):boolean {
    if (!value) return false;

    //set iso date by default
    if (format == null) format = moment.ISO_8601;

    let dateValue = moment(value, format);
    let isValid = dateValue.isValid();
    return isValid;
  } 

  static convertToDate(value: any, format:string | MomentBuiltinFormat = null) {
    //set iso date by default
    if (format == null) format = moment.ISO_8601;

    let dateValue = moment(value, format);
    return dateValue.isValid() ? dateValue.toDate() : null;
  }

  static tryConvertToDate(value: any, format:string | MomentBuiltinFormat = null) {
    if (!value) return value;
    
    //set iso date by default
    if (format == null) format = moment.ISO_8601;

    let dateValue = moment(value, format);
    if (dateValue.isValid()) {
      return dateValue.toDate();
    }
    
    //keep original value if its not date
    return value;
  }

}