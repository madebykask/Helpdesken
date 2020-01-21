export class CaseOverviewItem {
    constructor() {
    }

    id: number;
    caseIcon: number;
    sortOrder: string;
    secSortOrder: string;
    tooltip: string;
    isUnread: boolean;
    isUrgent: boolean;
    showCustomerName: boolean;
    columns: CaseOverviewColumn[];
}

export class CaseOverviewColumn {
    key: string;
    stringValue: string;
    dateTimeValue: Date;
    FieldType: number;
    translateThis: boolean;
    treeTranslation: boolean;
    id: number;
}
