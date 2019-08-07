import { Component, Input } from '@angular/core';
import { ComponentCommService } from '../services/component-comm.service';
import { FormModel, TabModel } from '../models/form.model';
import { TabTemplateModel, SectionTemplateModel, SectionType } from '../models/template.model';
import { Subscription } from 'rxjs';

@Component({
    selector: 'ec-tab',
    templateUrl: './ec-tab.component.html',
    //changeDetection: ChangeDetectionStrategy.OnPush
})
export class ExtendedCaseTabComponent {
    @Input() tabTemplate: TabTemplateModel;
    @Input() tabModel: TabModel;
    @Input() formModel: FormModel;

    private subscription: Subscription;
    private validationEventSubscription: Subscription;
    private colSequence: number[];

    constructor(private componentCommService: ComponentCommService) {
        //private changeDetector: ChangeDetectorRef) {
        //changeDetector.detach();
    }

    isReviewSection(section: SectionTemplateModel) {
        return section.type === SectionType.review;
    }

    getColumnSequence(): number[] {
        let seq: number[] = [];
        for (let i = 0; i < this.tabTemplate.columnCount; i++) {
            seq.push(i);
        }
        return seq;
    }

    getColumnCssClass(): string {
        let css = '';
        switch (this.tabTemplate.columnCount) {
            case 1:
                css = 'col-md-12';
                break;
            case 2:
                css = 'col-md-6';
                break;
            case 3:
                css = 'col-md-4';
                break;
            case 4:
                css = 'col-md-3';
                break;
        }
        return css;
    }

    trackColumnByFn(index: any, item: number) {
        return item;
    }

    trackById(index: any, item: SectionTemplateModel) {
        return item.id;
    }
}
