import { Injectable} from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { AlertType, Alert } from './alert-types';

@Injectable({providedIn: 'root'})
export class AlertsService {

    private alertsSubject: Subject<Alert> = new Subject<Alert>();
    alerts$: Observable<Alert> = this.alertsSubject.asObservable();

    success(msg:string) {
        this.raiseAlert(msg, AlertType.Success);
    }

    info(msg: string) {
        this.raiseAlert(msg, AlertType.Info);
    }

    warning(msg: string) {
        this.raiseAlert(msg, AlertType.Warning);
    }

    error(msg: string) {
        this.raiseAlert(msg, AlertType.Error);
    }

    clear() {
        this.alertsSubject.next(null);
    }

    private raiseAlert(msg:string, alertType: AlertType) {
        this.alertsSubject.next(new Alert(alertType, msg));
    }
}