import { LogFile, Mail2Ticket } from './case-actions-api.model';
import { UuidGenerator } from 'src/app/modules/shared-module/utils/uuid-generator';
import { CaseEventType } from '../../constants/case-event-type';

// Case Action Data classes
export type CaseActionDataType = CaseHistoryActionData | CaseLogActionData | GenericActionData;

// Case Action Group
export class CaseActionsGroup {
    id: string;
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
    private _id: string;

    constructor() {
      this._id = UuidGenerator.createUuid();
    }

    get id(): string {
      return this._id;
    }

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
  constructor(public text: string,
    public emails: string[],
    public files?: LogFile[],
    public mail2Tickets?: Mail2Ticket[]) {
  }
}

// Generic Action data
export class GenericActionData {
  constructor(public text: string, public action?: string) {
  }
}

