namespace DH.Helpdesk.Mobile.Models.Case
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Domain;

    public class CaseSearchFilterData
    {
        public CustomerUser customerUserSetting { get; set; }
        public Setting customerSetting { get; set; }
        public Customer customer { get; set; }
        public CaseSearchFilter caseSearchFilter { get; set; }
        public int filterCustomerId { get; set; }
        public IList<Country> filterCountry { get; set; }
        public IList<Region> filterRegion { get; set; }
        public IList<Department> filterDepartment { get; set; }
        public IList<User> filterUser { get; set; }
        public IList<User> filterPerformer { get; set; }
        public IList<UserLists> filterCaseUser { get; set; }
        public IList<CaseType> filterCaseType { get; set; }
        public IList<ProductArea> filterProductArea { get; set; }
        public IList<Category> filterCategory { get; set; }
        public IList<WorkingGroupEntity> filterWorkingGroup { get; set; }
        public IList<Priority> filterPriority { get; set; }
        public IList<Status> filterStatus { get; set; }
        public IList<StateSecondary> filterStateSecondary { get; set; }
        public IList<Field> filterCaseProgress { get; set; }

        public DateTime? CaseRegistrationDateStartFilter { get; set; }

        public DateTime? CaseRegistrationDateEndFilter { get; set; }

        public DateTime? CaseWatchDateStartFilter { get; set; }

        public DateTime? CaseWatchDateEndFilter { get; set; }

        public DateTime? CaseClosingDateStartFilter { get; set; }

        public DateTime? CaseClosingDateEndFilter { get; set; }

        public IList<FinishingCause> ClosingReasons { get; set; }

        public bool IsClearFilters { get; set; }
    }    
}
