import { Component, OnInit, OnChanges, Input } from '@angular/core';
import { CaseFileModel } from 'src/app/models/case/case-file-model';
import { CaseService } from 'src/app/services/case/case.service';
import { BaseCaseField, OptionItem } from 'src/app/models';
import { FileDownloadService } from 'src/app/services/files/file-download.service'

@Component({
  selector: 'case-files-control',
  templateUrl: './case-files-control.component.html',
  styleUrls: ['./case-files-control.component.scss']
})
export class CaseFilesControlComponent implements OnInit  {

  @Input() field: BaseCaseField<Array<any>>; 

  constructor(private caseService: CaseService,
              private fileDownloadService: FileDownloadService) { 
  }

  ngOnInit() { }  
   
  getFilesModel() {        
    var items = this.field.value || new Array<any>();       
    return items.map(f => new CaseFileModel(f.caseId, f.id, f.fileName));    
  }  

  buildCaseFileUrl(item: CaseFileModel){
    return this.caseService.buildCaseFileUrl(item.caseId, item.fileId);
  }  

  downloadFile(item: CaseFileModel){
    this.fileDownloadService.downloadCaseFile(item.caseId, item.fileId, item.fileName);
  }
 
  identify(item) {
    return item.fileId;
  }
}