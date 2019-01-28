import { CaseEventType } from "./case-actions.model";

export class CaseLogModel {
  id: number;
  text: string;
  isExternal: boolean;
  regUserName: string;  
  emails: Array<string>  
  files: Array<LogFile> 
  createdBy: UserInfo;
  createdAt: Date;
}

export class UserInfo {
  id: number; 
  firstName:string;
  surName:string;  
}

export class LogFile {
  id: number; 
  logId:string;
  fileName:string;
  createdDate:Date;
  caseId?: number | null; 
}

//todo: review after api is ready
export class CaseHistoryModel {
  id: number;
  fieldName: string;
  type: CaseEventType;
  createdBy: UserInfo;
  createdAt: Date;
}