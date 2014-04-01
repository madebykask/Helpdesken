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
            var items = new List<SelectListItem>
                            {
                                new SelectListItem
                                    {
                                        Text = CurrentModes.Workstations.ToString(),
                                        Value = CurrentModes.Workstations.ToString()
                                    },
                                new SelectListItem
                                    {
                                        Text = CurrentModes.Servers.ToString(),
                                        Value = CurrentModes.Servers.ToString()
                                    },
                                new SelectListItem
                                    {
                                        Text = CurrentModes.Printers.ToString(),
                                        Value = CurrentModes.Printers.ToString()
                                    },
                                new SelectListItem
                                    {
                                        Text = "-------------",
                                        Selected = false,
                                        Value = "-1"
                                    }
                            };

            items.AddRange(
                propertyTypes.Select(
                    x => new SelectListItem { Text = x.Name, Value = x.Value }));

            var viewModel = new IndexViewModel(currentMode, items);

            return viewModel;
        }
    }
}