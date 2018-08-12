using System;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.SelfService.Models.Case
{
    public class ClosedCaseAlertModel
    {
        public DateTime? FinishingDate { get; set; }
        public Setting CustomerSettings { get; set; }
    }
}