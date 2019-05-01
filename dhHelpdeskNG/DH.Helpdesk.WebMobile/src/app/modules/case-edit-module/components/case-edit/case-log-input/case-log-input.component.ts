import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { CaseEditInputModel, CaseAccessMode } from '../../../models';
import { CaseFieldsNames } from 'src/app/modules/shared-module/constants';
import { MbscListviewOptions, MbscSwitch } from '@mobiscroll/angular';
import { Subject } from 'rxjs';
import { CaseLogApiService } from '../../../services/api/case/case-log-api.service';
import { take, takeUntil } from 'rxjs/internal/operators';
import { CaseFormGroup, CaseFormControl } from 'src/app/modules/shared-module/models/forms';
import { CommunicationService, Channels, FormValueChangedEvent } from 'src/app/services/communication';

@Component({
  selector: 'case-log-input',
  templateUrl: './case-log-input.component.html',
  styleUrls: ['./case-log-input.component.scss']
})
export class CaseLogInputComponent implements OnInit {
  @Input() caseKey: string;
  @Input() form: CaseFormGroup;
  @Input() caseData: CaseEditInputModel;
  @Input() accessMode: CaseAccessMode;

  files: string[] = [];
  internalLogLabel = '';
  externalLogLabel = '';
  isExternalLogFieldVisible = false;
  isInternalLogFieldVisible = false;
  isAttachedFilesVisible = false;
  caseFieldsNames = CaseFieldsNames;

  sendExternalEmailsControl: CaseFormControl = null;
  externalLogEmailsToControl: CaseFormControl = null;
  externalLogEmailsCcControl: CaseFormControl = null;
  externalLogField: CaseFormControl = null;
  internalLogField: CaseFormControl = null;
  logFileField: CaseFormControl = null;
  personsEmailFormControl: CaseFormControl = null;

  fileListSettings: MbscListviewOptions = {
    enhance: true,
    swipe: true,
    stages: [{
      percent: -30,
      color: 'red',
      icon: 'fa-trash',
      confirm: true,
      action: this.onFileDelete.bind(this)
    }]
  };

  private destroy$ = new Subject();

  constructor(private caseLogApiService: CaseLogApiService, private commService: CommunicationService) {
  }

  get hasFullAccess() {
    return this.accessMode !== null && this.accessMode === CaseAccessMode.FullAccess;
  }

  get externalLogEmailsTo(): string {
    let val = 'No email address available'; // todo: translate!
    if (this.personsEmailFormControl && this.personsEmailFormControl.value && this.personsEmailFormControl.value.length) {
      val = this.personsEmailFormControl.value.toString();
    }
    return val;
  }

  ngOnInit() {
    this.personsEmailFormControl = this.getFormControl(CaseFieldsNames.PersonEmail);
    this.sendExternalEmailsControl  = this.getFormControl(CaseFieldsNames.Log_SendMailToNotifier);
    this.externalLogEmailsToControl = this.getFormControl(CaseFieldsNames.Log_ExternalEmailsTo);
    this.externalLogEmailsCcControl = this.getFormControl(CaseFieldsNames.Log_ExternalEmailsCC);
    this.externalLogField = this.getFormControl(CaseFieldsNames.Log_ExternalText);
    this.internalLogField = this.getFormControl(CaseFieldsNames.Log_InternalText);
    this.logFileField = this.getFormControl(CaseFieldsNames.Log_FileName);

    if (this.sendExternalEmailsControl) {
      this.sendExternalEmailsControl.valueChanges.pipe(
        takeUntil(this.destroy$)
        ).subscribe(v => {
            this.commService.publish(Channels.FormValueChanged, new FormValueChangedEvent(v, '', this.sendExternalEmailsControl.fieldName));
        });
    }

    if (this.externalLogField) {
      this.isExternalLogFieldVisible =  !this.externalLogField.fieldInfo.isHidden;
    }

    if (this.internalLogField) {
      this.isInternalLogFieldVisible =  !this.internalLogField.fieldInfo.isHidden;
    }

    if (this.logFileField) {
      this.isAttachedFilesVisible = !this.logFileField.fieldInfo.isHidden;
    }
  }

  ngAfterViewInit(): void {
  }

  processFileUploaded(file: string) {
    this.files.push(file);
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  protected getFormControl(name: string): CaseFormControl {
    if (this.form === null) { return null; }
    return this.form.get(name);
  }

  onFileDelete(event) {
    const index = +event.index ;
    const fileName = this.files[index];

    // todo:add delete confirmation
    this.caseLogApiService.deleteTempLogFile(this.caseKey, fileName).pipe(
      take(1)
    ).subscribe(res => {
      if (res) {
        this.files.splice(index, 1);
      }
    });
  }
}
