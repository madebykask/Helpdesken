namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Data
{
    using System;

    public sealed class RegistrationFieldGroupDto
    {
        public RegistrationFieldGroupDto(
            string description,
            string businessBenefits,
            string consequence,
            string impact,
            DateTime? desiredDate,
            bool verified,
            bool approval,
            string explanation)
        {
            Description = description;
            BusinessBenefits = businessBenefits;
            Consequence = consequence;
            Impact = impact;
            DesiredDate = desiredDate;
            Verified = verified;
            Approval = approval;
            Explanation = explanation;
        }

        public string Description { get; private set; }

        public string BusinessBenefits { get; private set; }

        public string Consequence { get; private set; }

        public string Impact { get; private set; }

        public DateTime? DesiredDate { get; private set; }

        public bool Verified { get; private set; }

        public bool Approval { get; private set; }

        public string Explanation { get; private set; }
    }
}
