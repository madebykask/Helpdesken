import { Component, OnInit, SimpleChanges } from '@angular/core';
import { CaseLogActionData } from 'src/app/modules/case-edit-module/models';
import { LogFile } from 'src/app/modules/case-edit-module/models/case/case-actions-api.model';
import { CaseActionBaseComponent } from '../case-action-base.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-log-note-action',
  templateUrl: './log-note-action.component.html',
  styleUrls: ['./log-note-action.component.scss']
})
export class LogNoteActionComponent extends CaseActionBaseComponent<CaseLogActionData> {

  constructor(private router: Router) {
    super();
  } 

  get data(): CaseLogActionData {
    return this.caseAction != null ? this.caseAction.data : null;
  }  

   downloadLogFile(file: LogFile) {
     this.router.navigate(['/case', this.caseKey, 'logfile', file.id]);
  }
  
  ngOnChanges(changes: SimpleChanges): void {
    // note: is not called for dynamic components!
  }
}
