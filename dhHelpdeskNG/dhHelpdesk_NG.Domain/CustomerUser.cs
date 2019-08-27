namespace DH.Helpdesk.Domain
{
    using global::System;

    public class CustomerUser
    {
        public int Customer_Id { get; set; }
        public int? ShowOnStartPage { get; set; }
        public int User_Id { get; set; }
        public int WatchDatePermission { get; set; }
        public int UserInfoPermission { get; set; }
        public int CaptionPermission { get; set; }
        public int ContactBeforeActionPermission { get; set; }
        public int PriorityPermission { get; set; }
        public int StateSecondaryPermission { get; set; }
        public string CaseCategoryFilter { get; set; }
        public string CasePerformerFilter { get; set; }
        public string CaseProductAreaFilter { get; set; }
        public string CaseRegionFilter { get; set; }

        /// <summary>
        /// List of departments speartated by ','
        /// </summary>
        public string CaseDepartmentFilter { get; set; }

        public string CaseRemainingTimeFilter { get; set; }

        public string CaseStateSecondaryFilter { get; set; }

        /// <summary>
        /// Filter cases by "registred by" field
        /// List of user ids sparated by ','.
        /// </summary>
        public string CaseUserFilter { get; set; }
        public string CaseWorkingGroupFilter { get; set; }
        public string CaseCaseTypeFilter { get; set; }
        public string CasePriorityFilter { get; set; }
        public string CaseStatusFilter { get; set; }
        public string CaseResponsibleFilter { get; set; }

        public DateTime? CaseRegistrationDateStartFilter { get; set; }

        public DateTime? CaseRegistrationDateEndFilter { get; set; }

        public DateTime? CaseWatchDateStartFilter { get; set; }

        public DateTime? CaseWatchDateEndFilter { get; set; }

        public DateTime? CaseClosingDateStartFilter { get; set; }

        public DateTime? CaseClosingDateEndFilter { get; set; }
                
        public bool CaseRegistrationDateFilterShow { get; set; }
                
        public bool CaseWatchDateFilterShow { get; set; }

        public bool CaseClosingDateFilterShow { get; set; }

        public string CaseClosingReasonFilter { get; set; }

        /// <summary>
        /// Flag to display "filter by intitator" field on case overview page
        /// </summary>
        public bool CaseInitiatorFilterShow { get; set; }

        public bool RestrictedCasePermission { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual User User { get; set; }
    }
}
