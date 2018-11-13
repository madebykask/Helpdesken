import { Injectable } from '@angular/core';
import { config } from '@env/environment';
import { InfoLoggerService } from './info-logger.service';

@Injectable({ providedIn: 'root' })
export class LoggerService {

  constructor() {}

  log(value: any, ...rest: any[]) {
    if (config.enableLog) {
      console.log(value, ...rest);
    }
  }

  info(value: string) {
    if (config.enableLog) {
      console.log(value);
    }
  }
  
  warn(value: any, ...rest: any[]) {
    console.warn(value, ...rest);
  }

  error(value: any, ...rest: any[]) {    
    console.error(value, ...rest);
  }
}