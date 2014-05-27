namespace DH.Helpdesk.Web.Models.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ReportViewModel
    {
        public ReportViewModel(List<ReportModel> reportModel, string header, bool isGrouped)
        {
            this.ReportModel = reportModel;
            this.Header = header;
            this.IsGrouped = isGrouped;
        }

        [NotNull]
        public List<ReportModel> ReportModel { get; set; }

        public string Header { get; set; }

        public bool IsGrouped { get; set; }
    }
}