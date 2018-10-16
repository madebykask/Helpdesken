import { ErrorHandler, Injectable } from '@angular/core';
import { config } from '../../../environments/environment';


@Injectable({ providedIn: 'root' })
export class LoggerService {

  constructor(private errorHandler: ErrorHandler) {}

  log(value: any, ...rest: any[]) {
    if (config.enableLog) {
      console.log(value, ...rest);
    }
  }

  error(error: Error) {
    this.errorHandler.handleError(error);
  }

  warn(value: any, ...rest: any[]) {
    console.warn(value, ...rest);
  }
}