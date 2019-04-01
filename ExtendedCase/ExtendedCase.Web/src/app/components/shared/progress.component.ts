import { Component, OnInit, OnDestroy, ChangeDetectorRef, ChangeDetectionStrategy, ViewEncapsulation } from '@angular/core';
import { AlertsService } from '../../services/alerts.service';
import { Alert, AlertType } from '../../shared/alert-types';
import { SubscriptionManager } from '../../shared/subscription-manager';

@Component({
    selector: 'progress-cmp',
    templateUrl: './progress.component.html'
})
export class ProgressComponent {
    progressText:string;
    isVisible:boolean;

    show(status: string = null) {
        this.progressText = status || '';
        this.isVisible = true;
    }

    hide() {
        this.isVisible = false;
    }
}

    

