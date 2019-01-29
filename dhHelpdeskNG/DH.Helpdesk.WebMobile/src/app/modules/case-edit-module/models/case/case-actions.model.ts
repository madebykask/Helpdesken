import { LogFile } from "./case-actions-api.model";

// Case Action Data classes
export type CaseActionDataType = CaseHistoryActionData | CaseLogActionData | GenericActionData;

// Case Action Group
export class CaseActionsGroup {
    constructor(public createdBy: string,
                public createdAt: Date) {
    } 

    Actions: Array<CaseAction<CaseActionDataType>>;

    get hasOther() {
      if (this.Actions && this.Actions.length) {
        return this.Actions.some(x => x.type === CaseEventType.OtherChanges);
      }
      return false;
    }

    get hasMain() {
      if (this.Actions && this.Actions.length) {
        return this.Actions.some(x => x.type !== CaseEventType.OtherChanges);
      }
      return false;
    }
}

// Case Action
export class CaseAction<TData extends CaseActionDataType> {
    //id: number;    
    type: CaseEventType;
    createdAt: Date;
    createdBy: string;    
    data: TData;
}

// Case History
export class CaseHistoryActionData {
  constructor(
    public fieldName: string, 
    public fieldLabel: string,
    public prevValue: string,
    public currentValue: string) {
  }
}

// Case Log Note Data
export class CaseLogActionData {
  constructor(public text:string, public files?: LogFile[]) {
  } 
}

// Generic Action data
export class GenericActionData {
  constructor(public text:string, public action?:string) {
  }
}

// CaseEventType enum
export enum CaseEventType {
  ExternalLogNote = 1,
  InternalLogNote = 2,
  ClosedCase = 3,
  AssignedAdministrator = 4,
  AssignedWorkingGroup = 5, 
  ChangeSubstatus = 6,
  ChangeWatchDate = 7,
  ChangePriority = 8,
  UploadLogFile = 9,
  SentEmails = 10,
  OtherChanges = 11 
}