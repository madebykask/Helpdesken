
export interface IActionData {
  getTitle(): string;
}

export class CaseActionsGroup {
    constructor(public CreatedByUserId: number,
                public CreatedByUserName: string,
                public CreatedAt: Date) {
    } 

    Actions: Array<CaseAction<any>>;
}

export class CaseAction<TData extends CaseHistoryActionData | CaseLogActionData | GenericActionData> {
    Id: number;
    CreatedAt: Date;
    Type: CaseActionEvents; // todo: introduce enum ?
    CreatedByUserId: number
    CreatedByUserName: string;
    Data: TData;    
}

///////////////////////////////////////////////////
// Case History
export class CaseHistoryActionData implements IActionData{
  constructor(private fieldName:string, 
              private prevValue: string, 
              private newValue: string) {

  }
  
  getTitle(): string {
      //todo: construct title out of fields
      return `Field <b>'${this.fieldName}'</b> changed`;
  }
  
  // getText() ?
}

///////////////////////////////////////////////////
// Case Log Note
export class CaseLogActionData implements IActionData {

  constructor(private text:string) {
  }
  
  getTitle(): string {
      //todo: trim text ?
      return this.text;
  }

  // getText() ?
}

///////////////////////////////////////////////////
// Generic Action data
export class GenericActionData implements IActionData{
  
  constructor(private title: string) {
  }

  getTitle(): string {
    return this.title;
  }
}

export enum CaseActionEvents {
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