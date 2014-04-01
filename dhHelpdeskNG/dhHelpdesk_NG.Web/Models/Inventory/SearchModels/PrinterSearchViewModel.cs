namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
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
    }
}