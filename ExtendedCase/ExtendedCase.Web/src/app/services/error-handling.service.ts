import { Injectable } from '@angular/core';
import 'rxjs/add/operator/catch';
import { LogService } from './log.service';
import { UuidGenerator } from '../utils/uuid-generator';
import { WindowWrapper } from '../shared/window-wrapper';
import { ClientLogEntryModel, ClientLogLevel } from '../models/client-log.model';
import { ClientLogApiService } from './data/client-log-api.service';
import { AlertsService } from './alerts.service';
import { Alert, AlertType } from '../shared/alert-types';

@Injectable()
export class ErrorHandlingService {
    
    constructor(
        private clientLogApiService: ClientLogApiService,
        private logService: LogService,
        private alertsService: AlertsService,
        private window: WindowWrapper) {
    }

    // handles unknown error (system)
    handleError(err: any, errorMsg: string = '') {

        //prepare log text
        let log = errorMsg || 'Unknown Error.';

        if (err) {
            log += ' ' + err.toString();
        }

        this.logService.error(log);

        //send error to server
        let logEntry = new ClientLogEntryModel();
        logEntry.UniqueId = UuidGenerator.createUuid();
        logEntry.Level = ClientLogLevel.Error;
        logEntry.Url = this.window.nativeWindow.location.href;
        logEntry.Message = log;
        if (err && err instanceof Error && err.stack) {
            logEntry.Stack = err.stack.toString();
        }

        this.clientLogApiService.saveLogEntry(logEntry)
            .subscribe(
                () => { },
                (e: any) => this.logService.error(`Failed to send error (${logEntry.UniqueId}) to server. error: ${e.toString()}`)
        );

        // raise error alert to display user error message on ui 
        let alertMsg = this.buildErrorAlertMessage(logEntry.UniqueId);
        this.alertsService.error(alertMsg);
    }

    //handles user error
    handleUserError(userMsg: string) {

        let errorMsg = userMsg || 'Unknown Error. ';

        this.logService.error(errorMsg);

        // raise error alert to display user error message on ui 
        this.alertsService.error(`${errorMsg}`);
    }

    //handles warning
    handleWarning(userMsg: string) {

        this.logService.warning(userMsg);

        // raise error alert to display user error message on ui 
        //this.alertsService.warning(`${userMsg}`);
    }

    private buildErrorAlertMessage(errorId:string) : string {
        let str: string =
            `Sorry, an error occurred while processing your request. Please provide Error Id to the support team.
             ErrorId: ${errorId}`;
        return str;
    }
}