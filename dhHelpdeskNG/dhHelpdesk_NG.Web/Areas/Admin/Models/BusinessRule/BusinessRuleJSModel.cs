using DH.Helpdesk.BusinessData.Models.BusinessRules;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DH.Helpdesk.Web.Infrastructure.Attributes;
using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;
using DH.Helpdesk.Common.Enums.BusinessRule;

namespace DH.Helpdesk.Web.Areas.Admin.Models.BusinessRule
{
    public class BusinessRuleJSModel
    {
        public BusinessRuleJSModel()
        {
            
        }           

        public string CustomerId { get; set; }

        public string RuleId { get; set; }        

        [Required]
        public string RuleName { get; set; }

        public string EventId { get; set; }

        public string RuleSequence { get; set; }

        public string ContinueOnSuccess { get; set; }

        public string ContinueOnError { get; set; }

        public string RuleActive { get; set; }

		[LocalizedDisplay("Processen från")]
		[RequiredIfNotEmpty("ProcessTo")]
		public string ProcessFrom { get; set; }

		[LocalizedDisplay("Processen till")]
		[RequiredIfNotEmpty("ProcessFrom")]
		public string ProcessTo { get; set; }

		[LocalizedDisplay("Sub status från")]
		[RequiredIfNotEmpty("SubStatusTo")]
		public string SubStatusFrom { get; set; }

		[LocalizedDisplay("Sub status till")]
		[RequiredIfNotEmpty("SubStatusFrom")]
		public string SubStatusTo { get; set; }

        [LocalizedDisplay("Sub status från")]
        [RequiredIfNotEmpty("SubStatusTo2")]
        public string SubStatusFrom2 { get; set; }

        [LocalizedDisplay("Sub status till")]
        [RequiredIfNotEmpty("SubStatusFrom2")]
        public string SubStatusTo2 { get; set; }

        [LocalizedDisplay("Domän från")]
        [RequiredIfNotEmpty("DomainTo")]
        public string DomainFrom { get; set; }

        [LocalizedDisplay("Domän till")]
        [RequiredIfNotEmpty("DomainFrom")]
        public string DomainTo { get; set; }

        public string EmailTemplate { get; set; }

        public string EmailGroups { get; set; }

        public string WorkingGroups { get; set; }

        public string Administrator { get; set; }

        public string Administrators { get; set; }

        public string Recipients { get; set; }

        public string CaseCreator { get; set; }

        public string Initiator { get; set; }

        public string CaseIsAbout { get; set; }

        public string Equals { get; set; }

        public string DisableFinishingType { get; set; }
    }

   
    public static class BusinessRuleJSMapper
    {
        public static char[] _SEPARATOR = {','};
        public static BusinessRuleModel MapToRuleData(this BusinessRuleJSModel it)
        {
            var ret = new BusinessRuleModel();

            ret.Id = int.Parse(it.RuleId);
            ret.CustomerId = int.Parse(it.CustomerId);            
            ret.RuleName = it.RuleName;
            ret.RuleSequence = int.Parse(it.RuleSequence);
            ret.ContinueOnSuccess = bool.Parse(it.ContinueOnSuccess);
            ret.ContinueOnError = bool.Parse(it.ContinueOnError);
            ret.RuleActive = bool.Parse(it.RuleActive);

            ret.ProcessFrom.AddItems(it.ProcessFrom, false);
            ret.ProcessTo.AddItems(it.ProcessTo, false);

            ret.SubStatusFrom.AddItems(it.SubStatusFrom, false);
            ret.SubStatusTo.AddItems(it.SubStatusTo, false);
            ret.SubStatusFrom.AddItems(it.SubStatusFrom2, false);
            ret.SubStatusTo.AddItems(it.SubStatusTo2, false);

            if (it.EmailTemplate != null)
            {
                ret.EmailTemplate = int.Parse(it.EmailTemplate);
            }
            
            ret.EmailGroups.AddItems(it.EmailGroups, false);
            ret.WorkingGroups.AddItems(it.WorkingGroups, false);
            ret.Administrators.AddItems(it.Administrators, false);

            if (!string.IsNullOrEmpty(it.Administrator) && Convert.ToInt32(it.EventId) == (int)BREventType.OnCreateCaseM2T)
            {
                ret.Administrators.Clear();
                ret.Administrators.AddItems(it.Administrator);
            }

            if (!string.IsNullOrEmpty(it.Recipients))                
                ret.Recipients = it.Recipients.Split(_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);

            ret.CaseCreator = bool.Parse(it.CaseCreator);
            ret.Initiator = bool.Parse(it.Initiator);
            ret.CaseIsAbout = bool.Parse(it.CaseIsAbout);

            ret.DomainFrom = it.Equals;
            ret.DomainTo = it.Equals;

            //Set eventID
            ret.EventId = int.Parse(it.EventId);

            ret.DisableFinishingType = bool.Parse(it.DisableFinishingType);


            return ret;
        }
    }
}