import { Component, OnInit, Input } from '@angular/core';
import { CaseEditInputModel, CaseAccessMode } from '../../../models';
import { CaseFieldsNames } from 'src/app/modules/shared-module/constants';
import { MbscListviewOptions } from '@mobiscroll/angular';
import { Subject } from 'rxjs';
import { CaseLogApiService } from '../../../services/api/case/case-log-api.service';
import { take } from 'rxjs/internal/operators';
import { CaseFormGroup, CaseFormControl } from 'src/app/modules/shared-module/models/forms';

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

  externalLogEmailsTo = '';
  files: string[] = [];
  internalLogLabel = '';
  externalLogLabel = '';
  isExternalLogFieldVisible = false;
  isInternalLogFieldVisible = false;
  isAttachedFilesVisible = false;
  caseFieldsNames = CaseFieldsNames;

  sendExternalEmailsControl: CaseFormControl = null;
  externalLogField: CaseFormControl = null;
  internalLogField: CaseFormControl = null;
  logFileField: CaseFormControl = null;

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

  constructor(private caseLogApiService: CaseLogApiService) {
  }

  get hasFullAccess() {
    return this.accessMode !== null && this.accessMode === CaseAccessMode.FullAccess;
  }

  ngOnInit() {
    // TODO: check how external Log emails TO (Followers) should be initialised?
    const externalEmailsToControl = this.getFormControl(CaseFieldsNames.Log_ExternalEmailsTo);
    if (externalEmailsToControl.value && externalEmailsToControl.value.length) {
      this.externalLogEmailsTo = externalEmailsToControl.value.toString();
    } else {
      this.externalLogEmailsTo = 'No email address available'; // todo: translate
    }

    this.externalLogField = this.getFormControl(CaseFieldsNames.Log_ExternalText);
    this.internalLogField = this.getFormControl(CaseFieldsNames.Log_InternalText);
    this.logFileField = this.getFormControl(CaseFieldsNames.Log_FileName);

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
    if (this.form === null) return null;
    return this.form.get(name);
  }

  onFileDelete(event){
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
