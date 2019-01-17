namespace DH.Helpdesk.Web.Areas.Inventory.Models.SearchModels
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Web.Enums.Inventory;
    using DH.Helpdesk.Web.Infrastructure;

    public abstract class ReportsIndexViewModel : IndexModel
    {
        protected ReportsIndexViewModel(int reportType, List<ItemOverview> reportTypes)
        {
            this.ReportType = reportType;
            this.ReportTypes = this.GetReportList(reportTypes);
        }

        public int ReportType { get; set; }

        public SelectList ReportTypes { get; private set; }

        public sealed override Tabs Tab
        {
            get { return Tabs.Reports; }
        }

        private SelectList GetReportList(List<ItemOverview> propertyTypes)
        {
            var reportTypes = from ReportTypes d in Enum.GetValues(typeof(ReportTypes))
                              select new
                              {
                                  Value = Convert.ToInt32(d).ToString(CultureInfo.InvariantCulture),
                                  Name = Translation.GetCoreTextTranslation(d.GetCaption())
                              };

            var reportTypeList = reportTypes.OrderBy(x => x.Name).Union(propertyTypes.Select(x => new { x.Value, x.Name }));
            var reportTypeSelectList = new SelectList(reportTypeList, "Value", "Name");

            return reportTypeSelectList;
        }
    }
}