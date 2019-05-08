import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { CaseEditInputModel, CaseAccessMode } from '../../../models';
import { CaseFieldsNames } from 'src/app/modules/shared-module/constants';
import { MbscListviewOptions, MbscSwitch, MbscFormOptions } from '@mobiscroll/angular';
import { Subject } from 'rxjs';
import { CaseLogApiService } from '../../../services/api/case/case-log-api.service';
import { take, takeUntil } from 'rxjs/internal/operators';
import { CaseFormGroup, CaseFormControl } from 'src/app/modules/shared-module/models/forms';
import { TranslateService } from '@ngx-translate/core';

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

  @ViewChild('sendMailToNotifierControl') sendMailToNotifierControl: MbscSwitch;

  files: string[] = [];
  internalLogLabel = '';
  externalLogLabel = '';
  isExternalLogFieldVisible = false;
  isInternalLogFieldVisible = false;
  isAttachedFilesVisible = false;
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
      action: this.onFileDelete.bind(this)
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
  private personsEmailFormControl: CaseFormControl = null;
  private noEmailsText = '';
  private destroy$ = new Subject();

  constructor(private caseLogApiService: CaseLogApiService,
    private translateService: TranslateService) {
  }

  get hasFullAccess() {
    return this.accessMode !== null && this.accessMode === CaseAccessMode.FullAccess;
  }

  ngOnInit() {
    this.personsEmailFormControl = this.getFormControl(CaseFieldsNames.PersonEmail);
    this.sendExternalEmailsFormControl  = this.getFormControl(CaseFieldsNames.Log_SendMailToNotifier);
    this.externalLogEmailsCcFormControl = this.getFormControl(CaseFieldsNames.Log_ExternalEmailsCC);
    this.externalLogFormControl = this.getFormControl(CaseFieldsNames.Log_ExternalText);
    this.internalLogFormControl = this.getFormControl(CaseFieldsNames.Log_InternalText);
    this.logFileFormControl = this.getFormControl(CaseFieldsNames.Log_FileName);

    this.noEmailsText = this.translateService.instant('Ingen tillgänglig mailadress'); //No email address available

    if (this.sendExternalEmailsFormControl) {
      this.isSendMailToNotifierDisabled = this.sendExternalEmailsFormControl.disabled;    

      // track send extneral control status change (disabled/enabled) for switch
      this.sendExternalEmailsFormControl.statusChanges.pipe(
        takeUntil(this.destroy$)
      ).subscribe(e => {
        if (this.sendMailToNotifierControl.disabled !== this.sendExternalEmailsFormControl.isDisabled) {
          this.isSendMailToNotifierDisabled = this.sendExternalEmailsFormControl.disabled;
        }
      });
    }

    // external emails text logic (depends on personsEmail form control value and on this.sendExternalEmailsFormControl.disabled)    
    this.updateExternalEmailsText();
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
  }

  onChange(e) {
   const t = 'test';
  }

  onSendExternalEmailsCheckChanged(e) {
    const val = this.sendExternalEmailsFormControl.value;
    const val2 = this.sendMailToNotifierControl.value;
    if (val) {
      this.externalLogEmailsCcFormControl.enable({onlySelf: true, emitEvent: true});
      this.externalLogEmailsCcFormControl.restorePrevValue();
    } else {
      this.externalLogEmailsCcFormControl.disable({onlySelf: true, emitEvent: true});
      this.externalLogEmailsCcFormControl.setValue('');
    }
    this.updateExternalEmailsText();
  }

  private updateExternalEmailsText() {
      if (this.sendExternalEmailsFormControl.value) {
        this.externalLogEmailsTo = (this.personsEmailFormControl.value || '').toString() || this.noEmailsText;        
      } else {
        this.externalLogEmailsTo =  '';
      }
  }

  processFileUploaded(file: string) {
    this.files.push(file);
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

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

}
