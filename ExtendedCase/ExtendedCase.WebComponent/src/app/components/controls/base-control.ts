
import { merge, Subscription } from 'rxjs';
import { Input, ChangeDetectorRef } from '@angular/core';
import { BaseControlTemplateModel } from '../../models/template.model';
import { FieldModelBase } from '../../models/form.model';
import { ComponentCommService } from '../../services/component-comm.service';

export class BaseControl {
    @Input() fieldTemplate: BaseControlTemplateModel;
    @Input() fieldModel: FieldModelBase;
    protected isRequiredLabel = '';
    private subscription: Subscription;

    constructor(protected componentCommService: ComponentCommService, protected changeDetector: ChangeDetectorRef = null) {
        const obs$ = merge(this.componentCommService.validationModeChange$, this.componentCommService.digestCompletedSubject$);

        this.subscription = obs$.subscribe(() => {
            this.isRequiredLabel = this.getLabel();
            if (changeDetector) { changeDetector.markForCheck(); }
        });
    }

    protected getLabel(): string {
        const isRequired = this.fieldModel.isRequired;
        // return (typeof isRequired === 'number' && isRequired <= mode) ? '*' : '';
        return (typeof isRequired === 'number') ? '*' : '';
    }

    protected ngOnDestroy() {
        if (this.subscription != null) { this.subscription.unsubscribe(); }
    }
}
