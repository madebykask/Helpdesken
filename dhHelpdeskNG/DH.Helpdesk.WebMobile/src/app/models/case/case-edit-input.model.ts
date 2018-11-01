import { CaseSectionType } from "..";

export class CaseEditInputModel { 
    id: number;
    caseNumber: number;
    fields: IBaseCaseField<any>[];    
    caseSolution: CaseSolution;
    mailToTickets: MailToTicketInfo[];
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
}