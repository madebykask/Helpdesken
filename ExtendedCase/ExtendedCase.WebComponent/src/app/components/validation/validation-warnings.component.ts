import { Component, Input, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
//import { DigestResult } from '../../models/digest.model';
//import { SubscriptionManager } from '../../shared/subscription-manager';
//import { ComponentCommService } from '../../services/component-comm.service';

@Component({
    selector: 'validation-warnings',
    templateUrl: './validation-warnings.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})

export class ValidationWarningComponent {
    @Input() warnings: Array<string>;

    //constructor(private componentCommService: ComponentCommService, private subscriptionManager: SubscriptionManager,
    //    private changeDetector: ChangeDetectorRef) {
    //}

    //ngOnInit(): void {
    //    this.subscriptionManager.addSingle('onDigestComplete',
    //        this.componentCommService.digestCompletedSubject$
    //        .subscribe((result: DigestResult) => this.changeDetector.markForCheck()));

    //}
}