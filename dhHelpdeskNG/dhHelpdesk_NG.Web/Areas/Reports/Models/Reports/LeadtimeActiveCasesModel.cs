namespace DH.Helpdesk.Web.Areas.Reports.Models.Reports
{
    using DH.Helpdesk.BusinessData.Models.Reports.Data.LeadtimeActiveCases;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class LeadtimeActiveCasesModel
    {
        public LeadtimeActiveCasesModel(
            LeadtimeActiveCasesData data, 
            int highHours, 
            int mediumDays, 
            int lowDays)
        {
            this.LowDays = lowDays;
            this.MediumDays = mediumDays;
            this.HighHours = highHours;
            this.Data = data;
        }

        [NotNull]
        public LeadtimeActiveCasesData Data { get; private set; }

        [MinValue(0)]
        public int HighHours { get; private set; }

        [MinValue(0)]
        public int MediumDays { get; private set; }

        [MinValue(0)]
        public int LowDays { get; private set; }
    }
}