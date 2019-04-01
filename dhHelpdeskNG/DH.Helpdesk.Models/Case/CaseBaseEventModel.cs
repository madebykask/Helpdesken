using System;
using DH.Helpdesk.BusinessData.Models.Logs.Output;

namespace DH.Helpdesk.Models.Case
{
    public class CaseBaseEventModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public LogUserOverview CreatedBy { get; set; }
    }

 }
