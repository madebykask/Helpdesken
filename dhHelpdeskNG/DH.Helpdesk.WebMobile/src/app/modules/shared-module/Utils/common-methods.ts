export function isUndefinedOrNull(value: any) {
  return value === null || value === undefined;
}

export function isUndefinedNullOrEmpty(value: any) {
  return value === '' || value === undefined || value === null;
}

export function areUndefinedNullOrEmpty(val1: any, val2: any) {
  return isUndefinedNullOrEmpty(val1) && isUndefinedNullOrEmpty(val2);
}

export function isArray(obj: any): boolean {
  return (obj instanceof Array) || Array.isArray(obj);
}

export function areArraysEqual(array1: any[], array2: any[]): boolean {
  if (!array1 || !array2) { return false; }

  const a = array1.slice().sort();
  const b = array2.slice().sort();

  return a.length === b.length && a.every((el, ix) => el === b[ix]);
}

export function convertNameValueArrayToObject(itemsArray: {name: string, value: string}[]): {[key: string]: string} {
  if (itemsArray && itemsArray.length) {
    return itemsArray.reduce((obj, f) => {
      obj[f.name] = f.value;
      return obj;
    }, {});
  }
  return null;
}

export function areValuesEqual(value1: any, value2: any): boolean {
  if (value1 instanceof Array && value2 instanceof Array) {
    return areArraysEqual(value1 as any[], value2 as any[]);
  }

  return value1 === value2;
}

export function convertAnyToString(obj: any): string {
  if (isUndefinedOrNull(obj)) {
    return '';
  }

  if (isArray(obj) && obj) {
    return obj.join();
  }

  return obj.toString();
}

export function clone(obj: any, copyNotOwnProperties?: boolean): any {
  if (isArray(obj)) { return obj.slice(); }

  const newObj = {};
  for (const prop in obj) {
    if (copyNotOwnProperties || obj.hasOwnProperty(prop)) {
      newObj[prop] = obj[prop];
    }
  }
  return newObj;
}

export function hasKey<T>(items: T, key: string): boolean {
  if (!items || items.constructor !== Array && items.constructor !== Object) {
    return false;
  }
  return items.hasOwnProperty(key);
}

export function parseIntOrDefault(val: any, defaultVal: number = 0) {
  if (isUndefinedNullOrEmpty(val)) {
    return defaultVal;
  }

  const res = parseInt(val.toString(), 10);
  if (isNaN(res) || res === undefined) {
    return defaultVal;
  }

  return res;
}

export function toNumber(strNumber: string): number {
  if (!strNumber.length) {
    throw new Error(`toNumber: value is empty`);
  }
  return parseFloat(strNumber.replace(',', '.').replace(' ', ''));
}

export function anyToBoolean(val: any): boolean {
  if (isUndefinedNullOrEmpty(val)) {
    return false;
  }

  switch (val.toString().toLowerCase().trim()) {
    case 'true':
    case 'yes':
    case '1':
      return true;

    case 'false':
    case 'no':
    case '0':
    case null:
      return false;

    default:
      return Boolean(val);
  }
}

export function removeAt(str: string, startIndex: number, count: number): string {
  return str.substr(0, startIndex) + str.substr(startIndex + count);
}

export function removeMultipleSpaces(val: string, trimTrailing: boolean = true): string {
  if (typeof val !== 'string') { return val; }

  const regex = /\s{2,}/igm; //with multiline (m)

  while (regex.test(val)) {
    val = val.replace(regex, ' ');
  }

  if (trimTrailing) {
    val = val.trim();
  }

  return val;
}

export function isValidEmail(s: string) {
  if (isUndefinedNullOrEmpty(s)) { return false; }

  const res = /^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$/ig.test(s);
  return res;
}
