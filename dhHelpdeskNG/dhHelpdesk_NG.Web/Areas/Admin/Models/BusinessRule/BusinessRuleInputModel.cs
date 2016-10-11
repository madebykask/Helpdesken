using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Common.Enums.BusinessRule;
using DH.Helpdesk.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Areas.Admin.Models.BusinessRule
{
    public class DdlModel : SelectListItem //TODO: URGENT FIX, REMOVE ASAP - rework view to do not use selectlist items for both option list and selectd list, use ordinal viewmodels instead so active field will be accessible in a view
    {
        public bool Disabled { get; set; }
    }

    public class BusinessRuleInputModel
    {
        public BusinessRuleInputModel()
        {
        }

        public int RuleId { get; set; }

        public int CustomerId { get; set; }

        [Required]
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

        public List<DdlModel> Process { get; set; }

        public List<DdlModel> ProcessFromValue { get; set; }

        public List<DdlModel> ProcessToValue { get; set; }

        public List<DdlModel> SubStatus { get; set; }

        public List<DdlModel> SubStatusFromValue { get; set; }

        public List<DdlModel> SubStatusToValue { get; set; }
        
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

        [Required]
        public int? EmailTemplateId { get; set; }

        public List<DdlModel> EMailGroups { get; set; }

        public List<DdlModel> WorkingGroups { get; set; }

        public List<DdlModel> Administrators { get; set; }

        public string Recipients { get; set; }

        public bool CaseCreator { get; set; }

        public bool Initiator { get; set; }

        public bool CaseIsAbout { get; set; }

    }
}