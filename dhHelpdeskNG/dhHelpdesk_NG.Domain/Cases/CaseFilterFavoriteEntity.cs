using System;

namespace DH.Helpdesk.Domain.Cases
{
    using global::System.Collections.Generic;
    

    public class CaseFilterFavoriteEntity : Entity
    {
        public int Customer_Id { get; set; }
        public int User_Id { get; set; }
        public string Name { get; set; }

        public string InitiatorFilter { get; set; }
        public string InitiatorSearchScopeFilter { get; set; }
		public string RegionFilter { get; set; }
		public string DepartmentFilter { get; set; }		
		public string CaseTypeFilter { get; set; }
        public string CategoryTypeFilter { get; set; }
        public string ProductAreaFilter { get; set; }
		public string WorkingGroupFilter { get; set; }
		public string ResponsibleFilter { get; set; }
		public string AdministratorFilter { get; set; }
		public string PriorityFilter { get; set; }
		public string StatusFilter { get; set; }
		public string SubStatusFilter { get; set; }
		public string RemainingTimeFilter { get; set; }
		public string ClosingReasonFilter { get; set; }
        public string RegisteredByFilter { get; set; }
        
		public DateTime? RegistrationDateStartFilter { get; set; }
		public DateTime? RegistrationDateEndFilter { get; set; }
		public DateTime? WatchDateStartFilter { get; set; }
		public DateTime? WatchDateEndFilter { get; set; }
		public DateTime? ClosingDateStartFilter { get; set; }
		public DateTime? ClosingDateEndFilter { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}