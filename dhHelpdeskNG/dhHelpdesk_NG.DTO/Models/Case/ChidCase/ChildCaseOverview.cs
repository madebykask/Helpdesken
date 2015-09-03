﻿namespace DH.Helpdesk.BusinessData.Models.Case.ChidCase
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
    }
}
