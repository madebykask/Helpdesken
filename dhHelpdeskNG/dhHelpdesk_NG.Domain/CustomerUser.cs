namespace DH.Helpdesk.Domain
{
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
        public string CaseStateSecondaryFilter { get; set; }
        public string CaseUserFilter { get; set; }
        public string CaseWorkingGroupFilter { get; set; }
        public string CaseCaseTypeFilter { get; set; }
        public string CasePriorityFilter { get; set; }
        public string CaseStatusFilter { get; set; }
        public string CaseResponsibleFilter { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual User User { get; set; }

    }
}
