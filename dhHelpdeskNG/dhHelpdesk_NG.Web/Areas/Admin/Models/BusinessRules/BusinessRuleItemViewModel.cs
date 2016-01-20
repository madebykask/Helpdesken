using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Areas.Admin.Models.BusinessRules
{
    public class BusinessRuleItemViewModel
    {
        public int Id { get; set; }
        public string RuleName { get; set; }
        public string EventName { get; set; }
        public string SubjectName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByName { get; set; }
    }
}