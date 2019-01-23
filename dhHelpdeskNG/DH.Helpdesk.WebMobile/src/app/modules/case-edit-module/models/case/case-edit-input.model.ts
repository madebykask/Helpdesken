import { CaseFieldOptions } from "src/app/modules/shared-module/constants";
import { CaseSectionType } from "./case-section-input.model";

export class CaseEditInputModel {
    id: number;
    caseNumber: number;
    caseGuid: string;
    editMode: CaseAccessMode;
    fields: IBaseCaseField<any>[];
    caseSolution: CaseSolution;
    mailToTickets: MailToTicketInfo[];
}

export enum CaseAccessMode {
    NoAccess = 0,
    ReadOnly = 1,
    FullAccess = 2
}

export class CaseLockModel {
    isLocked: boolean;
    userId: number;
    lockGuid: string;
    browserSession: string;
    createdTime: Date;
    extendedTime: Date;
    extendValue: number;
    timerInterval: number;
    userFullName: string;
}

export class MailToTicketInfo{
    email: string;
    type: string;
    isHelpdesk: boolean;
}

export class CaseSolution {
    caseSolutionId:number;
    name: string;
    stateSecondaryId: number;
}

export class BaseCaseField<T> implements IBaseCaseField<T> {
    public name: string;
    public label: string;
    //public JsonType: string;
    public value?: T;
    public section?: CaseSectionType;
    public options: KeyValue[]

    public get isReadonly(): boolean {
      return this.options != null && this.options.filter((o) => o.key == CaseFieldOptions.readonly).length > 0;
    }

    public get isRequired(): boolean {
      return this.options != null && this.options.filter((o) => o.key == CaseFieldOptions.reqiured).length > 0;
    }
}

export class KeyValue {
    public key: string;
    public value: string;
}

export interface IBaseCaseField<T> {
    name: string;
    label: string;
    //JsonType: string;
    value?: T;
    section?: CaseSectionType;
    options: KeyValue[]

    isReadonly: boolean;
    isRequired: boolean;
}
