import { ValidateOn } from '../validation-types';
import { LogLevel } from '../common-types';

export interface IAppConfig {
  debugMode: boolean,
  dbDateFormat: string,
  dateFormat: string;
  yearFormat: string;
  dbDecimalSeparator: string,
  decimalSeparator: string;
  apiHost: string;
  showDebugProxyModel: boolean;
  validationMode: ValidateOn;
  isManualValidation: boolean; // if true - validation is started manually from code,  false - angular default validation
  logSettings: ILogSettings;
}

export interface ILogSettings {
  enabled: boolean;
  logLevel: LogLevel;
}
