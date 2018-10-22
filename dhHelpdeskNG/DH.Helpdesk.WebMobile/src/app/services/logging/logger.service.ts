import { ErrorHandler, Injectable } from '@angular/core';
import { config } from '@env/environment';

@Injectable({ providedIn: 'root' })
export class LoggerService {

  constructor() {}

  log(value: any, ...rest: any[]) {
    if (config.enableLog) {
      console.log(value, ...rest);
    }
  }
  
  warn(value: any, ...rest: any[]) {
    console.warn(value, ...rest);
  }

  error(value: any, ...rest: any[]) {    
    console.error(value, ...rest);
  }
}