import { Component, Input, ChangeDetectorRef, ChangeDetectionStrategy, OnChanges, OnInit, AfterViewInit, ViewChildren, QueryList, OnDestroy, DoCheck } from '@angular/core';
import { TabTemplateModel, SectionTemplateModel, SectionType, BaseControlTemplateModel } from '../models/template.model';
import { FormModelService } from '../services/form-model.service';
import { ProxyModel, ProxySectionInstance } from '../models/proxy.model'
import { FormModel, TabModel, SectionModel, SectionInstanceModel, FieldModelBase, FormControlType } from '../models/form.model';
import { ComponentCommService } from '../services/component-comm.service';

import { ExtendedCaseReviewComponentEx } from '../components/controls/ec-review-ex.component';
import { ExtendedCaseReviewSectionInstanceComponent } from './ec-review-section-instance.component';
import { SubscriptionManager } from '../shared/subscription-manager';
import { DigestResult } from '../models/digest.model';
import { Subscription } from 'rxjs/Rx';
import { LogService } from '../services/log.service';

@Component({
    selector: 'ec-review-section',
    templateUrl: './ec-review-section.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class ExtendedCaseReviewSectionComponent implements OnChanges, OnInit, AfterViewInit, OnDestroy {
    @Input() sectionTemplate: SectionTemplateModel;
    @Input() sectionModel: SectionModel;
    @Input() formModel: FormModel;

    isHidden: boolean;

    @ViewChildren(ExtendedCaseReviewSectionInstanceComponent)
    reviewSectionChildComponents: QueryList<ExtendedCaseReviewSectionInstanceComponent>;
    
    private reviewSectionModel: SectionModel;
    private subscriptionManager: SubscriptionManager;

    constructor(private changeDetector: ChangeDetectorRef,
        private componentCommService: ComponentCommService,
        private formModelService: FormModelService,
        
        private logService: LogService) {

        // this was done for a reason since multiple instances of ExtendedCaseReviewSectionComponent may unsubscribe 
        // previous subscription if shared (injected) between instances of the same class
        this.subscriptionManager = new SubscriptionManager(); 
    }

    get reviewSectionInstances(): SectionInstanceModel[] {
        return this.reviewSectionModel.instances;
    }

    get proxyModel():ProxyModel {
        return this.formModel.proxyModel;
    }

    ngOnChanges() {
        //this.logService.debug('ReviewSection: OnChanges called.');
        let sectionPath = this.sectionTemplate.reviewSectionId;
        this.reviewSectionModel = this.findSourceSectionModel(sectionPath);
    }
    
    ngOnInit() {

        //this.logService.debug('ReviewSection: OnInit called.');
        let self = this;

        this.subscriptionManager.addSingle('onDigestComplete',
            this.componentCommService.digestCompletedSubject$
                .subscribe((result: DigestResult) => self.updateChildComponents(result)));
    }

    ngAfterViewInit(): void {
        this.updateChildComponents(null);

    }

    ngOnDestroy(): void {
        this.subscriptionManager.removeAll();
    }
    
    private updateChildComponents(digestResult: DigestResult) {
        if (this.reviewSectionChildComponents && this.reviewSectionChildComponents.length) {
            //this.logService.debug(`@@@ section.updateChildReviewControls: updating review section (${this.reviewSectionModel.id}) instances. Length: ${this.reviewSectionChildComponents.length}`);
            this.reviewSectionChildComponents.forEach(
                (cmp: ExtendedCaseReviewSectionInstanceComponent) =>
                    cmp.updateChildReviewControls(digestResult));
        }

        //trigger update of the component and its childs
        this.changeDetector.detectChanges();
    }

    private findSourceSectionModel(sourceSectionPath : string): SectionModel {

        let sectionModel: SectionModel = null;

        //find: "tabs.tab1.sections.sec1",
        let regex = /tabs.(\w+).sections.(\w+)/gi;
        let match = regex.exec(sourceSectionPath);

        if (match) {
            let tabId = match[1],
                sectionId = match[2];

            sectionModel = this.formModel.tabs[tabId].sections[sectionId];
        }

        return sectionModel;
    }
}
