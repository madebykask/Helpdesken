export class BundleOptionsFilter {
    CaseResponsibleUserId?: number;
    CasePerformerUserId?: number;
    CaseWorkingGroupId?:number;
    CaseCausingPartId?: number;
    CustomerRegistrationSources: boolean;
    Systems: boolean;
    Urgencies: boolean;
    Impacts: boolean;
    Suppliers: boolean;
    CausingParts: boolean;
    Currencies: boolean;
    WorkingGroups: boolean;
    ResponsibleUsers: boolean;
    Performers: boolean;
    Priorities: boolean;
    Statuses: boolean;
    StateSecondaries: boolean;
    SolutionsRates: boolean;
    Changes: boolean;
    Problems: boolean;
    Projects: boolean;
 }

export class CaseOptionsFilterModel extends BundleOptionsFilter {
    RegionId?: any;
    DepartmentId?: any;
    IsAboutRegionId?: any;
    IsAboutDepartmentId?: any;
    CaseTypes: boolean;
    CaseTypeId?: any;
    ProductAreas: boolean;
    ProductAreaId?: any;
    Categories: boolean;
    ClosingReasons: boolean;
 }
 