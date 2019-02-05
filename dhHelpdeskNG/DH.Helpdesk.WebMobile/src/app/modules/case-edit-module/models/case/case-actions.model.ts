import { LogFile } from "./case-actions-api.model";
import { UuidGenerator } from "src/app/helpers";

// Case Action Data classes
export type CaseActionDataType = CaseHistoryActionData | CaseLogActionData | GenericActionData;

// Case Action Group
export class CaseActionsGroup {
    id:string;
    constructor(public createdBy: string,
                public createdAt: Date) {
      this.id = UuidGenerator.createUuid();
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
    constructor() {
      this.id = UuidGenerator.createUuid();
    }
    id: string;    
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
    public prevValue: any,
    public currentValue: any) {
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