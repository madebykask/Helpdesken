import { CaseLogModel, CaseHistoryModel } from "../../models/case/case-events.model";
import { CaseLogActionData, CaseAction, CaseEventType } from "../../models";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root'})
export class CaseActionsDataService  {

  //processes case logs and history data into actions
  process(logs: CaseLogModel[], caseHistoryEvents: CaseHistoryModel[]): CaseAction<any>[] {
      
      let actions = new Array<CaseAction<any>>();
      
      //processlogs
      for (let log of logs) {
        const caseAction  = new CaseAction<CaseLogActionData>();        
        caseAction.Id = log.id;
        caseAction.Type = log.isExternal ? CaseEventType.ExternalLogNote : CaseEventType.InternalLogNote;
        caseAction.CreatedAt = log.createdAt;
        caseAction.CreatedByUserId = log.createdBy.id;
        caseAction.CreatedByUserName = log.createdBy.firstName + ' ' + log.createdBy.surName;
        caseAction.Data = new CaseLogActionData(log.text, log.files);

        //add log action
        actions.push(caseAction);
      }

      return actions;
  }
}