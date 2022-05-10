import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { CaseEditInputModel, CaseAccessMode } from '../../../models';
import { CaseFieldsNames } from 'src/app/modules/shared-module/constants';
import { MbscListviewOptions, MbscFormOptions } from '@mobiscroll/angular';
import { CaseLogApiService } from '../../../services/api/case/case-log-api.service';
import { delay, take } from 'rxjs/internal/operators';
import { untilDestroyed } from 'ngx-take-until-destroy';
import { CaseFormGroup, CaseFormControl } from 'src/app/modules/shared-module/models/forms';
import { TranslateService } from '@ngx-translate/core';
import { LogFileType } from 'src/app/modules/shared-module/constants/logFileType.enum';
import { FileUploadArgs } from '../controls/log-files-upload/log-files-upload.component';
import { Channels, CommunicationService } from 'src/app/services/communication';
import { EmailEventData } from 'src/app/services/communication/data/email-event-data';
import { PerfomersService } from 'src/app/services/case-organization/perfomers-service';

@Component({
  selector: 'case-log-input',
  templateUrl: './case-log-input.component.html',
  styleUrls: ['./case-log-input.component.scss']
})
export class CaseLogInputComponent implements OnInit {
  @Input() caseKey: string;
  @Input() customerId: number;
  @Input() form: CaseFormGroup;
  @Input() caseData: CaseEditInputModel;
  @Input() accessMode: CaseAccessMode;

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
  isSendMailToPerformerDisabled = false;
  externalLogEmailsTo = '';
  performerUserEmail = '';

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
      action: (event, inst) => this.onFileDelete(event, LogFileType.Internal)
    }]
  };

  switchOptions: MbscFormOptions = {
    onInit: (event, inst) => {
      const e = event;
      const t = inst;
    }
  };

  private sendExternalEmailsFormControl: CaseFormControl = null;
  private sendInternalEmailsFormControl: CaseFormControl = null;
  private externalLogEmailsCcFormControl: CaseFormControl = null;
  private externalLogFormControl: CaseFormControl = null;
  private internalLogFormControl: CaseFormControl = null;
  private logFileFormControl: CaseFormControl = null;
  private logFileInternalFormControl: CaseFormControl = null;
  private personsEmailFormControl: CaseFormControl = null;
  private noEmailsText = '';

  constructor(private caseLogApiService: CaseLogApiService,
    private translateService: TranslateService,
    private commService: CommunicationService,
    
    private performersService: PerfomersService) {
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
    this.sendExternalEmailsFormControl = this.getFormControl(CaseFieldsNames.Log_SendMailToNotifier);
    this.sendInternalEmailsFormControl = this.getFormControl(CaseFieldsNames.Log_SendMailToPerformer);
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
        untilDestroyed(this)
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

    if (this.sendInternalEmailsFormControl) {
      this.isSendMailToPerformerDisabled = this.sendInternalEmailsFormControl.disabled;
      if (this.isSendMailToPerformerDisabled) {
        this.onSendInternalEmailsStatusChanged(!this.isSendMailToPerformerDisabled);
      }
      // track send extneral control status change (disabled/enabled
      this.sendInternalEmailsFormControl.statusChanges.pipe(
        untilDestroyed(this)
      ).subscribe(e => {
        // process only if disabled state has changed, ignore same values
        const isDisabled = this.sendInternalEmailsFormControl.isDisabled || this.internalLogFormControl.disabled;
        if (this.isSendMailToPerformerDisabled !== isDisabled) {
          this.isSendMailToPerformerDisabled = isDisabled;
          this.onSendInternalEmailsStatusChanged(!isDisabled);
        }
      });

      // external emails text logic (depends on personsEmail form control value and on this.sendExternalEmailsFormControl.disabled)
      // this.updateExternalEmailsText();
    }

    if (this.personsEmailFormControl) {
      this.personsEmailFormControl.valueChanges.pipe(
        untilDestroyed(this)
      ).subscribe(v => {
        this.updateExternalEmailsText();
      });
    }

    this.commService.listen<EmailEventData>(Channels.CaseFieldValueChanged)
      .pipe(
        delay(0),
        untilDestroyed(this)
      )
      .subscribe(e => {
        e.eMail = e.eMail === undefined ? '' : e.eMail; 
        this.performerUserEmail = e.eMail;
      });

    if (this.externalLogFormControl) {
      this.isExternalLogFieldVisible = !this.externalLogFormControl.fieldInfo.isHidden;
    }

    if (this.internalLogFormControl) {
      this.isInternalLogFieldVisible = !this.internalLogFormControl.fieldInfo.isHidden;
    }

    if (this.logFileFormControl) {
      this.isAttachedFilesVisible = !this.logFileFormControl.fieldInfo.isHidden;
    }

    if (this.logFileInternalFormControl) {
      this.isAttachedInternalFilesVisible = !this.logFileInternalFormControl.fieldInfo.isHidden;
    }

    let performanceUserId = this.caseData.fields.filter(x=> x.name === 'PerformerUserId')[0].value;
    if (!isNaN(parseInt(performanceUserId))) {
      let performerUserIdInt = parseInt(performanceUserId);
      if (performerUserIdInt > 0) {
        this.performersService.getPerformerEmail(performerUserIdInt).pipe(take(1)).subscribe((m) => {
          // console.log(m)
          this.commService.publish(Channels.CaseFieldValueChanged, new EmailEventData(m.eMail));
        });
      }   
    } 
  }

  ngOnDestroy() {
  }

  // handles UI switch change by user
  onSendExternalEmailsCheckChanged() {
    const val = this.sendExternalEmailsFormControl.value;
    if (val) {
      this.externalLogEmailsCcFormControl.enable({ onlySelf: true, emitEvent: true });
      this.externalLogEmailsCcFormControl.restorePrevValue();
    } else {
      this.externalLogEmailsCcFormControl.disable({ onlySelf: true, emitEvent: true });
      this.externalLogEmailsCcFormControl.setValue('');
    }
    this.updateExternalEmailsText();
  }

  onSendInternalEmailsCheckChanged() {
    const val = this.sendInternalEmailsFormControl.value;
   
    if (val) {
      this.sendInternalEmailsFormControl.enable({ onlySelf: true, emitEvent: true });
      // this.externalLogEmailsCcFormControl.restorePrevValue();
    } else {
      this.sendInternalEmailsFormControl.disable({ onlySelf: true, emitEvent: true });
      // this.externalLogEmailsCcFormControl.setValue('');
    }
    // this.updateExternalEmailsText();
  }

  processFileUploaded(params: FileUploadArgs) {
    this.getFiles(params.type).push(params.file);
  }

  onFileDelete(event, type: LogFileType) {
    const index = +event.index;
    const fileName = this.getFiles(type)[index];

    // todo:add delete confirmation
    this.caseLogApiService.deleteTempLogFile(this.caseKey, fileName, type, this.customerId).pipe(
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
      this.externalLogEmailsCcFormControl.enable({ onlySelf: true, emitEvent: true });
      this.externalLogEmailsCcFormControl.restorePrevValue();
    } else {
      this.sendExternalEmailsFormControl.setValue(false, { emitEvent: true });
      this.externalLogEmailsCcFormControl.disable({ onlySelf: true, emitEvent: true });
      this.externalLogEmailsCcFormControl.setValue('');
    }
    this.updateExternalEmailsText();
  }

  // handles switch enable/disable state change (case lock, Substatus change,..)
  private onSendInternalEmailsStatusChanged(enable) {
    if (enable) {
      this.sendInternalEmailsFormControl.setValue(true, { emitEvent: true });
    } 
    else {
      this.sendInternalEmailsFormControl.setValue(false, { emitEvent: true });
    }
    // this.updateExternalEmailsText();
  }

  private getFiles(type: LogFileType) {
    return type == LogFileType.External ? this.files : this.filesInternal;
  }

  private updateExternalEmailsText() {
    if (this.sendExternalEmailsFormControl.value === false || this.isSendMailToNotifierDisabled) {
      this.externalLogEmailsTo = '&nbsp;'; // required to keep empty row, otherwise it gets collapsed (bug)
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
