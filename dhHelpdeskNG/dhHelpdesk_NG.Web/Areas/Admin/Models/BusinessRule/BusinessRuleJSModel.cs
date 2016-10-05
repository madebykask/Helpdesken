using DH.Helpdesk.BusinessData.Models.BusinessRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Areas.Admin.Models.BusinessRule
{
    public class BusinessRuleJSModel
    {
        public BusinessRuleJSModel()
        {
            
        }           

        public string CustomerId { get; set; }

        public string RuleId { get; set; }        

        public string RuleName { get; set; }

        public string EventId { get; set; }

        public string RuleSequence { get; set; }

        public string ContinueOnSuccess { get; set; }

        public string ContinueOnError { get; set; }

        public string RuleActive { get; set; }

        public string ProcessFrom { get; set; }

        public string ProcessTo { get; set; }

        public string SubStatusFrom { get; set; }

        public string SubStatusTo { get; set; }

        public string EmailTemplate { get; set; }

        public string EmailGroups { get; set; }

        public string WorkingGroups { get; set; }

        public string Administrators { get; set; }

        public string Recipients { get; set; }
        
    }

   
    public static class BusinessRuleJSMapper
    {
        private static char[] _SEPARATOR = {','};
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

            ret.EmailTemplate = int.Parse(it.EmailTemplate);
            ret.EmailGroups.AddItems(it.EmailGroups, false);
            ret.WorkingGroups.AddItems(it.WorkingGroups, false);
            ret.Administrators.AddItems(it.Administrators, false);
            
            if (!string.IsNullOrEmpty(it.Recipients))                
                ret.Recipients = it.Recipients.Split(_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
            
            return ret;
        }
    }
}