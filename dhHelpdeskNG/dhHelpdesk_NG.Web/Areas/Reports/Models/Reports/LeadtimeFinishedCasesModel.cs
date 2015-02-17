namespace DH.Helpdesk.Web.Areas.Reports.Models.Reports
{
    using DH.Helpdesk.BusinessData.Models.Reports.Data.LeadtimeFinishedCases;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class LeadtimeFinishedCasesModel
    {
        public LeadtimeFinishedCasesModel(
            LeadtimeFinishedCasesData data, 
            bool isShowDetails)
        {
            this.IsShowDetails = isShowDetails;
            this.Data = data;
        }

        [NotNull]
        public LeadtimeFinishedCasesData Data { get; private set; }

        public bool IsShowDetails { get; private set; }
    }
}