namespace DH.Helpdesk.Web.Models.Inventory
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class IndexViewModel
    {
        public IndexViewModel(CurrentModes currentMode, List<SelectListItem> propertyTypes)
        {
            this.CurrentMode = currentMode;
            this.PropertyTypes = propertyTypes;
        }

        public CurrentModes CurrentMode { get; private set; }

        [NotNull]
        public List<SelectListItem> PropertyTypes { get; private set; }

        public static IndexViewModel GetModel(CurrentModes currentMode, List<ItemOverview> propertyTypes)
        {
            var items =
                propertyTypes.Select(
                    x => new SelectListItem { Text = x.Name, Value = x.Value }).ToList();

            items.Add(new SelectListItem { Text = CurrentModes.Workstations.ToString() });
            items.Add(new SelectListItem { Text = CurrentModes.Servers.ToString() });
            items.Add(new SelectListItem { Text = CurrentModes.Printers.ToString() });

            var viewModel = new IndexViewModel(currentMode, items);

            return viewModel;
        }
    }
}