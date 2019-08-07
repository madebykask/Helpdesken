import { InjectionToken } from '@angular/core';
import { ValidateOn } from '../../shared/validation-types';
import { LogLevel } from '../../shared/common-types';
import { IAppConfig } from './app-config.interface';
declare var AppSettings: any; // to avoid compiler error. Using global variable from js.

const _settings = AppSettings;

const _degugLogSettings = { enabled: true, logLevel: LogLevel.Debug };
const _prodLogSettings = { enabled: true, logLevel: LogLevel.Error }; // change once dev is complete

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



