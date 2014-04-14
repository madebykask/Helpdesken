namespace DH.Helpdesk.Web.Models.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class ReportViewModel
    {
        public ReportViewModel(List<ReportModel> reportModel, ReportTypes reportTypes)
        {
            this.ReportModel = reportModel;
            this.ReportTypes = reportTypes;
        }

        [NotNull]
        public List<ReportModel> ReportModel { get; set; }

        public ReportTypes ReportTypes { get; set; }

        public bool IsGrouped { get; set; }
    }
}