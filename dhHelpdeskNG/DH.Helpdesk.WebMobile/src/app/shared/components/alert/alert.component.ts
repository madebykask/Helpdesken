/* import { Component, OnInit, ChangeDetectorRef, ViewChild } from '@angular/core';
import { AlertType, Alert } from 'src/app/helpers/alerts/alert-types';
import { AlertsService } from 'src/app/helpers/alerts/alerts.service';
import { ChangeDetectionStrategy } from '@angular/core';
import { Subject } from 'rxjs';
import { MbscPopupOptions } from '@mobiscroll/angular';
import { takeUntil } from 'rxjs/operators';
import { mobiscroll } from '@mobiscroll/angular';

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AlertComponent implements OnInit {

  private destroy$ = new Subject();
  alerts: Alert[] = [];   
    
  @ViewChild('alertpopup')
  popup: any;

  popupSettings: MbscPopupOptions  = {
    display: 'bottom',
    closeOnOverlayTap	: true,
    animate: 'slidedown',    
    cssClass: 'mbsc-no-padding alertspopup',
    };
  
    constructor(private alertsService: AlertsService, 
                private changeDetector:ChangeDetectorRef) {
    }

    ngOnInit(): void {
        let self = this;
        this.alertsService.alerts$.pipe(
                takeUntil(self.destroy$)
            ).subscribe(alert => self.processNewAlert2(alert));     
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    private processNewAlert2(alert: Alert) {            
        //todo: support other alert types!
        mobiscroll.snackbar({
            color: 'danger',
            duration: 5000,
            message: alert.message,
            button: {
                icon:'fa-close',
                action : function(){                    
                }
            }               
        });
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
        
        this.popup.instance.show();        

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

        if (this.alerts.length === 0) {
            this.popup.instance.hide();
        }
    }
}
 */