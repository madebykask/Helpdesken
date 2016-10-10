using DH.Helpdesk.Common.Enums.BusinessRule;
using DH.Helpdesk.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Areas.Admin.Models.BusinessRule
{
    public class BusinessRuleIndexModel
    {
        public BusinessRuleIndexModel()
        {
        }

        public Customer Customer { get; set; }

        public List<BusinessRuleListItemModel> Rules { get; set; }
    }
    
}