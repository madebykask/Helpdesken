export class CaseSectionInputModel {
    id: number;
    header: string;
    type: CaseSectionType;
    isNewCollapsed: boolean;
    isEditCollapsed: boolean;

    constructor(id: number, header: string, type: CaseSectionType,
         isNewCollapsed: boolean, isEditCollapsed: boolean) {
            this.id = id;
            this.header = header;
            this.type = type;
            this.isEditCollapsed = isEditCollapsed;
            this.isNewCollapsed = isNewCollapsed;
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
    ExtendedCase = 9
}