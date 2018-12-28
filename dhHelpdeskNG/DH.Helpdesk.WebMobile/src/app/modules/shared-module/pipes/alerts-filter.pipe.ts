
import { Pipe, PipeTransform } from '@angular/core';
import { AlertType, Alert } from 'src/app/modules/shared-module/alerts/alert-types';

@Pipe({ name: 'alertsFilter', pure: false })
export class AlertsFilterPipe implements PipeTransform {
    transform(alerts: Alert[], type: string): Alert[] {
        let alertType = <AlertType>AlertType[type];
        let filteredItems = alerts.filter(item => item.type === alertType);
        return filteredItems;
    }
}