export class CaseSectionInputModel {
    constructor(public id: number,
       public header: string,
       public type: CaseSectionType,
       public isNewCollapsed: boolean,
       public isEditCollapsed: boolean,
       public caseSectionFields: Array<string>) {
    }
}

export enum CaseSectionType {
    Initiator = 0,
    Regarding = 1,
    ComputerInfo = 2,
    CaseInfo = 3,
    CaseManagement = 4,
    Communication = 5,
    Status = 6,
    Invoices = 7,
    Invoicing = 8,
    ExtendedCase = 9,
    CloseCase = 10
}
