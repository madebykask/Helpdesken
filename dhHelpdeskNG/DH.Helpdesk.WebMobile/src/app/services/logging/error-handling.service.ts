import { Injectable } from '@angular/core';
import { UuidGenerator, WindowWrapper } from '../../helpers';
import { ClientLogEntryModel, ClientLogLevel } from '../../models/shared/client-log.model';
import { LoggerService } from './logger.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { ClientLogApiService } from '../api';
import { AlertsService } from 'src/app/services/alerts/alerts.service';
import { AlertType } from 'src/app/modules/shared-module/alerts/alert-types';

@Injectable({providedIn: 'root'})
export class ErrorHandlingService {
    
    constructor(
        private clientLogApiService: ClientLogApiService,
        private logService: LoggerService,
        private alertsService: AlertsService,
        private window: WindowWrapper,
        private router: Router) {
    }

    // handles unknown error (system)
    handleError(err: any, errorMsg: string = '') {
        
        // prepare log text
        let log = errorMsg || 'Unknown Error.';
        
        if (err instanceof Error && err.message) {
            log +=  ` ${err.message || ''}`;
            if (err.stack) {
              log += ` Stack: ${err.stack || ''}`;
            }
        }
        else {
            log += ' ' + (err || '').toString();
        }

        // send error to server
        let errorGuid = UuidGenerator.createUuid();

        // log error to console
        this.logService.error(`Error ${errorGuid}: ${log}`);

        // save error to log on server
        this.saveErrorOnServer(errorGuid, err, log);

        //todo: improve error handling logic below
        if (err instanceof HttpErrorResponse) {
            // Server or connection error happened
            if (!navigator.onLine) {
              // Handle offline error
              // return this.alertsService.warning('No Internet Connection');
            } else {
              // Handle Http Errors (error.status === 403, 404...)
              // let alertMsg = this.buildErrorAlertMessage(errorGuid);
              // this.alertsService.error(alertMsg); 
            }
         } else {
             // Handle Client Error (Angular Error, ReferenceError...)
             // this.router.navigate(['/error'], {  queryParams: { errorGuid: errorGuid });
         }
         
         this.router.navigate(['/error'], {  queryParams: { errorGuid: errorGuid } });
    }

    private saveErrorOnServer(errorGuid:string, error:any, errorMsg){
        let logEntry = new ClientLogEntryModel();
        logEntry.uniqueId = errorGuid;
        logEntry.level = ClientLogLevel.Error;
        logEntry.url = this.window.nativeWindow.location.href;
        logEntry.message = errorMsg;
        
        if (error && error instanceof Error && error.stack) {
            logEntry.stack = error.stack.toString();
        }

        this.clientLogApiService.saveLogEntry(logEntry)
            .subscribe(
                () => { },
                (e: any) => this.logService.error(`Failed to send error (${logEntry.uniqueId}) to server. error: ${e.toString()}`)
        );
    }

    // handles user error
    handleUserError(userMsg: string) {

        let errorMsg = userMsg || 'Unknown Error. ';

        this.logService.error(errorMsg);

        // raise error alert to display user error message on ui 
        this.alertsService.showMessage(`${errorMsg}`, AlertType.Error);
    }

    // handles warning
    handleWarning(userMsg: string) {

        this.logService.warn(userMsg);

        //raise error alert to display user error message on ui 
        this.alertsService.showMessage(`${userMsg}`, AlertType.Warning);
    }

    private buildErrorAlertMessage(errorId:string) : string {
        let str: string =
            `Sorry, an error occurred while processing your request. Please provide Error Id to the support team.
             ErrorId: ${errorId}`;
        return str;
    }
}