import { Component, OnInit, OnDestroy, ChangeDetectorRef, ChangeDetectionStrategy } from '@angular/core';
import { AlertsService } from '../../services/alerts.service';
import { Alert, AlertType } from '../../shared/alert-types';
import { SubscriptionManager } from '../../shared/subscription-manager';

@Component({
    selector: 'alert',
    templateUrl: './alert.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class AlertComponent implements OnInit, OnDestroy {
    alerts: Alert[] = [];
    private subscriptionManager = new SubscriptionManager();

    constructor(private alertsService: AlertsService, private changeDetector:ChangeDetectorRef) {
    }

    ngOnInit(): void {
        let self = this;
        this.subscriptionManager.addSingle('alerts',
            this.alertsService.alerts$.subscribe(alert => self.processNewAlert(alert)));
    }

    ngOnDestroy(): void {
        this.subscriptionManager.removeAll();
    }

    private processNewAlert(alert: Alert) {
        if (alert === null) {
            this.alerts = [];
        } else {
            //show only 5 alerts at time - clean previous 
            while (this.alerts.length > 4) {
                this.alerts.shift();
            }
            this.alerts.push(alert);
        }

        // raise change detection event for the component only to update the ui
        this.changeDetector.detectChanges();
    }

    get alertTypes(): string[] {
        //get enum names
        return Object.keys(AlertType).filter(a => a.match(/^\D/));
    }

    getCssClass(type: string) {
        if (!alert) return '';

        let alertType = <AlertType>AlertType[type];

        switch (alertType) {
        case AlertType.Success:
            return 'alert alert-success';
        case AlertType.Info:
            return 'alert alert-info';
        case AlertType.Warning:
            return 'alert alert-warning';
        case AlertType.Error:
            return 'alert alert-danger';
        default:
            return '';
        }
    }

    removeAlert(alert: Alert) {
        this.alerts = this.alerts.filter(item => item !== alert);
    }

    removeAlerts(type: string) {
        let alertType = <AlertType>AlertType[type];
        this.alerts = this.alerts.filter(item => item.type !== alertType);
    }
}

    

