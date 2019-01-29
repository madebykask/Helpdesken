import { Component, OnInit, Input } from '@angular/core';
import { CaseActionBaseComponent } from '../case-action-base.component';
import { CaseLogActionData } from 'src/app/modules/case-edit-module/models';

@Component({
  selector: 'app-log-note-action',
  templateUrl: './log-note-action.component.html',
  styleUrls: ['./log-note-action.component.scss']
})
export class LogNoteActionComponent extends CaseActionBaseComponent<CaseLogActionData> implements OnInit {

  constructor() { 
    super();
  }

  ngOnInit() {
  }

  get data(): CaseLogActionData {
    return this.caseAction != null ? this.caseAction.data : null;
  }  
}
