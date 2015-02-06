namespace DH.Helpdesk.Web.Areas.Reports.Models.Reports
{
    using DH.Helpdesk.BusinessData.Models.Reports.Data.ReportGenerator;

    public sealed class ReportGeneratorModel
    {
        public ReportGeneratorModel(ReportGeneratorData data)
        {
            this.Data = data;
        }

        public ReportGeneratorData Data { get; private set; }
    }
}