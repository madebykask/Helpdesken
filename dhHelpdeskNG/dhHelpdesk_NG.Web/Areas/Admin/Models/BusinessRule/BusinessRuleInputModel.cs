using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DH.Helpdesk.Web.AppCode.Attributes;
using DH.Helpdesk.Web.AppCode.Constants;

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
			RuleName = string.Empty;
			ContinueOnSuccess = true;
			ContinueOnError = true;
			IsActive = true;
			Events = new List<BREvent>();
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
			ProcessesFromValue = new List<string>();
			ProcessesToValue = new List<string>();
			SubStatusesFromValue = new List<string>();
			SubStatusesToValue = new List<string>();
		}

		public int Id { get; set; }

		public int RuleId { get; set; }

		[RequiredIfNotEmpty("lstProcessTo", ErrorMessage = ErrorMessages.IsRequired)]
		public ICollection<string> ProcessesFromValue { get; set; }

		[RequiredIfNotEmpty("lstProcessFrom", ErrorMessage = ErrorMessages.IsRequired)]
		public ICollection<string> ProcessesToValue { get; set; }

		[RequiredIfNotEmpty("lstSubStatusTo", ErrorMessage = ErrorMessages.IsRequired)]
		public ICollection<string> SubStatusesFromValue { get; set; }

		[RequiredIfNotEmpty("lstSubStatusFrom", ErrorMessage = ErrorMessages.IsRequired)]
		public ICollection<string> SubStatusesToValue { get; set; }

		public int Sequence { get; set; }
	}

	public class BRActionModel
	{
		public BRActionModel()
		{
			Recipients = string.Empty;
			EMailGroupIds = new List<string>();
			WorkingGroupIds = new List<string>();
			AdministratorIds = new List<string>();
		}

		public int Id { get; set; }

		public int RuleId { get; set; }

		// Send Email
		public int ActionTypeId { get; set; }

		public int Sequence { get; set; }

		[Required]
		public int? EmailTemplateId { get; set; }

		public ICollection<string> EMailGroupIds { get; set; }

		public ICollection<string> WorkingGroupIds { get; set; }

		public ICollection<string> AdministratorIds { get; set; }

		public string Recipients { get; set; }

		public bool CaseCreator { get; set; }

		public bool Initiator { get; set; }

		public bool CaseIsAbout { get; set; }

	}
}