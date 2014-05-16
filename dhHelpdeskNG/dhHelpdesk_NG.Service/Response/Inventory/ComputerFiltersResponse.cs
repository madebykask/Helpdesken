namespace DH.Helpdesk.Services.Response.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ComputerFiltersResponse
    {
        public ComputerFiltersResponse(List<ItemOverview> computerTypes)
        {
            this.ComputerTypes = computerTypes;
        }

        [NotNull]
        public List<ItemOverview> ComputerTypes { get; private set; }
    }
}