import { Component, OnInit, Input, ViewChild, ElementRef, Renderer2 } from '@angular/core';
import { CaseEditDataHelper } from '../../../logic/case-edit/case-editdata.helper';
import { CaseEditInputModel, BaseCaseField, CaseAccessMode } from '../../../models';
import { CaseFieldsNames } from 'src/app/modules/shared-module/constants';
import { MbscListviewOptions } from '@mobiscroll/angular';
import { Subject } from 'rxjs';
import { CaseLogApiService } from '../../../services/api/case/case-log-api.service';
import { take } from 'rxjs/internal/operators';
import { CaseFormGroup } from 'src/app/modules/shared-module/models/forms';


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
  caseFieldsNames = CaseFieldsNames;
  internalLogLabel = '';
  externalLogLabel = '';
  isExternalLogFieldVisible = false;
  isInternalLogFieldVisible = false;
  isAttachedFilesVisible = false;

  externalLogField: BaseCaseField<string> = null;
  internalLogField: BaseCaseField<string> = null;
  logFileField: BaseCaseField<any> = null;

  get hasFullAccess() {
    return this.accessMode !== null && this.accessMode === CaseAccessMode.FullAccess;
  }

  private destroy$ = new Subject();

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

  constructor(private caseDataHelpder: CaseEditDataHelper,
              private caseLogApiService: CaseLogApiService) {
  }

  ngOnInit() {
    this.externalLogField = this.getField(CaseFieldsNames.Log_ExternalText);
    this.internalLogField = this.getField(CaseFieldsNames.Log_InternalText);
    this.logFileField = this.getField(CaseFieldsNames.Log_FileName);

    if (this.externalLogField) {
      this.isExternalLogFieldVisible =  !this.externalLogField.isHidden;
    }

    if (this.internalLogField) {
      this.isInternalLogFieldVisible =  !this.internalLogField.isHidden;
    }

    if (this.logFileField) {
      this.isAttachedFilesVisible = !this.logFileField.isHidden;
    }
  }

  ngAfterViewInit(): void {
  }

  processFileUploaded(file:string) {
    this.files.push(file);
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  hasField(name: string): boolean {
     return this.caseDataHelpder.hasField(this.caseData, name);
  }

  getField(name: string): BaseCaseField<any> {
    return this.caseDataHelpder.getField(this.caseData, name);
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
