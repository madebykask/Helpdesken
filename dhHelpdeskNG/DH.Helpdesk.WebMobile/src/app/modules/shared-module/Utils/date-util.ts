
export class DateUtil {
  
  static isIsoDateString(value: string){
    //ex:2018-10-29T17:39:13.013Z  
    const res = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)Z$/ig.test(value);
    return res;
  }

  static tryConvertToDate(value: any) {
    if (!value) return value;
    let str = value || '';
    if (this.isIsoDateString(str.toString())) {
      return new Date(str.toString());
    }

    return value;
  }

}