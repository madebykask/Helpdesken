using DH.Helpdesk.BusinessData.Enums.BusinessRules;
using DH.Helpdesk.BusinessData.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.BusinessRules
{
    public class RuleCondition
    {
        public RuleCondition()
        {

        }

        public int Id { get; set; }

        public Rule Rule { get; set; }

        public RuleField FieldId { get; set; }

        public CustomSelectList SourceValues { get; set; }

        public CustomSelectList TargetValues { get; set; }

    }
}
