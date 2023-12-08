using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Areas.Admin.Models.BusinessRule
{
    public class BusinessRuleListItemModel
    {
        public int RuleId { get; set; }

        public string RuleName { get; set; }

        public string Event { get; set; }

        public bool IsActive { get; set; }

        public string Condition { get; set; }

        public string Action { get; set; }

        public string ChangedBy { get; set; }

        public DateTime ChangedOn { get; set; }

        public int Sequence { get; set; }
    }
}