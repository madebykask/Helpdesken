import { Component, OnInit, Input, SimpleChanges, ViewChild } from '@angular/core';
import { CaseFileModel } from 'src/app/models/case/case-file-model';
import { BaseCaseField } from 'src/app/models';
import { CaseFilesApiService } from 'src/app/services/api/case/case-files-api.service'
import { CaseApiService } from 'src/app/services/api/case/case-api.service';
import { MbscListviewOptions, mobiscroll } from '@mobiscroll/angular';
import { CaseFilesUploadComponent } from '../case-files-upload/case-files-upload.component';
import { takeUntil, catchError, take } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { AlertsService } from 'src/app/helpers/alerts/alerts.service';
import { AlertType } from 'src/app/helpers/alerts/alert-types';

@Component({
  selector: 'case-files-control',
  templateUrl: './case-files-control.component.html',
  styleUrls: ['./case-files-control.component.scss']
})
export class CaseFilesControlComponent implements OnInit  {

  @Input() field: BaseCaseField<Array<any>>; 
  @Input() caseKey: string; 

  files: CaseFileModel[] = [];
  
  fileListSettings:MbscListviewOptions = {
    enhance: true,
    swipe: true,
    //todo: add swipe effects for delete
    stages: [{
      percent: -25,
      color: 'red',
      text: 'Delete', //todo: translate
      confirm: true,
      action: this.onFileDelete.bind(this)
    }]  
  };

  @ViewChild(CaseFilesUploadComponent) caseFilesComponent: CaseFilesUploadComponent

  private onDestroy = new Subject();

  constructor(private caseApiService: CaseApiService,
              private caseFilesApiService: CaseFilesApiService,
              private alertsService: AlertsService) {
  }

  ngOnInit() {
    //subscribe on new file upload
    this.caseFilesComponent.NewFileUploadComplete.pipe(
      takeUntil(this.onDestroy)
    ).subscribe(x => this.processNewFileUpload(x));
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }
   
  ngOnChanges(changes: SimpleChanges): void {
    // create  files list model to bind
    if (changes['field'] && changes['field'].currentValue.value !== undefined) {
        let fieldValue = changes['field'].currentValue as BaseCaseField<Array<any>>;
        if (fieldValue && fieldValue.value) {
          let items = fieldValue.value || [];
          this.files = items.map(f => new CaseFileModel(f.id, f.fileName));
        }
    }    
  }

  private processNewFileUpload(data: { id:number, name:string }) {
    if (data) {
        this.files.push(new CaseFileModel(data.id, data.name));
    }
  }

  downloadFile(item: CaseFileModel) {
    //todo: handle New case files download! 
    let caseId = +this.caseKey;
    this.caseFilesApiService.downloadCaseFile(caseId, item.fileId, item.fileName);
  }
  
  onFileDelete(event, inst){
    let self = this;
    let index = +event.index;
    if (!isNaN(index) && this.files.length > index) {
        let fileItem = self.files[index];
        if (fileItem) {
              //todo: move confirm to a separate service!
              //todo: add translations!
              mobiscroll.confirm({
                title: "Confirm",
                display: 'bottom',
                message: 'Do you want to delete the file?', 
                okText: 'Yes',
                cancelText: 'No'
              }).then(function (result) {
                  if (result) {
                      self.caseFilesApiService.deleteCaseFile(self.caseKey,fileItem.fileId, fileItem.fileName).pipe(
                          take(1)
                      ).subscribe(() => {
                          //remove fileItem from the list on success only
                          self.files = self.files.filter(el => el !== fileItem);
                      }, 
                      (err) => {
                        self.alertsService.showMessage('Failed to delete a file', AlertType.Error);
                        console.error(err);
                      });
                  }
            });
        }
    }
  }

  identify(item) {
        return item.fileId;
  }

}