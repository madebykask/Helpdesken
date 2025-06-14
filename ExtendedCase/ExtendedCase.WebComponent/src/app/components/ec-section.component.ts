﻿import { Component, Input, OnInit, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { ComponentCommService, PopulateSectionParams, EnableSectionParams, AddSectionInstanceParams, DeleteSectionInstanceParams } from '../services/component-comm.service';
import { SectionModel, SectionInstanceModel, FieldModelBase, FormControlType, FormModel } from '../models/form.model';
import { SectionTemplateModel, SectionType, BaseControlTemplateModel } from '../models/template.model';
import { SubscriptionManager } from '../shared/subscription-manager';
import { ExtendedCaseTextBoxComponent, ExtendedCaseLabelComponent, ExtendedCaseTextBoxSearchComponent,
   ExtendedCaseTextAreaComponent, ExtendedCaseDropdownComponent, ExtendedCaseMultiselectComponent, ExtendedCaseDateComponent,
    ExtendedCaseCheckboxListComponent, ExtendedCaseCheckboxComponent, ExtendedCaseRadioComponent, ExtendedCaseReviewComponent,
     ExtendedCaseHtmlComponent, ExtendedUnknowControlComponent, ExtendedCaseFileUploadComponent } from './controls';
import { FormInfo } from '@app/models/proxy.model';


@Component({
    selector: 'ec-section',
    templateUrl: './ec-section.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class ExtendedCaseSectionComponent implements OnInit {
    @Input() sectionTemplate: SectionTemplateModel;
    @Input() sectionModel: SectionModel;
    @Input() formInfo: FormInfo;

    controlType = new FormControlType(); // to  use in html static string

    constructor(private componentCommService: ComponentCommService, private subscriptionManager: SubscriptionManager,
        private changeDetector: ChangeDetectorRef) {
    }

    ngOnInit(): void {
        // let self = this;
        // this.subscriptionManager.addSingle('onDigestComplete',
        //    this.componentCommService.digestCompletedSubject$
        //        .subscribe((result: DigestResult) => self.changeDetector.detectChanges()));

    }

    populateAction(sectionInstance: SectionInstanceModel) {
        let args = new PopulateSectionParams(sectionInstance, this.sectionTemplate);
        this.componentCommService.raisePopulateSectionEvent(args);
    }

    enableSectionChanged(chk: HTMLInputElement, sectionInstance: SectionInstanceModel) {
        let args = new EnableSectionParams(sectionInstance, this.sectionTemplate, chk.checked);
        this.componentCommService.raiseEnableSectionEvent(args);
    }

    getFormField(sectionIndex: number, controlId: string): FieldModelBase {
        return this.sectionModel.instances[sectionIndex].fields[controlId];
    }

    onAddSectionInstance() {
        // validate max sections
        let sectionsMaxCount = this.sectionTemplate.multiSectionAction.maxCount || 0;
        if (sectionsMaxCount > 0 && this.sectionModel.instances.length >= sectionsMaxCount) {
            // alert(`You have reached maximum number (${sectionsMaxCount}) of allowed sections.`);
            return;
        }

        let args = new AddSectionInstanceParams(this.sectionModel);
        this.componentCommService.raiseAddNewSectionInstance(args);
    }

    getAddSectionText() {
        let actionText = 'Add Section';
        if (this.sectionTemplate.multiSectionAction && this.sectionTemplate.multiSectionAction.actionName) {
            actionText = this.sectionTemplate.multiSectionAction.actionName;
        }
        return actionText;
    }

    showActionsPanel(): boolean {
        return this.sectionTemplate.multiSectionAction && this.sectionTemplate.multiSectionAction.allowMultipleSections;
    }

    showDeleteSectionInstanceButton(index: number): boolean {
        return this.sectionTemplate.multiSectionAction &&
            this.sectionTemplate.multiSectionAction.allowMultipleSections &&
            index > 0;
    }

    deleteSectionInstance(index: number, sectionInstance: SectionInstanceModel) {
        if (index > 0) {
            let args = new DeleteSectionInstanceParams(index, sectionInstance);
            this.componentCommService.raiseDeleteSectionInstance(args);
        }
    }


    trackById(index: any, item: SectionInstanceModel) {
        return item.id;
    }

    trackByControlId(index: any, item: BaseControlTemplateModel) {
        return item.id;
    }

    getControlByType(type: string): any {
        switch (type) {
            case FormControlType.Textbox:
                return ExtendedCaseTextBoxComponent;
            case FormControlType.Amount:
                return ExtendedCaseTextBoxComponent;
            case FormControlType.Percentage:
                return ExtendedCaseTextBoxComponent;
            case FormControlType.Unit:
                return ExtendedCaseTextBoxComponent;
            case FormControlType.Number:
                return ExtendedCaseTextBoxComponent;
            case FormControlType.AltNumber:
                return ExtendedCaseTextBoxComponent;
            case FormControlType.Label:
                return ExtendedCaseLabelComponent;
            case FormControlType.Search:
                return ExtendedCaseTextBoxSearchComponent;
            case FormControlType.Textarea:
                return ExtendedCaseTextAreaComponent;
            case FormControlType.Dropdown:
                return ExtendedCaseDropdownComponent;
            case FormControlType.Multiselect:
                return ExtendedCaseMultiselectComponent;
            case FormControlType.Date:
                return ExtendedCaseDateComponent;
            case FormControlType.CheckboxList:
                return ExtendedCaseCheckboxListComponent;
            case FormControlType.Checkbox:
                return ExtendedCaseCheckboxComponent;
            case FormControlType.Radio:
                return ExtendedCaseRadioComponent;
            case FormControlType.Review:
                return ExtendedCaseReviewComponent;
            case FormControlType.Html:
                return ExtendedCaseHtmlComponent;
            case FormControlType.FileUpload:
              return ExtendedCaseFileUploadComponent;
            default:
                return ExtendedUnknowControlComponent;
        }
    }
}
