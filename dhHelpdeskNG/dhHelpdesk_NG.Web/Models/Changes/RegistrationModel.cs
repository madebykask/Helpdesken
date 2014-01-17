namespace dhHelpdesk_NG.Web.Models.Changes
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using dhHelpdesk_NG.Web.Infrastructure;
    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public sealed class RegistrationModel
    {
        public RegistrationModel(
            MultiSelectList departmentsAffected,
            string description,
            string businessBenefits,
            string consequence,
            string impact,
            DateTime desiredDateTime,
            bool verified,
            SelectList approved,
            string approvableExplanation,
            DateTime approvedDateTime,
            string approvedUser)
        {
            this.DepartmentsAffected = departmentsAffected;
            this.Description = description;
            this.BusinessBenefits = businessBenefits;
            this.Consequence = consequence;
            this.Impact = impact;
            this.DesiredDate = desiredDateTime;
            this.Verified = verified;
            this.Approved = approved;
            this.ApprovableExplanation = approvableExplanation;
            this.ApprovedDateTime = approvedDateTime;
            this.ApprovedUser = approvedUser;
        }

        public RegistrationModel()
        {
            this.AttachedFiles = new List<string>();
        }

        [LocalizedDisplay("Departments affected")]
        public MultiSelectList DepartmentsAffected { get; set; }

        [LocalizedDisplay("Description")]
        public string Description { get; set; }

        [LocalizedDisplay("Business benefits")]
        public string BusinessBenefits { get; set; }

        [LocalizedDisplay("Consequence")]
        public string Consequence { get; set; }

        [LocalizedDisplay("Impact")]
        public string Impact { get; set; }

        [LocalizedDisplay("Desired date")]
        public DateTime DesiredDate { get; set; }

        [LocalizedDisplay("Verified")]
        public bool Verified { get; set; }

        [LocalizedDisplay("Approved")]
        public SelectList Approved { get; set; }

        public Enums.RegistrationApproveResult ApprovedValue { get; set; }

        [LocalizedDisplay("Explanation")]
        public string ApprovableExplanation { get; set; }

        [LocalizedDisplay("Attached files")]
        public List<string> AttachedFiles { get; set; }

        public DateTime ApprovedDateTime { get; set; }

        public string ApprovedUser { get; set; }
    }
}