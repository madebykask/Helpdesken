import { Injectable, Inject, forwardRef } from '@angular/core';
import { IAppConfig, AppConfig, ILogSettings } from '../shared/app-config/app-config';
import { LogLevel } from '../shared/common-types';
import * as cm from '../utils/common-methods';

@Injectable()
export class LogService {
    private settings: ILogSettings;
    
    constructor(@Inject(AppConfig) private config: IAppConfig,) {
        this.settings = this.config.logSettings
            ? this.config.logSettings
            : <ILogSettings>{ enabled: true, logLevel: LogLevel.Error };
    }
    
    debugFormatted(template: string, ...args: any[]) {
        if (this.isDebug) {
            let s = this.buildFormattedString(template, args);
            this.debug(s);
        }
    }

    debug(msg: string) {
        if (this.isDebug) {
            console.log('[debug]:' + msg);
        }
    }

    info(msg: string) {
        if (this.isInfo) {
            console.info('[info]:' + msg);
        }
    }

    infoFormatted(template: string, ...args: any[]) {
        if (this.isInfo) {
            let s = this.buildFormattedString(template, args);
            this.info(s);
        }
    }

    warning(msg: string) {
        if (this.isWarning) {
            console.warn('[warn]:' + msg);    
        }
    }

    warningFormatted(msg: string, ...args: any[]) {
        if (this.isWarning) {
            let s = this.buildFormattedString(msg, args);
            this.warning(s);
        }
    }

    error(msg: string) {
        if (this.isError) {
            console.error('[error]:' + msg);
        }
    }

    private buildFormattedString(template:string, args:any[]) : string {
        let formattedArgs = this.formatArgs(args);
        let s = cm.formatString(template, formattedArgs);
        return s;
    }

    private formatArgs(args: string[]) {
        if (cm.isUndefinedOrNull(args)) return args;

        let formattedArgs = args.map((item: any) => cm.formatObject(item));
        return formattedArgs;
    }

    private get isDebug(): boolean {
        return this.settings.enabled && this.settings.logLevel === LogLevel.Debug;
    }

    private get isInfo(): boolean {
        return this.settings.enabled &&
               this.settings.logLevel <= LogLevel.Info;
    }

    private get isWarning(): boolean {
        return this.settings.enabled &&
            this.settings.logLevel <= LogLevel.Warning;
    }

    private get isError(): boolean {
        return this.settings.enabled &&
            this.settings.logLevel <= LogLevel.Error;
    }

    /*
    createLogger(name: string) {
        return new Logger(name);
    }*/
}

export interface ILogger {
    logDebug(msg: string): void;
    logInfo(msg: string): void;
    logWarning(msg: string): void;
    logError(msg: string): void;
}

export class Logger {
    constructor(private name: string) {
    }

    private logDebug(msg: string) {
        let s = this.prepareLogMessage(msg);
        console.log('[debug]:' + s);
    }

    private logInfo(msg: string) {
        let s = this.prepareLogMessage(msg);
        console.info('[info]:' + s);
    }

    private logWarning(msg: string) {
        let s = this.prepareLogMessage(msg);
        console.warn('[warn]:' + s);
    }

    private logError(msg: string) {
        let s = this.prepareLogMessage(msg);
        console.error('[error]:' + s);
    }

    private prepareLogMessage(msg: string) {
        return `[${name}]: ${msg}`;
    }
}

