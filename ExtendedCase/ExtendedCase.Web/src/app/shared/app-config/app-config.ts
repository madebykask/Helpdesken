import { InjectionToken } from '@angular/core';
import { ValidatorError, MinMax, ValidateOn } from '../../shared/validation-types';
import { LogLevel } from '../../shared/common-types';

export interface ILogSettings {
    enabled: boolean;
    logLevel: LogLevel;
}

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

const _settings = AppSettings;

const _degugLogSettings = { enabled: true, logLevel: LogLevel.Debug };
const _prodLogSettings = { enabled: true, logLevel: LogLevel.Error };//change once dev is complete

export const AppDiConfig: IAppConfig = {
    debugMode: _settings.debugMode,
    dbDateFormat: 'YYYY-MM-DD',
    dateFormat: 'DD/MM/YYYY',
    yearFormat: 'YYYY',
    dbDecimalSeparator: '.',
    decimalSeparator: '.',
    apiHost: _settings.apiHost,
    showDebugProxyModel: _settings.showDebugProxyModel,
    validationMode: ValidateOn.OnSave,
    isManualValidation: false,
    logSettings: _settings.debugMode ? _degugLogSettings : _prodLogSettings
};

export let AppConfig = new InjectionToken<IAppConfig>('app.config');



