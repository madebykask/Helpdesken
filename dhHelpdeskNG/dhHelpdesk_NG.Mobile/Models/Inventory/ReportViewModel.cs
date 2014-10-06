namespace DH.Helpdesk.Mobile.Models.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ReportViewModel
    {
        public ReportViewModel(List<ReportModelWrapper> reportModel, string header, bool isGrouped)
        {
            this.ReportModel = reportModel;
            this.Header = header;
            this.IsGrouped = isGrouped;
        }

        [NotNull]
        public List<ReportModelWrapper> ReportModel { get; set; }

        public string Header { get; set; }

        public bool IsGrouped { get; set; }
    }
}