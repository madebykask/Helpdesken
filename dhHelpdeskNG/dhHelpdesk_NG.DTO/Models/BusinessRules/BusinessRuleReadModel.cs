using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.Common.Enums.BusinessRule;
using DH.Helpdesk.Common.Types;
using DH.Helpdesk.Domain.BusinessRules;

namespace DH.Helpdesk.BusinessData.Models.BusinessRules
{
    public class BusinessRuleReadModel
    {
        public BusinessRuleReadModel()
        {
            this.Conditions = new List<string>();
            this.Actions = new List<string>();
        }

        public int Id { get; set; }

        public int CustomerId { get; set; }        

        public string RuleName { get; set; }

        public BREventType Event { get; set; }

        public int RuleSequence { get; set; }

        public bool ContinueOnSuccess { get; set; }

        public bool ContinueOnError { get; set; }

        public bool RuleActive { get; set; }

        public DateTime CreatedTime { get; set; }

        public UserName CreatedBy { get; set; }

        public DateTime ChangedTime { get; set; }

        public UserName ChangedBy { get; set; }

        public List<string> Conditions { get; set; }

        public List<string> Actions { get; set; }
    }
}
