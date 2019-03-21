export class BundleOptionsFilter {
    CaseResponsibleUserId?: number;
    CaseCausingPartId?: number;
    CustomerRegistrationSources?: boolean;
    Systems?: boolean;
    Urgencies?: boolean;
    Impacts?: boolean;
    Suppliers?: boolean;
    CausingParts?: boolean;
    Currencies?: boolean;
    ResponsibleUsers?: boolean;
    Priorities?: boolean;
    Statuses?: boolean;
    SolutionsRates?: boolean;
    Changes?: boolean;
    Problems?: boolean;
    Projects?: boolean;
 }

export class CaseOptionsFilterModel extends BundleOptionsFilter {
    RegionId?: any;
    CasePerformerUserId?: number;
    CaseWorkingGroupId?: number;
    DepartmentId?: any;
    OrgUnitId?: any;
    IsAboutRegionId?: any;
    IsAboutDepartmentId?: any;
    WorkingGroups?: boolean;
    Performers?: boolean;
    CaseTypes?: boolean;
    CaseTypeId?: any;
    ProductAreas?: boolean;
    ProductAreaId?: any;
    Categories?: boolean;
    ClosingReasons?: boolean;
    StateSecondaries?: boolean;

 }
 