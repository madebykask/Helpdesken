namespace ECT.Model.Entities.Reports
{
    using System;

    public class HelpDeskFieldData
    {
        public string CaseNumber { get; set; }

        public string EmployeeNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DateOfBirth { get; set; }

        public string SubProcess { get; set; }

        public string MainProcess { get; set; }   
  
        public string Company { get; set; }

        public string Unit { get; set; }

        public string Department { get; set; }

        public string Function { get; set; }

        public string Status { get; set; }

        public string NumberOFRecords { get; set; }

        public string WorkingGroup { get; set; }   
     
        public string AdminName { get; set; }

        public string AdminLastName { get; set; }

        public string SLA { get; set; }

        public string RegisteredByUser { get; set; }

        public string Eyequality { get; set; }

        public DateTime RegisterTime { get; set; }

        public DateTime FinishingDate { get; set; }

        public DateTime EffectiveDate { get; set; }
    }
}
