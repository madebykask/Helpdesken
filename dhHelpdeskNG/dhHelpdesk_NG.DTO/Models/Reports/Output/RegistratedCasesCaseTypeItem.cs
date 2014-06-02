namespace DH.Helpdesk.BusinessData.Models.Reports.Output
{
    using System;

    public sealed class RegistratedCasesCaseTypeItem
    {
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public int WorkingGroupId { get; set; }

        public string WorkingGroupName { get; set; }

        public int CaseTypeId { get; set; }

        public string CaseTypeName { get; set; }

        public int ProductAreadId { get; set; }

        public string ProductAreaName { get; set; }

        public int CaseId { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}