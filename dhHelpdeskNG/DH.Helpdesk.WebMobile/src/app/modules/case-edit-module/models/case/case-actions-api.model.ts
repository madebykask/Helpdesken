import { LogFileType } from 'src/app/modules/shared-module/constants/logFileType.enum';

export class CaseLogModel {
  id: number;
  text: string;
  isExternal: boolean;
  emailLogs: Array<EmailLog>;
  files: Array<LogFile>;
  mail2Tickets: Array<Mail2Ticket>;
  createdBy: string;
  createdAt: Date;
}

export class LogFile {
  id: number;
  logId: string;
  fileName: string;
  createdDate: Date;
  caseId?: number | null;
  logType: LogFileType;
}

export class EmailLog {
  id: number;
  mailId: number;
  email: string;
}

export class Mail2Ticket {
    id: number;
    email: string;
    subject: string;
}

export class CaseHistoryModel {
    //todo:
    emailLogs: any[];
    changes: CaseHistoryChangeModel[] = [];
}

export class CaseHistoryChangeModel {
  id: number;
  fieldName: string;
  fieldLabel: string;
  previousValue?: any;
  currentValue?: any;
  createdAt: Date;
  createdBy: string;
}

