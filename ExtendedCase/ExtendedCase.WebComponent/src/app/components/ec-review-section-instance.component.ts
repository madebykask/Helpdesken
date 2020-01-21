import { Component, ChangeDetectorRef, Output, Input, OnChanges, OnInit, AfterViewInit, ViewChildren, QueryList, DoCheck } from '@angular/core'
import { ProxyModel } from '../models/proxy.model'

import { FormModelService } from '../services/form-model.service';
import { ExtendedCaseReviewComponentEx } from '../components/controls/ec-review-ex.component';
import { FormModel, SectionModel, SectionInstanceModel, FieldModelBase } from '../models/form.model';
import { SectionTemplateModel, BaseControlTemplateModel } from '../models/template.model';

import { DigestResult } from '../models/digest.model';
import { LogService } from '../services/log.service';

@Component({
    selector: 'ec-review-section-instance',
    templateUrl: './ec-review-section-instance.component.html'
})
export class ExtendedCaseReviewSectionInstanceComponent implements AfterViewInit {

    @Input() reviewSectionInstance: SectionInstanceModel;
    @Input() sectionTemplate: SectionTemplateModel;
    @Input() sectionIndex: number;
    @Input() proxyModel: ProxyModel;

    isHidden: boolean;

    @ViewChildren(ExtendedCaseReviewComponentEx)
    reviewComponents: QueryList<ExtendedCaseReviewComponentEx>;

    get reviewSectionTemplate() : SectionTemplateModel {
        return this.reviewSectionInstance.section.template;
    }

    constructor(private formModelService: FormModelService,
        private changeDetector: ChangeDetectorRef,
        private logService: LogService) {
    }

    ngAfterViewInit(): void {

        this.updateChildReviewControls(null);

        //required to update section visibility 
        this.changeDetector.detectChanges();
        
        //subscribe to review section child controls' QueryList collection change to update controls values when section is made visible
        this.reviewComponents.changes.subscribe((o: any) => {
            if (o && o.length) {
                //console.log('@@@ review section child items changed');
                this.updateChildReviewControls(null);
                this.changeDetector.detectChanges();
            }
        });
    }

    trackReviewItem(index: number, value: any): string {
        return `${index}_${value.reviewFieldModel.id}`;
    }

    getSectionTitle() {
        let sectionTitle = this.sectionTemplate.name;
        if (!sectionTitle) {
            sectionTitle = this.reviewSectionInstance.section.template.name;
        }
        return this.sectionIndex > 0 ? `${sectionTitle} ${(this.sectionIndex + 1).toString()}` : sectionTitle;
    }

    updateChildReviewControls(digestResult: DigestResult) {
        //console.log(`@@@ instance.updateChildReviewControls called. reviewSectionId: ${this.reviewSectionTemplate.id}, ChildCmp_Length: ${this.reviewComponents.length}`);

        //handle deleted section instance
        if (!this.reviewSectionInstance.section)
            return;

        if (this.reviewComponents && this.reviewComponents.length) {
            //this.logService.debugFormatted('reviewSectionInstance.updateChildReviewControls(): updating child review controls. ReviewSectionId: {0}, Childs: {1}', this.reviewSectionTemplate.id, this.reviewComponents.length);
            this.reviewComponents.forEach((cmp: ExtendedCaseReviewComponentEx) => cmp.updateValues(digestResult));
        }

        this.setVisibility();
    }

    getReviewSectionFields(reviewSection: SectionInstanceModel): { reviewFieldModel: FieldModelBase, controlTemplate: BaseControlTemplateModel }[] {
        //this.logService.debugFormatted('@@@ instance.getReviewSectionFields called for ReviewSectionId: {0}', this.reviewSectionTemplate.id);
        let items: { reviewFieldModel: FieldModelBase, controlTemplate: BaseControlTemplateModel }[] = [];

        if (this.sectionTemplate.reviewControls) {
            let fieldIds = this.sectionTemplate.reviewControls === '*' ? null : this.sectionTemplate.reviewControls.split(',').map(o => o.trim());
            let reviewFields = this.formModelService.getSectionFields(reviewSection, fieldIds, fieldIds != null);

            for (let reviewField of reviewFields) {
                let ctrlTpl = this.findControlTemplateById(reviewField.id);
               
                items.push({
                    reviewFieldModel: reviewField,
                    controlTemplate: ctrlTpl
                });
            }
        } else {

            for (let controlTpl of this.sectionTemplate.controls) {
                let fieldModel = reviewSection.fields[controlTpl.reviewControlId];
                if (fieldModel && controlTpl)
                    items.push({
                        reviewFieldModel: fieldModel,
                        controlTemplate: controlTpl
                    });
            }
        }

        this.logService.debugFormatted('getReviewSectionFields(): added review controls: {0}, ReviewSectionId: {1}', items.length, this.reviewSectionTemplate.id);
        return items;
    }

    private findControlTemplateById(fieldId: string): BaseControlTemplateModel {
        if (this.sectionTemplate.controls) {
            for (let ctrlKey of Object.keys(this.sectionTemplate.controls)) {
                let ctrl = this.sectionTemplate.controls[ctrlKey];
                if (ctrl.reviewControlId && ctrl.reviewControlId === fieldId) {
                    return ctrl;
                }
            }
        }
        return null;
    }

    private setVisibility() {
        let prevHidden = this.isHidden;

        if (this.reviewSectionInstance.hidden || this.reviewSectionInstance.sectionEnableStateSelection === false) {
            this.isHidden = true;
        } else if (this.reviewComponents && this.reviewComponents.length) {
            let allHidden = true;
            this.reviewComponents.forEach((cmp: ExtendedCaseReviewComponentEx) => allHidden = allHidden && cmp.isHidden);
            this.isHidden = allHidden;
        } else {
            this.isHidden = false;
        }

        if (prevHidden !== this.isHidden) {
            this.logService.debugFormatted('reviewSectionInstance: visibility changed. OldValue: {0}, NewValue: {1}', prevHidden, this.isHidden);
        }
    }
}
