namespace DH.Helpdesk.BusinessData.Models.Reports.Data.LeadtimeActiveCases
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class LeadtimeActiveCasesData
    {
        public LeadtimeActiveCasesData(
            CasesTimeLeftOnSolution highHours, 
            List<CasesTimeLeftOnSolution> mediumDays, 
            List<CasesTimeLeftOnSolution> lowDays)
        {
            this.LowDays = lowDays;
            this.MediumDays = mediumDays;
            this.HighHours = highHours;
        }

        [NotNull]
        public CasesTimeLeftOnSolution HighHours { get; private set; }

        [NotNull]
        public List<CasesTimeLeftOnSolution> MediumDays { get; private set; } 

        [NotNull]
        public List<CasesTimeLeftOnSolution> LowDays { get; private set; } 
    }
}