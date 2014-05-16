namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;

    public class DropDownViewModel
    {
        public DropDownViewModel(SelectList values)
        {
            this.Values = values;
        }

        public int? Selected { get; set; }

        public SelectList Values { get; set; }

        public bool AllowEmpty { get; set; }

        public string PropertyName { get; set; }

        public static DropDownViewModel BuildViewModel(List<ItemOverview> overviews)
        {
            var values = new SelectList(overviews, "Value", "Name");

            return new DropDownViewModel(values);
        }

        public static DropDownViewModel BuildViewModel(List<ItemOverview> overviews, string selected)
        {
            var values = new SelectList(overviews, "Value", "Name", selected);

            return new DropDownViewModel(values);
        }

        public static DropDownViewModel BuildDefault()
        {
            return new DropDownViewModel(new SelectList(Enumerable.Empty<SelectListItem>()));
        }
    }
}