namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.PrinterFieldSettings;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.Response.Inventory;
    using DH.Helpdesk.Web.Models.Common;

    public class PrinterSearchViewModel
    {
        public PrinterSearchViewModel(ConfigurableSearchFieldModel<List<SelectListItem>> departments, PrinterSearchFilter filter)
        {
            this.Filter = filter;
            this.Departments = departments;
        }

        [NotNull]
        public PrinterSearchFilter Filter { get; private set; }

        [NotNull]
        public ConfigurableSearchFieldModel<List<SelectListItem>> Departments { get; private set; }

        public static PrinterSearchViewModel BuildViewModel(PrinterSearchFilter currentFilter, PrinterFiltersResponse filters, PrinterFieldsSettingsOverviewForFilter settings)
        {
            throw new System.NotImplementedException();
        }
    }
}