using DH.Helpdesk.BusinessData.OldComponents;

namespace DH.Helpdesk.Web.Models.Problem
{
    using System;

    public class CaseOutputModel
    {
        public int Id { get; set; }

        public string CaseNumber { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string Caption { get; set; }

        public string SubState { get; set; }

        public string CaseType { get; set; }

        public DateTime? WatchDate { get; set; }

        public GlobalEnums.CaseIcon CaseIcon { get; set; }
    }
}