namespace DH.Helpdesk.Web.Areas.Reports.Models.Reports
{
    using DH.Helpdesk.BusinessData.Models.Reports.Data.LeadtimeActiveCases;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class LeadtimeActiveCasesModel
    {
        public LeadtimeActiveCasesModel(LeadtimeActiveCasesData data)
        {
            this.Data = data;
        }

        [NotNull]
        public LeadtimeActiveCasesData Data { get; private set; }
    }
}