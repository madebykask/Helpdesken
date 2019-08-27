import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { ClientLogEntryModel, ClientLogLevel } from '../../models/shared/client-log.model';
import { LoggerService } from './logger.service';
import { ClientLogApiService } from '../api/logging/client-log-api.service';
import { AlertsService } from 'src/app/services/alerts/alerts.service';
import { AlertType } from 'src/app/modules/shared-module/alerts/alert-types';
import { WindowWrapper } from 'src/app/modules/shared-module/helpers/window-wrapper';
import { UuidGenerator } from 'src/app/modules/shared-module/utils/uuid-generator';
import { take, catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

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
        let log = errorMsg || 'Unknown Error. ';

        if (err instanceof HttpErrorResponse) {
            // Server Error
            if (err.message) {
              log += `Message: ${err.message}. `;
            }
            if (err.status) {
              log = log + `Status: ${err.status}.`;
            }
        } else if (err instanceof Error) {
            // Client Error
            const msg = err.message ? err.message : err.toString();
            log = log + `Message: ${msg}. `;
            if (err.stack) {
              log = log +  `Stack: ${err.stack}`;
            }
        }

        // send error to server
        const errorGuid = UuidGenerator.createUuid();

        // log error to console
        this.logService.error(`Error ${errorGuid}: ${log}`);

        // save error to log on server
        this.saveErrorOnServer(errorGuid, err, log);

        //todo: improve error handling logic below
        if (err instanceof HttpErrorResponse) {
            // Server or connection error happened
            if (!navigator.onLine) {
              // Handle offline error
              this.alertsService.showMessage('No Internet Connection', AlertType.Warning);
              return;
            } else {
              // Handle Http Errors (error.status === 403, 404...)
              //let alertMsg = this.buildErrorAlertMessage(errorGuid);
              //this.alertsService.showMessage(alertMsg, AlertType.Error);
            }
         } else {
             // Handle Client Error (Angular Error, ReferenceError...)
             // this.router.navigate(['/error'], {  queryParams: { errorGuid: errorGuid });
         }

         this.router.navigate(['/error'], {  queryParams: { errorGuid: errorGuid } });
    }

    private saveErrorOnServer(errorGuid: string, error: any, errorMsg: string) {
        const logEntry = new ClientLogEntryModel();
        logEntry.uniqueId = errorGuid;
        logEntry.level = ClientLogLevel.Error;
        logEntry.url = this.window.nativeWindow.location.href;
        logEntry.message = errorMsg || '';

        if (error && error instanceof Error && error.stack) {
            logEntry.stack = error.stack.toString();
        }

        this.clientLogApiService.saveLogEntry(logEntry).pipe(
          take(1),
          catchError((err: any) => throwError(err))
        ).subscribe(
          () => { },
          (e: any) => {
            this.logService.error(`Failed to send error (${logEntry.uniqueId}) to server. Error: ${e.toString()}`);
            this.alertsService.showMessage(`Failed to send the error (${logEntry.uniqueId}) to server. Error: ${errorMsg}.`, AlertType.Error);
          });
    }

    // handles user error
    handleUserError(userMsg: string) {

        const errorMsg = userMsg || 'Unknown Error. ';

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
}
