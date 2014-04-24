namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.PrinterFieldSettings;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.Response.Inventory;

    public class PrinterSearchViewModel
    {
        public PrinterSearchViewModel(
            SelectList departments,
            PrinterSearchFilter filter,
            PrinterFieldsSettingsOverviewForFilter settings)
        {
            this.Settings = settings;
            this.Filter = filter;
            this.Departments = departments;
        }

        [NotNull]
        public PrinterSearchFilter Filter { get; private set; }

        [NotNull]
        public SelectList Departments { get; private set; }

        public PrinterFieldsSettingsOverviewForFilter Settings { get; private set; }

        public static PrinterSearchViewModel BuildViewModel(
            PrinterSearchFilter currentFilter,
            PrinterFiltersResponse additionalData,
            PrinterFieldsSettingsOverviewForFilter settings)
        {
            var departments = new SelectList(additionalData.Departments, "Value", "Name");

            return new PrinterSearchViewModel(departments, currentFilter, settings);
        }
    }
}