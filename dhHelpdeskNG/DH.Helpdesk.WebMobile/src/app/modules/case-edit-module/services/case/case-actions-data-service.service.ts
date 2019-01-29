import { CaseLogModel, CaseHistoryModel, LogFile, CaseHistoryChangeModel } from "../../models/case/case-actions-api.model";
import { CaseLogActionData, CaseAction, CaseEventType, CaseHistoryActionData } from "../../models";
import { Injectable } from "@angular/core";

@Injectable({ providedIn: 'root'})
export class CaseActionsDataService  {

  //processes case logs and history data into actions
  process(logs: CaseLogModel[], caseHistory: CaseHistoryModel): CaseAction<any>[] {
      
    let actions = new Array<CaseAction<any>>();
    
    //process logs
    if (logs && logs.length) {
      for (let log of logs) {
        const caseAction  = new CaseAction<CaseLogActionData>();
        //caseAction.id = log.id;
        caseAction.type = log.isExternal ? CaseEventType.ExternalLogNote : CaseEventType.InternalLogNote;
        caseAction.createdAt = log.createdAt;
        caseAction.createdBy= log.createdBy;
        caseAction.data = new CaseLogActionData(log.text, log.files);

        //add log action
        actions.push(caseAction);
      }
    }
     
    //process case history 
    if (caseHistory.changes && caseHistory.changes.length) {
        let changes = caseHistory.changes.sort((a, b) => b.id - a.id); //sort desc
        
        //create action for each field change
        for (const change of changes) {
            let caseAction  = new CaseAction<CaseHistoryActionData>();
            caseAction.type = this.resolveCaseEvent(change);
            caseAction.createdAt = change.createdAt;
            caseAction.createdBy = change.createdBy;
            caseAction.data = 
                new CaseHistoryActionData(
                    change.fieldName, 
                    change.fieldLabel, 
                    change.previousValue, 
                    change.currentValue);
            
            actions.push(caseAction);
        }
      }
    
      return actions;
  }  

  private resolveCaseEvent(change:CaseHistoryChangeModel) : CaseEventType {
    const prevValue = change.previousValue;
    const curValue = change.currentValue || '';
    const fieldName = change.fieldName;
    
    if (fieldName === 'FinishingDate' && curValue) {
      return CaseEventType.ClosedCase;
    }
    else if (fieldName === 'PerformerUserId') {
       //todo: working groups
       return CaseEventType.AssignedAdministrator;
    }
    else if (fieldName === 'WorkingGroupId') {
      return CaseEventType.AssignedWorkingGroup;
    }
    else if (fieldName === 'StateSecondaryId') {
      //todo: check field name
      return CaseEventType.ChangeSubstatus;
    }
    else if (fieldName === 'WatchDate') {
      return CaseEventType.ChangeWatchDate;
    }
    else if (fieldName === 'PriorityId') {
      return CaseEventType.ChangePriority;
    }       
    else {
      return CaseEventType.OtherChanges;
    }
    
    
    //todo:
    //SentEmails = 10,
    


      return CaseEventType.OtherChanges;
  }
}