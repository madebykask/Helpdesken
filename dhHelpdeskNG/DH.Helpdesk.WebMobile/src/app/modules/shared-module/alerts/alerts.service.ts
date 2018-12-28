import { Injectable} from '@angular/core';
import { AlertType } from './alert-types';
import { CommunicationService } from 'src/app/services/communication';
import { mobiscroll } from '@mobiscroll/angular';

@Injectable({providedIn: 'root'})
export class AlertsService {

    constructor(private comService: CommunicationService) {
    }

    clearMessages() {
      mobiscroll.snackbar({duration: 100});
    }

    showMessage(message, alertType: AlertType = AlertType.Info, visibilityTimeoutSec: number = 0) {

        let alertColor = '';
        switch (alertType) {
            case AlertType.Success:
                alertColor = 'success';
                break;
            case AlertType.Warning:
                alertColor = 'warning';
                break;
            case AlertType.Info:
                alertColor = 'info';
                break;
            case AlertType.Error:
                alertColor = 'danger';
                break;
            default:
                throw 'AlertType value is not supported';
        }

        mobiscroll.snackbar({
            color: alertColor,
            duration: visibilityTimeoutSec === 0 ? false: visibilityTimeoutSec * 1000, // the value is Boolean|Number, false for persistent alert
            message: message,
            button: {
                icon:'fa-close',
                action : function() {
                }
            }
        });
    }
}