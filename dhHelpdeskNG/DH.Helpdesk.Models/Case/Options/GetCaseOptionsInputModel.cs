namespace DH.Helpdesk.Models.Case.Options
{
    public class GetCaseOptionsInputModel
    {
        public int? CaseResponsibleUserId { get; set; }
        public int? CaseCausingPartId { get; set; }

        public bool CustomerRegistrationSources { get; set; }
        public bool Systems { get; set; }
        public bool Urgencies { get; set; }
        public bool Impacts { get; set; }
        public bool Suppliers { get; set; }
        public bool CausingParts { get; set; }
        public bool Currencies { get; set; }
        public bool ResponsibleUsers { get; set; }
        public bool Priorities { get; set; }
        public bool Statuses { get; set; }
        public bool SolutionsRates { get; set; }
        public bool Changes { get; set; }
        public bool Problems { get; set; }
        public bool Projects { get; set; }
    }
}
