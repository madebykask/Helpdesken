namespace DH.Helpdesk.Domain
{
    using DH.Helpdesk.Domain.Cases;
    using DH.Helpdesk.Domain.Interfaces;
    using DH.Helpdesk.Domain.Problems;

    using global::System;
    using global::System.Collections.Generic;

    public class CaseIsAboutEntity : Entity
    {

        //public int Id { get; set; }
        public string ReportedBy { get; set; }
        public string Person_Name { get; set; }
        public string Person_Email { get; set; }
        public string Person_Phone { get; set; }
        public string Person_Cellphone { get; set; }
        public int? Region_Id { get; set; }
        public int? Department_Id { get; set; }
        public int? OU_Id { get; set; }
        public string CostCentre { get; set; }
        public string Place { get; set; }
        public string UserCode { get; set; }

        public virtual Case Case { get; set; }
       
    }
}
