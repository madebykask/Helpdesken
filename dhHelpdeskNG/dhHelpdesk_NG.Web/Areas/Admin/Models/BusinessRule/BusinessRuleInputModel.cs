using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DH.Helpdesk.Web.Infrastructure.Attributes;
using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

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
            StatusesFromValue = new List<string>();
            StatusesToValue = new List<string>();
            SubStatusesFromValue = new List<string>();
			SubStatusesToValue = new List<string>();
		}

		public int Id { get; set; }

		public int RuleId { get; set; }

		[LocalizedDisplay("Processen från")]
		[RequiredIfNotEmpty("ProcessesToValue", "lstProcessTo")]
		public ICollection<string> ProcessesFromValue { get; set; }

		[LocalizedDisplay("Processen till")]
		[RequiredIfNotEmpty("ProcessesFromValue", "lstProcessFrom")]
		public ICollection<string> ProcessesToValue { get; set; }

        [LocalizedDisplay("Status från")]
        [RequiredIfNotEmpty("StatusesToValue", "SubStatusTo")]
        public ICollection<string> StatusesFromValue { get; set; }

        [LocalizedDisplay("Status till")]
        [RequiredIfNotEmpty("StatusesFromValue", "SubStatusFrom")]
        public ICollection<string> StatusesToValue { get; set; }

        [LocalizedDisplay("Sub status från")]
		[RequiredIfNotEmpty("SubStatusesToValue", "lstSubStatusTo")]
		public ICollection<string> SubStatusesFromValue { get; set; }

		[LocalizedDisplay("Sub status till")]
		[RequiredIfNotEmpty("SubStatusesFromValue", "lstSubStatusFrom")]
		public ICollection<string> SubStatusesToValue { get; set; }

        //Domain
        [LocalizedDisplay("Domän till")]
        [RequiredIfNotEmpty("DomainToValue", "lstDomainTo")]
        public string DomainFromValue { get; set; }

        [LocalizedDisplay("Domän till")]
        [RequiredIfNotEmpty("DomainFromValue", "lstDomainFrom")]
        public string DomainToValue { get; set; }
		//End of domain

		public string Equals { get; set; }

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

        public int? Administrator { get; set; }

        public string Recipients { get; set; }

		public bool CaseCreator { get; set; }

		public bool Initiator { get; set; }

		public bool CaseIsAbout { get; set; }

		public bool DisableFinishingType { get; set; }

	}
}