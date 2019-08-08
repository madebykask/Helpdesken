import { Component, OnInit, Input, ViewChild, SimpleChanges } from '@angular/core';
import { CaseEditInputModel, CaseAccessMode } from '../../../models';
import { CaseFieldsNames } from 'src/app/modules/shared-module/constants';
import { MbscListviewOptions, MbscSwitch, MbscFormOptions } from '@mobiscroll/angular';
import { Subject } from 'rxjs';
import { CaseLogApiService } from '../../../services/api/case/case-log-api.service';
import { take, takeUntil } from 'rxjs/internal/operators';
import { CaseFormGroup, CaseFormControl } from 'src/app/modules/shared-module/models/forms';
import { TranslateService } from '@ngx-translate/core';
import { LogFileType } from 'src/app/modules/shared-module/constants/logFileType.enum';
import { FileUploadArgs } from '../controls/log-files-upload/log-files-upload.component';

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

  @ViewChild('sendMailToNotifierControl', { static: false }) sendMailToNotifierControl: MbscSwitch;

  logFileType = LogFileType;

  files: string[] = [];
  filesInternal: string[] = [];
  internalLogLabel = '';
  externalLogLabel = '';
  isExternalLogFieldVisible = false;
  isInternalLogFieldVisible = false;
  isAttachedFilesVisible = false;
  isAttachedInternalFilesVisible = false;
  caseFieldsNames = CaseFieldsNames;
  isSendMailToNotifierDisabled = false; // have to use this variable and [disabled] binding, because control.disabled dont work on switch
  externalLogEmailsTo = '';

  fileListSettings: MbscListviewOptions = {
    enhance: true,
    swipe: true,
    stages: [{
      percent: -30,
      color: 'red',
      icon: 'fa-trash',
      confirm: true,
      action: (event, inst) => this.onFileDelete(event, LogFileType.External)
    }]
  };

  fileInternalListSettings: MbscListviewOptions = {
    enhance: true,
    swipe: true,
    stages: [{
      percent: -30,
      color: 'red',
      icon: 'fa-trash',
      confirm: true,
      action:  (event, inst) => this.onFileDelete(event, LogFileType.Internal)
    }]
  };

  switchOptions: MbscFormOptions = {
    onInit: (event, inst) => {
      const e = event;
      const t = inst;
    }
  };

  private sendExternalEmailsFormControl: CaseFormControl = null;
  private externalLogEmailsCcFormControl: CaseFormControl = null;
  private externalLogFormControl: CaseFormControl = null;
  private internalLogFormControl: CaseFormControl = null;
  private logFileFormControl: CaseFormControl = null;
  private logFileInternalFormControl: CaseFormControl = null;
  private personsEmailFormControl: CaseFormControl = null;
  private noEmailsText = '';
  private destroy$ = new Subject();

  constructor(private caseLogApiService: CaseLogApiService,
    private translateService: TranslateService) {
  }

  get hasFullAccess() {
    return this.accessMode !== null && this.accessMode === CaseAccessMode.FullAccess;
  }

  get isSendMailToNotifierDisabledOrOff() {
    return this.isSendMailToNotifierDisabled || (this.sendExternalEmailsFormControl && this.sendExternalEmailsFormControl.value === false);
  }

  ngOnChanges(changes: SimpleChanges): void {
    //if (changes.accessMode && !changes.accessMode.firstChange) {
    //  console.log(`>>>> caseLogInput: AccessMode has changed!!! Value: ${changes.accessMode.previousValue} -> ${changes.accessMode.previousValue}`);
    //}
  }

  ngOnInit() {
    this.personsEmailFormControl = this.getFormControl(CaseFieldsNames.PersonEmail);
    this.sendExternalEmailsFormControl  = this.getFormControl(CaseFieldsNames.Log_SendMailToNotifier);
    this.externalLogEmailsCcFormControl = this.getFormControl(CaseFieldsNames.Log_ExternalEmailsCC);
    this.externalLogFormControl = this.getFormControl(CaseFieldsNames.Log_ExternalText);
    this.internalLogFormControl = this.getFormControl(CaseFieldsNames.Log_InternalText);
    this.logFileFormControl = this.getFormControl(CaseFieldsNames.Log_FileName);
    this.logFileInternalFormControl = this.getFormControl(CaseFieldsNames.Log_FileName_Internal);

    this.noEmailsText = this.translateService.instant('Ingen tillgänglig mailadress'); //No email address available

    if (this.sendExternalEmailsFormControl) {
      this.isSendMailToNotifierDisabled = this.sendExternalEmailsFormControl.disabled;
      if (this.isSendMailToNotifierDisabled) {
        this.onSendExternalEmailsStatusChanged(!this.isSendMailToNotifierDisabled);
      }
      // track send extneral control status change (disabled/enabled
      this.sendExternalEmailsFormControl.statusChanges.pipe(
        takeUntil(this.destroy$)
      ).subscribe(e => {
        // process only if disabled state has changed, ignore same values
        const isDisabled = this.sendExternalEmailsFormControl.isDisabled || this.externalLogFormControl.disabled;
        if (this.isSendMailToNotifierDisabled !== isDisabled) {
          this.isSendMailToNotifierDisabled = isDisabled;
          this.onSendExternalEmailsStatusChanged(!this.isSendMailToNotifierDisabled);
        }
      });

      // external emails text logic (depends on personsEmail form control value and on this.sendExternalEmailsFormControl.disabled)
      this.updateExternalEmailsText();
    }

    if (this.personsEmailFormControl) {
      this.personsEmailFormControl.valueChanges.pipe(
        takeUntil(this.destroy$)
        ).subscribe(v => {
          this.updateExternalEmailsText();
        });
    }

    if (this.externalLogFormControl) {
      this.isExternalLogFieldVisible =  !this.externalLogFormControl.fieldInfo.isHidden;
    }

    if (this.internalLogFormControl) {
      this.isInternalLogFieldVisible =  !this.internalLogFormControl.fieldInfo.isHidden;
    }

    if (this.logFileFormControl) {
      this.isAttachedFilesVisible = !this.logFileFormControl.fieldInfo.isHidden;
    }

    if (this.logFileInternalFormControl) {
      this.isAttachedInternalFilesVisible = !this.logFileInternalFormControl.fieldInfo.isHidden;
    }
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  // handles UI switch change by user
  onSendExternalEmailsCheckChanged() {
    const val = this.sendExternalEmailsFormControl.value;
    if (val) {
      this.externalLogEmailsCcFormControl.enable({onlySelf: true, emitEvent: true});
      this.externalLogEmailsCcFormControl.restorePrevValue();
    } else {
      this.externalLogEmailsCcFormControl.disable({onlySelf: true, emitEvent: true});
      this.externalLogEmailsCcFormControl.setValue('');
    }
    this.updateExternalEmailsText();
  }

  processFileUploaded(params: FileUploadArgs) {
    this.getFiles(params.type).push(params.file);
  }

  onFileDelete(event, type: LogFileType) {
    const index = +event.index ;
    const fileName = this.getFiles(type)[index];

    // todo:add delete confirmation
    this.caseLogApiService.deleteTempLogFile(this.caseKey, fileName, type).pipe(
      take(1)
    ).subscribe(res => {
      if (res) {
        this.getFiles(type).splice(index, 1);
      }
    });
  }

  // handles switch enable/disable state change (case lock, Substatus change,..)
  private onSendExternalEmailsStatusChanged(enable) {
    if (enable) {
      this.sendExternalEmailsFormControl.setValue(true, { emitEvent: true });
      this.externalLogEmailsCcFormControl.enable({onlySelf: true, emitEvent: true});
      this.externalLogEmailsCcFormControl.restorePrevValue();
    } else {
      this.sendExternalEmailsFormControl.setValue(false, { emitEvent: true });
      this.externalLogEmailsCcFormControl.disable({onlySelf: true, emitEvent: true});
      this.externalLogEmailsCcFormControl.setValue('');
    }
    this.updateExternalEmailsText();
  }

  private getFiles(type: LogFileType) {
    return type == LogFileType.External ? this.files : this.filesInternal;
  }

  private updateExternalEmailsText() {
    if (this.sendExternalEmailsFormControl.value === false || this.isSendMailToNotifierDisabled) {
      this.externalLogEmailsTo =  '&nbsp;'; // required to keep empty row, otherwise it gets collapsed (bug)
    } else {
      const externalEmailTo = (this.personsEmailFormControl.value || '').toString();
      this.externalLogEmailsTo = externalEmailTo.toLowerCase() || this.noEmailsText;
    }
  }

  protected getFormControl(name: string): CaseFormControl {
    if (this.form === null) { return null; }
    return this.form.get(name);
  }
}
