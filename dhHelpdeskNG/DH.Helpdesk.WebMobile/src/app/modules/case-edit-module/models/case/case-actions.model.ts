
export class CaseActionsGroup {
    constructor(public CreatedByUserId: number,
                public CreatedByUserName: string,
                public CreatedAt: Date) {
    } 

    Actions: Array<CaseAction<any>>;
}

export class CaseAction<TData extends CaseHistoryActionData | CaseLogActionData | GenericActionData> {
    Id: number;
    Type: CaseEventType;
    CreatedAt: Date;    
    CreatedByUserId: number
    CreatedByUserName: string;    
    Data: TData;
}

///////////////////////////////////////////////////
// Case Action Data classes
export type CaseActionDataType = CaseHistoryActionData | CaseLogActionData | GenericActionData;

// Case History
export class CaseHistoryActionData {
  constructor(public fieldName:string,
              public prevValue: string, 
              public newValue: string) {
  }
}

// Case Log Note
export class CaseLogActionData {
  constructor(public text:string) {
  } 
}

// Generic Action data
export class GenericActionData {
  constructor(public text?:string, 
              public action?:string) {
  }
}
///////////////////////////////////////////////////

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