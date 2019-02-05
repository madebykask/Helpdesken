import * as moment from 'moment'

export class DateUtil {
  
  static isDateString(value: string):boolean {
    if (!value) return false;
    let dateValue = moment(value);
    let isValid = dateValue.isValid();
    return isValid;
  }

  static tryConvertToDate(value: any) {
    if (!value) return value;

    let dateValue = moment(value);
    if (dateValue.isValid()) {
      return dateValue.toDate();
    }
    
    //keep original value if its not date
    return value;
  }

}