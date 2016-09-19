using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Web.Areas.Admin.Models.BusinessRules
{
    public class BusinessRulesViewModel
    {
        public Customer Customer { get; set; }
        public IList<BusinessRuleItemViewModel> BusinessRules { get; set; }
    }
}