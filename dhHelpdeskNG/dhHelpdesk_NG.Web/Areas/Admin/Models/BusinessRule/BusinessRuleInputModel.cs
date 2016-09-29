using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Common.Enums.BusinessRule;
using DH.Helpdesk.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Areas.Admin.Models.BusinessRule
{
    public class BusinessRuleInputModel
    {
        public BusinessRuleInputModel()
        {
        }

        public int RuleId { get; set; }

        public int CustomerId { get; set; }

        public string RuleName { get; set; }

        public List<BREvent> Events { get; set; }

        public int Sequence { get; set; }

        public bool ContinueOnSuccess { get; set; }

        public bool ContinueOnError { get; set; }

        public bool IsActive { get; set; }

        public BRConditionModel Condition { get; set; }

        public BRActionModel Action { get; set; }

        
    }

    public class BREvent
    {
        public BREvent(int id, string name, bool selected)
        {
            Id = id;
            Name = name;
            Selected = selected;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public bool Selected { get; private set; }
    }

    public class BRConditionModel
    {
        public BRConditionModel()
        {

        }

        public int Id { get; set; }

        public int RuleId { get; set; }

        public List<SelectListItem> Process { get; set; }

        public List<SelectListItem> ProcessFromValue { get; set; }

        public List<SelectListItem> ProcessToValue { get; set; }

        public List<SelectListItem> SubStatus { get; set; }

        public List<SelectListItem> SubStatusFromValue { get; set; }

        public List<SelectListItem> SubStatusToValue { get; set; }
        
        public int Sequence { get; set; }        
    }

    public class BRActionModel
    {
        public BRActionModel()
        {
        }

        public int Id { get; set; }

        public int RuleId { get; set; }

        // Send Email
        public int ActionTypeId { get; set; }

        public int Sequence { get; set; }

        public List<SelectListItem> EMailTemplates { get; set; }

        public List<SelectListItem> EMailGroups { get; set; }

        public List<SelectListItem> WorkingGroups { get; set; }

        public CustomSelectList Administrators { get; set; }

        public string Recipients { get; set; }

    }
}