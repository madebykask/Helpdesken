namespace DH.Helpdesk.BusinessData.Models.Case.ChidCase
{
    using System;

    public class ChildCaseOverview
    {
        public int Id { get; set; }

        public int CaseNo { get; set; }

        public string Subject { get; set; }

        public string CaseType { get; set; }

        public UserNamesStruct CasePerformer { get; set; }
        
        public string SubStatus { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime? ClosingDate { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public string Priority { get; set; }

        public bool IsRequriedToApprive { get; set; }

        public int ParentId { get; set; }

        public bool RelationType { get; set; }

        public bool Indepandent { get; set; }

        // used for linq queries to avoid type casting in sql
        public decimal CaseNoDecimal
        {
            get { return Convert.ToDecimal(CaseNo); }
            set { CaseNo = Convert.ToInt32(value); }
        }
    }
}
