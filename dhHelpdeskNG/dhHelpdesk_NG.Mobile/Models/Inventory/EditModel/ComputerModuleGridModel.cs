namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ComputerModuleGridModel
    {
        public ComputerModuleGridModel(List<ItemOverview> overviews, ModuleTypes moduleType)
        {
            this.Overviews = overviews;
            this.ModuleType = moduleType;
        }

        [NotNull]
        public List<ItemOverview> Overviews { get; set; }

        public ModuleTypes ModuleType { get; set; }
    }
}