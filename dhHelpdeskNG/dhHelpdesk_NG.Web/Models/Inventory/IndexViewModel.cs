namespace DH.Helpdesk.Web.Models.Inventory
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class IndexViewModel
    {
        public const string Separator = "Separator";

        public IndexViewModel(int currentMode, List<SelectListItem> propertyTypes)
        {
            this.CurrentMode = currentMode;
            this.PropertyTypes = propertyTypes;
        }

        public int CurrentMode { get; private set; }

        [NotNull]
        public List<SelectListItem> PropertyTypes { get; private set; }

        public static IndexViewModel BuildViewModel(int currentMode, List<ItemOverview> propertyTypes)
        {
            var items = new List<SelectListItem>
                            {
                                new SelectListItem
                                    {
                                        Text = CurrentModes.Workstations.ToString(),
                                        Value = ((int)CurrentModes.Workstations).ToString(CultureInfo.InvariantCulture)
                                    },
                                new SelectListItem
                                    {
                                        Text = CurrentModes.Servers.ToString(),
                                        Value = ((int)CurrentModes.Servers).ToString(CultureInfo.InvariantCulture)
                                    },
                                new SelectListItem
                                    {
                                        Text = CurrentModes.Printers.ToString(),
                                        Value = ((int)CurrentModes.Printers).ToString(CultureInfo.InvariantCulture)
                                    },
                                new SelectListItem
                                    {
                                        Text = "-------------",
                                        Value = Separator,
                                        Selected = false,
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