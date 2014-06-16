namespace DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeDetailedOverview
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Shared;

    public sealed class RegistrationFields
    {
        public RegistrationFields(
            List<Contact> contacts,
            string owner,
            List<ItemOverview> affectedProcesses,
            List<ItemOverview> affectedDepartments,
            string description,
            string businessBenefits,
            string consequence,
            string impact,
            DateTime? desiredDate,
            bool verified,
            StepStatus approval,
            string rejectExplanation)
        {
            this.Contacts = contacts;
            this.Owner = owner;
            this.AffectedProcesses = affectedProcesses;
            this.AffectedDepartments = affectedDepartments;
            this.Description = description;
            this.BusinessBenefits = businessBenefits;
            this.Consequence = consequence;
            this.Impact = impact;
            this.DesiredDate = desiredDate;
            this.Verified = verified;
            this.Approval = approval;
            this.RejectExplanation = rejectExplanation;
        }

        public List<Contact> Contacts { get; private set; }

        public string Owner { get; private set; }

        public List<ItemOverview> AffectedProcesses { get; private set; }

        public List<ItemOverview> AffectedDepartments { get; private set; }

        public string Description { get; private set; }

        public string BusinessBenefits { get; private set; }

        public string Consequence { get; private set; }

        public string Impact { get; private set; }

        public DateTime? DesiredDate { get; private set; }

        public bool Verified { get; private set; }

        public StepStatus Approval { get; private set; }

        public string RejectExplanation { get; private set; }
    }
}
