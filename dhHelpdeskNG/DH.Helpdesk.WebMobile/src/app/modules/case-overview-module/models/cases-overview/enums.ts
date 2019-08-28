export enum CaseProgressFilter {
   None = -1,
   ClosedCases = 1,
   CasesInProgress = 2,
   CasesInRest = 3,
   UnreadCases = 4,
   FinishedNotApproved = 5,
   InProgressStatusGreater1 = 6,
   CasesWithWatchDate = 7,
   FollowUp = 8
}


export enum CaseStandardSearchFilters {
  AllCases = 0,
  MyCases = -1
}

export enum FilterFields {
  RegionFilter,
  DepartmentFilter,
  CaseTypeFilter,
  ProductAreaFilter,
  WorkingGroupFilter,
  ResponsibleFilter,
  AdministratorFilter,
  PriorityFilter,
  StatusFilter,
  SubStatusFilter,
  RemainingTimeFilter,
  ClosingReasonFilter,
  RegisteredByFilter,
  RegistrationDateFilter,
  WatchDateFilter,
  ClosingDateFilter,
  ProductArea
}

export enum InitiatorSearchScope {
  UserAndIsAbout = 0,
  User = 1,
  IsAbout = 2
}
