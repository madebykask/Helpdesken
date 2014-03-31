namespace DH.Helpdesk.Services.Requests.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ComputerFiltersRequest
    {
        public ComputerFiltersRequest(List<ItemOverview> regions, List<ItemOverview> departments, List<ItemOverview> computerTypes)
        {
            this.Regions = regions;
            this.Departments = departments;
            this.ComputerTypes = computerTypes;
        }

        [NotNull]
        public List<ItemOverview> Regions { get; private set; }

        [NotNull]
        public List<ItemOverview> Departments { get; private set; }

        [NotNull]
        public List<ItemOverview> ComputerTypes { get; private set; }
    }
}