using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.BusinessData.Models.Shared.Input;
using DH.Helpdesk.Common.Enums.BusinessRule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.BusinessRules
{
    public class BusinessRuleModel : INewBusinessModel
    {
        public BusinessRuleModel()
        {
            this.ProcessFrom = new SelectedItems();
            this.ProcessTo = new SelectedItems();
            this.SubStatusFrom = new SelectedItems();
            this.SubStatusTo = new SelectedItems();
            this.EmailGroups = new SelectedItems();
            this.WorkingGroups = new SelectedItems();
            this.Administrators = new SelectedItems();            
        }

        public int Id { get; set; }

        public int CustomerId { get; set; }        

        public string RuleName { get; set; }

        public int EventId { get; set; }

        public int RuleSequence { get; set; }

        public bool ContinueOnSuccess { get; set; }

        public bool ContinueOnError { get; set; }

        public bool RuleActive { get; set; }

        public DateTime CreatedTime { get; set; }

        public int CreatedByUserId { get; set; }

        public DateTime ChangedTime { get; set; }

        public int ChangedByUserId { get; set; }

        public SelectedItems ProcessFrom { get; set; }

        public SelectedItems ProcessTo { get; set; }

        public SelectedItems SubStatusFrom { get; set; }

        public SelectedItems SubStatusTo { get; set; }

        public string DomainFrom { get; set; }

        public string DomainTo { get; set; }

        public int EmailTemplate { get; set; }

        public SelectedItems EmailGroups { get; set; }

        public SelectedItems WorkingGroups { get; set; }

        public SelectedItems Administrators { get; set; }        

        public string[] Recipients { get; set; }

        public bool CaseCreator { get; set; }

        public bool Initiator { get; set; }

        public bool CaseIsAbout { get; set; }

        public bool DisableFinishingType { get; set; }
    }

    public class BusinessRuleActionModel
    {
        public BusinessRuleActionModel(int ruleId, int actionType)
        {
            RuleId = ruleId;
            ActionType = actionType;
            ActionParams = new List<BusinessRuleActionParamModel>();
        }

        public BusinessRuleActionModel(int ruleId, int actionType, List<BusinessRuleActionParamModel> actionParams)
        {
            RuleId = ruleId;
            ActionType = actionType;
            ActionParams = new List<BusinessRuleActionParamModel>();
        }

        public void AddActionParam(BusinessRuleActionParamModel actionParam)
        {
            this.ActionParams.Add(actionParam);
        }

        public int RuleId { get; private set; }

        public int ActionType { get; private set; }

        public List<BusinessRuleActionParamModel> ActionParams { get; private set; }
    }

    public class BusinessRuleActionParamModel
    {
        public BusinessRuleActionParamModel(int paramType, string paramValue)
        {            
            ParamType = paramType;
            ParamValue = paramValue;
        }
        
        public int ParamType { get; private set; }
        
        public string ParamValue { get; private set; }

    }
}
