namespace DH.Helpdesk.Web.Models.Inventory
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Enums.Inventory;
    using DH.Helpdesk.Web.Infrastructure.Extensions;

    public class IndexViewModel
    {
        public const string Separator = "Separator";

        private IndexViewModel(
            int currentMode,
            int reportType,
            int moduleType,
            SelectList propertyTypes,
            SelectList reportTypes,
            SelectList moduleTypes,
            string activeTab)
        {
            this.CurrentMode = currentMode;
            this.ReportType = reportType;
            this.ModuleType = moduleType;
            this.PropertyTypes = propertyTypes;
            this.ReportTypes = reportTypes;
            this.ModuleTypes = moduleTypes;
            this.ActiveTab = activeTab;
        }

        public int CurrentMode { get; private set; }

        public int ReportType { get; private set; }

        public int ModuleType { get; private set; }

        [NotNull]
        public SelectList PropertyTypes { get; private set; }

        [NotNull]
        public SelectList ReportTypes { get; private set; }

        [NotNull]
        public SelectList ModuleTypes { get; private set; }

        public string ActiveTab { get; private set; }

        public static IndexViewModel BuildViewModel(
            int currentMode,
            int reportType,
            int moduleType,
            List<ItemOverview> propertyTypes,
            string activeTab)
        {
            var inventoryTypes = (from Enum d in Enum.GetValues(typeof(CurrentModes))
                                  select
                                      new
                                          {
                                              Value = Convert.ToInt32(d).ToString(CultureInfo.InvariantCulture),
                                              Name = d.ToString()
                                          }).ToList();
            inventoryTypes.Add(new { Value = Separator, Name = "-------------" });
            var inventoryTypeList = inventoryTypes.Union(propertyTypes.Select(x => new { x.Value, x.Name }));
            var inventoryTypeSelectList = new SelectList(inventoryTypeList, "Value", "Name");

            var reportTypes = from Enum d in Enum.GetValues(typeof(ReportTypes))
                              select
                                  new
                                  {
                                      Value = Convert.ToInt32(d).ToString(CultureInfo.InvariantCulture),
                                      Name = d.ToString()
                                  };
            var reportTypeList = reportTypes.Union(propertyTypes.Select(x => new { x.Value, x.Name }));
            var reportTypeSelectList = new SelectList(reportTypeList, "Value", "Name");

            var moduleTypes = EditModel.ModuleTypes.Processor.ToSelectList();

            var viewModel = new IndexViewModel(
                currentMode,
                reportType,
                moduleType,
                inventoryTypeSelectList,
                reportTypeSelectList,
                moduleTypes,
                activeTab);

            return viewModel;
        }
    }
}