namespace DH.Helpdesk.Services.Response.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class CustomTypeFiltersResponse
    {
        public CustomTypeFiltersResponse(List<ItemOverview> departments)
        {
            this.Departments = departments;
        }

        [NotNull]
        public List<ItemOverview> Departments { get; private set; }
    }
}