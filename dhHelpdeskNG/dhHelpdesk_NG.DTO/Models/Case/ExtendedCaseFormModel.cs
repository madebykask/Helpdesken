using System;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class ExtendedCaseFormModel
    {
        public int CaseId { get; set; }
        public int Id { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public int UserRole { get; set; } //Case.Working group
        public int CaseStatus { get; set; }  //Case.StateSecondary
        public int LanguageId { get; set; }
        public Guid UserGuid { get; set; }
        public Guid ExtendedCaseGuid { get; set; }
    }

    public class ExtendedCaseDataModel
    {
        public int Id { get; set; }
        public Guid ExtendedCaseGuid { get; set; }
        public int ExtendedCaseFormId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}