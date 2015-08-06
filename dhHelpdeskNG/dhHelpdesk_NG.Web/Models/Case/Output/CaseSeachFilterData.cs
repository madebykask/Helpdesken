namespace DH.Helpdesk.Web.Models.Case.Output
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Domain;

    public class CaseSearchFilterData
    {
        public CustomerUser customerUserSetting { get; set; }
        public Setting customerSetting { get; set; }
        public Customer customer { get; set; }
        public CaseSearchFilter caseSearchFilter { get; set; }
        public int filterCustomerId { get; set; }
        public IList<Region> filterRegion { get; set; }
        public IList<Department> filterDepartment { get; set; }

        public IList<CaseType> filterCaseType { get; set; }
        public IList<ProductArea> filterProductArea { get; set; }
        public IList<Category> filterCategory { get; set; }
        public IList<WorkingGroupEntity> filterWorkingGroup { get; set; }
        public IList<Priority> filterPriority { get; set; }
        public IList<Status> filterStatus { get; set; }
        public IList<StateSecondary> filterStateSecondary { get; set; }
        public IList<Field> filterCaseProgress { get; set; }
        public IList<ItemOverview> filterCustomers { get; set; }
        public IList<ItemOverview> filterMaxRows { get; set; }

        public bool SearchInMyCasesOnly { get; set; }

        public DateTime? CaseRegistrationDateStartFilter { get; set; }

        public DateTime? CaseRegistrationDateEndFilter { get; set; }

        public DateTime? CaseWatchDateStartFilter { get; set; }

        public DateTime? CaseWatchDateEndFilter { get; set; }

        public DateTime? CaseClosingDateStartFilter { get; set; }

        public DateTime? CaseClosingDateEndFilter { get; set; }

        public IList<FinishingCause> ClosingReasons { get; set; }

        /// <summary>
        /// Preset string in "filter by intitator" filter field on case overview page
        /// </summary>
        public string CaseInitiatorFilter { get; set; }
        

        public bool IsAdvancedSearch { get; set; }

        public string CaseNumberFilter { get; set; }

        /// <summary>
        /// Available users for "registered by" search field
        /// </summary>
        public SelectList RegisteredByUserList { get; set; }

        /// <summary>
        /// Selected user ids for "registered by" search field
        /// </summary>
        public int[] lstfilterUser { get; set; }

        /// <summary>
        /// Available users for "responsible" search field
        /// </summary>
        public SelectList ResponsibleUserList { get; set; }

        /// <summary>
        /// Selected user ids for "Responsible" search field
        /// </summary>
        public int[] lstfilterResponsible { get; set; }

        /// <summary>
        /// List of available performers for "Administrator" case field in search form
        /// </summary>
        public SelectList AvailablePerformersList { get; set; }

        /// <summary>
        /// List of selected performers for "Administrator" case field in search form
        /// </summary>
        public int[] lstfilterPerformer { get; set; }
    }

    public class AdvancedSearchSpecificFilterData
    {
        public AdvancedSearchSpecificFilterData()
        {
        }
        
        public int CustomerId { get; set; }

        public Setting CustomerSetting { get; set; }

        public IList<Department> DepartmentList { get; set; }

        public string FilteredDepartment { get; set; }

        public IList<CaseType> CaseTypeList { get; set; }

        public string FilteredCaseType { get; set; }

        public IList<ProductArea> ProductAreaList { get; set; }

        public string FilteredProductArea { get; set; }

        public IList<Priority> PriorityList { get; set; }

        public string FilteredPriority { get; set; }

        public IList<StateSecondary> StateSecondaryList { get; set; }

        public string FilteredStateSecondary { get; set; }

        public IList<FinishingCause> ClosingReasonList { get; set; }

        public string FilteredClosingReason { get; set; }
    }
}
