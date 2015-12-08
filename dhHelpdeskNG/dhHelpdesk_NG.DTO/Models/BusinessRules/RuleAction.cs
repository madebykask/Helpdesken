using DH.Helpdesk.BusinessData.Enums.BusinessRules;
using DH.Helpdesk.BusinessData.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.BusinessRules
{
    public class RuleAction
    {
        public RuleAction()
        {
           
        }

        public int Id { get; set; }

        public Rule Rule { get; set; }

        public int OrderNum { get; set; }

        public RuleActionType ActionType { get; set; }

        public RuleParameter Parameters { get; set; }
        
    }
}
