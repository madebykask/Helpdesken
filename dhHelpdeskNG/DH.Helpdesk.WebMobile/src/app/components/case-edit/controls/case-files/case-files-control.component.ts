import { Component, OnInit, OnChanges, Input } from '@angular/core';
import { CaseFileModel } from 'src/app/models/case/case-file-model';
import { BaseCaseField, OptionItem } from 'src/app/models';
import { FileDownloadService } from 'src/app/services/files/file-download.service'
import { CaseApiService } from 'src/app/services/api/case/case-api.service';
import { MbscListviewOptions } from '@mobiscroll/angular';

@Component({
  selector: 'case-files-control',
  templateUrl: './case-files-control.component.html',
  styleUrls: ['./case-files-control.component.scss']
})
export class CaseFilesControlComponent implements OnInit  {

  @Input() field: BaseCaseField<Array<any>>; 
  @Input() caseKey: string; 

  fileListSettings:MbscListviewOptions = {
    enhance: true,
    swipe: true,
    //todo: add swipe effects for delete
    stages: [{
      percent: -25,
      color: 'red',
      text: 'Delete',
      confirm: true,
      action: (event, inst) => {
          console.log('delete file');
      }
    }]  
  };

  constructor(private caseApiService: CaseApiService,
              private fileDownloadService: FileDownloadService) { 
  }

  ngOnInit() { }
   
  getFilesModel() {
    var items = this.field.value || new Array<any>();
    return items.map(f => new CaseFileModel(f.caseId, f.id, f.fileName));
  }  

  buildCaseFileUrl(item: CaseFileModel) {
    let url = this.caseApiService.buildResourseUrl(`/api/case/${item.caseId}/file/${item.fileId}`, null, true, false)
    return url;
  }

  downloadFile(item: CaseFileModel) {
    this.fileDownloadService.downloadCaseFile(item.caseId, item.fileId, item.fileName);
  }
 
  identify(item) {
    return item.fileId;
  }
}