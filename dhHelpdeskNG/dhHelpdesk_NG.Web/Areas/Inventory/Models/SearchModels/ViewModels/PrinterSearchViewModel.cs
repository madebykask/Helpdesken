namespace DH.Helpdesk.Web.Areas.Inventory.Models.SearchModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.PrinterFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class PrinterSearchViewModel : BaseIndexModel
    {
        public PrinterSearchViewModel(
            SelectList departments,
            PrinterSearchFilter filter,
            PrinterFieldsSettingsOverviewForFilter settings,
            int currentMode,
            List<ItemOverview> types)
            : base(currentMode, types)
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

        public bool UserHasInventoryAdminPermission { get; set; }

        public static PrinterSearchViewModel BuildViewModel(
            PrinterSearchFilter currentFilter,
            List<ItemOverview> departments,
            PrinterFieldsSettingsOverviewForFilter settings,
            int currentMode,
            List<ItemOverview> types)
        {
            var list = new SelectList(departments, "Value", "Name");

            return new PrinterSearchViewModel(list, currentFilter, settings, currentMode, types);
        }
    }
}