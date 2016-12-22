using System;
using System.Collections.Generic;

namespace ECT.Model.Entities
{
    public class Contract
    {
        public int Id { get; set; }
        public string CaseNumber { get; set; }
        public string Initiator { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string InitiatorEMail { get; set; }
        public string HelpdeskEMail { get; set; }
        public DateTime HireDate { get; set; }
        public Option Company { get; set; }
        public Option Unit { get; set; }
        public int StateSecondaryId { get; set; }
        public string StateSecondary { get; set; }
        public string AlternativeStateSecondaryName { get; set; }
        public int WorkingGroupId { get; set; }
        public int ProductAreaId { get; set; }
        public string PriorityName { get; set; }
        public int CustomerId { get; set; }
        public int LanguageId { get; set; }
        public IEnumerable<CaseFile> CaseFiles { get; set; }
        public int DateRange { get; set; }
        public IEnumerable<CaseHistory> CaseHistory { get; set; }
        public string EmployeeNumber { get; set; }
        public IEnumerable<int> ChildCaseNumbers { get; set; }
        public int ParentCaseNumber { get; set; }
        public DateTime? FinishingDate { get; set; }
        public DateTime? WatchDate { get; set; }
        public DateTime ChangeTime { get; set; }
        public DateTime RegTime { get; set; }
        public DateTime? LatestSLACountDate { get; set; }
        public int HolidayHeader_Id { get; set; }
        public int SolutionTime { get; set; }
        public int ExternalTime { get; set; }
        public string BaseUrl { get; set; }
    }
}