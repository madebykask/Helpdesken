import { Component, OnInit, SimpleChanges } from '@angular/core';
import { CaseLogActionData } from 'src/app/modules/case-edit-module/models';
import { LogFile } from 'src/app/modules/case-edit-module/models/case/case-actions-api.model';
import { CaseFilesApiService } from 'src/app/modules/case-edit-module/services/api/case/case-files-api.service';

import { CaseActionBaseComponent } from '../case-action-base.component';

@Component({
  selector: 'app-log-note-action',
  templateUrl: './log-note-action.component.html',
  styleUrls: ['./log-note-action.component.scss']
})
export class LogNoteActionComponent extends CaseActionBaseComponent<CaseLogActionData> {

  constructor(private filesApiService: CaseFilesApiService) { 
    super();
  } 

  get data(): CaseLogActionData {
    return this.caseAction != null ? this.caseAction.data : null;
  }  

   downloadLogFile(file: LogFile) {
     const caseId = parseInt(this.caseKey);
     this.filesApiService.downloadLogFile(file.id, caseId);
  }
  
  ngOnChanges(changes: SimpleChanges): void {
    // note: is not called for dynamic components!
  }
}
