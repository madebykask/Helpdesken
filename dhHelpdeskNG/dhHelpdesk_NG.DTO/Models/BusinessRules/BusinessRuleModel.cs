using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.BusinessData.Models.Shared.Input;
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

        public int EmailTemplate { get; set; }

        public SelectedItems EmailGroups { get; set; }

        public SelectedItems WorkingGroups { get; set; }

        public SelectedItems Administrators { get; set; }        

        public string[] Recipients { get; set; }        
    }
    
}
