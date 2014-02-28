namespace DH.Helpdesk.Web.Models.Case
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Problem.Output;
    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.Web.Models.Common;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Changes;

    public class CaseInputViewModel
    {
        public string CaseKey { get; set; }
        public string LogKey { get; set; }
        public string ParantPath_CaseType { get; set; }
        public string ParantPath_ProductArea { get; set; }
        public int DepartmentFilterFormat { get; set; }
        public int? CountryId { get; set; }
        public int ShowInvoiceFields { get; set; }
        public int CaseIsLockedByUserId { get; set; }
        public Case case_  { get; set; }
        public CaseLog CaseLog { get; set; }
        public SendToDialogModel SendToDialogModel { get; set; }
        public CaseMailSetting CaseMailSetting { get; set; }
        public User RegByUser { get; set; }
        public CustomerUser customerUserSetting { get; set; }
        public IList<CaseFieldSetting> caseFieldSettings { get; set; }
        public IList<CaseType> caseTypes { get; set; }
        public IList<StandardText> standardTexts { get; set; }
        public IList<Category> categories { get; set; }
        public IList<ChangeEntity> changes { get; set; }
        public IList<Country> countries { get; set; }
        public IList<Currency> currencies { get; set; }
        public IList<Department> departments { get; set; }
        public IList<FinishingCause> finishingCauses { get; set; }
        public IList<Impact> impacts { get; set; }
        public IList<ProblemOverview> problems { get; set; }
        public IList<ProductArea> productAreas { get; set; }
        public IList<Priority> priorities { get; set; }
        public IList<ProjectOverview> projects { get; set; }
        public IList<OU> ous { get; set; }  //unit
        public IList<Region> regions { get; set; }
        public IList<Status> statuses { get; set; }
        public IList<StateSecondary> stateSecondaries { get; set; }
        public IList<Supplier> suppliers { get; set; }
        public IList<Helpdesk.Domain.System> systems { get; set; }
        public IList<Urgency> urgencies { get; set; }
        public IList<User> users { get; set; }
        public IList<User> performers { get; set; }
        public IList<WorkingGroupEntity> workingGroups { get; set; }
        public IList<Log> Logs { get; set; }
        //public IList<CaseHistory> caseHistories { get; set; }
        public CaseHistory EmptyCaseHistory { get; set; }
        public FilesModel LogFilesModel { get; set; }
        public FilesModel CaseFilesModel { get; set; }
    }

    public class CaseIndexViewModel
    {
        public CaseSearchFilterData caseSearchFilterData { get; set; }
        public CaseSearchResultModel caseSearchResult { get; set; }
    }

    public class CaseSearchResultModel
    {
        public IList<CaseSettings> caseSettings { get; set; }
        public IList<CaseSearchResult> cases { get; set; }
    }

}