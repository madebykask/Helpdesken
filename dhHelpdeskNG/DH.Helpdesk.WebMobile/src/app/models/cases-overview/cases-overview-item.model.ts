export class CaseOverviewItem {
    constructor() {
    }

    Id: number;
    CaseIcon: number;
    SortOrder: string;
    SecSortOrder: string;
    Tooltip: string;
    IsUnread: false;
    IsUrgent: boolean;
    Columns: CaseOverviewColumn[]
}

export class CaseOverviewColumn {
    Key: string;
    StringValue: string;
    DateTimeValue: Date;
    FieldType: number;
    TranslateThis: boolean;
    TreeTranslation: boolean;
    Id: number;
}
