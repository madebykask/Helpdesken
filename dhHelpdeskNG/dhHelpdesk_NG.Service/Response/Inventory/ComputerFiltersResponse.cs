namespace DH.Helpdesk.Services.Response.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ComputerFiltersResponse
    {
        public ComputerFiltersResponse(List<ItemOverview> regions, List<ItemOverview> computerTypes)
        {
            this.Regions = regions;
            this.ComputerTypes = computerTypes;
        }

        [NotNull]
        public List<ItemOverview> Regions { get; private set; }

        [NotNull]
        public List<ItemOverview> ComputerTypes { get; private set; }
    }
}