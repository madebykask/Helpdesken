using System;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class ExtendedCaseDataOverview
    {
        public int CaseId { get; set; }
        public int ExtendedCaseFormId { get; set; }
        public int SectionType { get; set; }
        public string ExtendedCaseFormName { get; set; }
        public Guid ExtendedCaseGuid { get; set; }
        public int StateSecondaryId { get; set; }
        public int Version { get; set; }
    }
}