import { Component, OnInit, Input, ViewChild, AfterViewInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { CaseEditDataHelper } from '../../../logic/case-edit/case-editdata.helper';
import { CaseEditInputModel, BaseCaseField } from '../../../models';
import { CaseFieldsNames } from 'src/app/modules/shared-module/constants';
import { LogFilesUploadComponent } from '../controls/log-files-upload/log-files-upload.component';
import { MbscListviewOptions } from '@mobiscroll/angular';
import { takeUntil, take } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { CaseLogApiService } from '../../../services/api/case/case-log-api.service';

@Component({
  selector: 'case-log-input',
  templateUrl: './case-log-input.component.html',
  styleUrls: ['./case-log-input.component.scss']
})
export class CaseLogInputComponent implements OnInit, AfterViewInit {
  
  @Input() caseKey:string;
  @Input() form: FormGroup;
  @Input() caseData: CaseEditInputModel;   

  @ViewChild(LogFilesUploadComponent) fileUpload: LogFilesUploadComponent;

  files:string[] = [];
  caseFieldsNames = CaseFieldsNames;
  internalLogLabel:string = '';
  externalLogLabel:string = '';
  externalLogField:BaseCaseField<string> = null;
  internalLogField:BaseCaseField<string> = null;

  private destroy$ = new Subject();

  fileListSettings: MbscListviewOptions = {
    enhance: true,
    swipe: true,
    //todo: add swipe effects for delete
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
    
    if (this.externalLogField) {
      this.externalLogLabel = this.externalLogField.label + (this.externalLogField.isRequired ? '*' : '');  
    }

    if (this.internalLogField) {
      this.internalLogLabel = this.internalLogField.label + (this.internalLogField.isRequired ? '*' : '');
    }
  }

  ngAfterViewInit(): void {
    //subscribe on new files upload events
    this.fileUpload.fileUploaded.pipe(
      takeUntil(this.destroy$)
    ).subscribe(x => this.files.push(x))
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
  
  onFileDelete(event, inst){
    let index = +event.index ;
    let fileName = this.files[index];
    
    //todo:add delete confirmation
    this.caseLogApiService.deleteTempLogFile(this.caseKey, fileName).pipe(
      take(1)
    )
    .subscribe(res => {
      if (res) {
        this.files.splice(index, 1);
      }
    })
  }

}
