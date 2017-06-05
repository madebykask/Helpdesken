using System;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class ExtendedCaseFormModel
    {
        public int CaseId { get; set; }
        public int Id { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        ExtendedCaseDataModel ExtendedCaseData { get; set; }
    }

    public class ExtendedCaseDataModel
    {
        public int Id { get; set; }
        public Guid ExtendedCaseGuid { get; set; }
        public int ExtendedCaseFormId { get; set; }
    }
}