namespace DH.Helpdesk.Services.Requests.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class CustomTypeFiltersRequest
    {
        public CustomTypeFiltersRequest(List<ItemOverview> departments)
        {
            this.Departments = departments;
        }

        [NotNull]
        public List<ItemOverview> Departments { get; private set; }
    }
}