import { Component, OnInit, SimpleChanges } from '@angular/core';
import { CaseLogActionData } from 'src/app/modules/case-edit-module/models';
import { LogFile } from 'src/app/modules/case-edit-module/models/case/case-actions-api.model';
import { CaseActionBaseComponent } from '../case-action-base.component';
import { Router } from '@angular/router';
import { LogFileType } from 'src/app/modules/shared-module/constants/logFileType.enum';

@Component({
  selector: 'app-log-note-action',
  templateUrl: './log-note-action.component.html',
  styleUrls: ['./log-note-action.component.scss']
})
export class LogNoteActionComponent extends CaseActionBaseComponent<CaseLogActionData> implements OnInit {

  data: CaseLogActionData;
  logFileType = LogFileType;

  constructor(private router: Router) {
    super();
  }

  get hasM2T() {
    return this.data.mail2Tickets && this.data.mail2Tickets.length > 0;
  }

  ngOnInit(): void {
    this.data = this.caseAction != null ? this.caseAction.data : null;
  }

  downloadLogFile(file: LogFile) {
    this.router.navigate(['/case', this.caseKey, 'logfile', file.id], {
      queryParams: {
        fileName: file.fileName
      }
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    // note: is not called for dynamic components!
  }
}
