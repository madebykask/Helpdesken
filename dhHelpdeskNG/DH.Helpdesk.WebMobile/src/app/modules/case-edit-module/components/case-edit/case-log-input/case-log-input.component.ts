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
  @ViewChild('external') externalCtrl: any
  @ViewChild('internal') internalCtrl: any
  
  @Input() caseKey: string;
  @Input() form: CaseFormGroup;
  @Input() caseData: CaseEditInputModel;
  @Input() accessMode: CaseAccessMode;

  files: string[] = [];
  caseFieldsNames = CaseFieldsNames;
  internalLogLabel: string = '';
  externalLogLabel: string = '';
  externalLogField: BaseCaseField<string> = null;
  internalLogField: BaseCaseField<string> = null;

  get hasFullAccess() {
    return this.accessMode !== null && this.accessMode === CaseAccessMode.FullAccess;
  }

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

  constructor(private caseDataHelpder: CaseEditDataHelper, // TODO: review caseDataHelper usage
              private caseLogApiService: CaseLogApiService,
              private renderer: Renderer2) {
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
    const applyMaxLength = (ctrl, maxLength: number) => {
      const inputControl = ctrl.initialElem.nativeElement.querySelector('textarea');
      if (inputControl) {
        this.renderer.setAttribute(inputControl, 'maxlength', maxLength.toString());
      }
    };
    if (this.externalLogField.maxLength) {
      applyMaxLength(this.externalCtrl, this.externalLogField.maxLength);
    }
    if (this.internalLogField.maxLength) {
      applyMaxLength(this.internalCtrl, this.internalLogField.maxLength);
    }
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
    let index = +event.index ;
    let fileName = this.files[index];
    
    //todo:add delete confirmation
    this.caseLogApiService.deleteTempLogFile(this.caseKey, fileName).pipe(
      take(1)
    ).subscribe(res => {
      if (res) {
        this.files.splice(index, 1);
      }
    })
  }

}
