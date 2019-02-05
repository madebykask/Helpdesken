import { Component, OnInit } from '@angular/core';
import { CaseLogActionData } from 'src/app/modules/case-edit-module/models';
import { LogFile } from 'src/app/modules/case-edit-module/models/case/case-actions-api.model';
import { CaseFilesApiService } from 'src/app/modules/case-edit-module/services/api/case/case-files-api.service';

import { CaseActionBaseComponent } from '../case-action-base.component';

@Component({
  selector: 'app-log-note-action',
  templateUrl: './log-note-action.component.html',
  styleUrls: ['./log-note-action.component.scss']
})
export class LogNoteActionComponent extends CaseActionBaseComponent<CaseLogActionData> implements OnInit {

  constructor(private filesApiService: CaseFilesApiService) { 
    super();
  }

  ngOnInit() {
  }

  get data(): CaseLogActionData {
    return this.caseAction != null ? this.caseAction.data : null;
  }  

   downloadLogFile(file: LogFile) {
    this.filesApiService.downloadLogFile(file.id, parseInt(this.caseKey));
  }

}
