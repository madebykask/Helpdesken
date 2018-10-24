import { Injectable } from '@angular/core';
import { UuidGenerator, WindowWrapper } from '../../helpers';
import { ClientLogEntryModel, ClientLogLevel } from '../../models/shared/client-log.model';
import { ClientLogApiService } from './client-log-api.service';
import { LoggerService } from '../logging';
import { AlertsService } from '../../helpers/alerts/alerts.service';
import 'rxjs/add/operator/catch';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({providedIn: 'root'})
export class ErrorHandlingService {
    
    constructor(
        private clientLogApiService: ClientLogApiService,
        private logService: LoggerService,
        private alertsService: AlertsService,
        private window: WindowWrapper) {
    }

    // handles unknown error (system)
    handleError(err: any, errorMsg: string = '') {
        
        //prepare log text
        let log = errorMsg || 'Unknown Error.';
        
        if (err instanceof Error && err.message) {
            log +=  ' ' + err.message;            
        }
        else {
            log += ' ' + (err || '').toString();
        }
        //send error to server
        let errorGuid = UuidGenerator.createUuid();

        //log error to console
        this.logService.error(`Error ${errorGuid}: ${log}`);

        // raise error alert to display user error message on ui 
        let alertMsg = this.buildErrorAlertMessage(errorGuid);        
        this.alertsService.error(alertMsg); //todo: implement error alert service

        if (err instanceof HttpErrorResponse) {
            // Server or connection error happened
            if (!navigator.onLine) {
              // Handle offline error
              //return notificationService.notify('No Internet Connection');
            } else {
              // Handle Http Error (error.status === 403, 404...)
              //return notificationService.notify(`${error.status} - ${error.message}`);
            }
         } else {
             // Handle Client Error (Angular Error, ReferenceError...)
             //router.navigate(['/error'], { queryParams: {error: error} });           
         }

         this.saveErrorOnServer(errorGuid, err, log);
    }

    private saveErrorOnServer(errorGuid:string, error:any, errorMsg){
        let logEntry = new ClientLogEntryModel();
        logEntry.UniqueId = errorGuid;
        logEntry.Level = ClientLogLevel.Error;
        logEntry.Url = this.window.nativeWindow.location.href;
        logEntry.Message = errorMsg;
        
        if (error && error instanceof Error && error.stack) {
            logEntry.Stack = error.stack.toString();
        }

        this.clientLogApiService.saveLogEntry(logEntry)        
            .subscribe(
                () => { },
                (e: any) => this.logService.error(`Failed to send error (${logEntry.UniqueId}) to server. error: ${e.toString()}`)
        );
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

        this.logService.warn(userMsg);

        //raise error alert to display user error message on ui 
        this.alertsService.warning(`${userMsg}`);
    }

    private buildErrorAlertMessage(errorId:string) : string {
        let str: string =
            `Sorry, an error occurred while processing your request. Please provide Error Id to the support team.
             ErrorId: ${errorId}`;
        return str;
    }
}