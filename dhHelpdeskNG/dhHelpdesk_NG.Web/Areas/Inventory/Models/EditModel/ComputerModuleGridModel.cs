namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.Extensions;

    public class ComputerModuleGridModel : IndexModel
    {
        public ComputerModuleGridModel(List<ItemOverview> overviews, ModuleTypes moduleType)
        {
            this.Overviews = overviews;
            this.ModuleType = (int)moduleType;

            this.ModuleTypes = new ModuleTypes().ToSelectList(((int)moduleType).ToString(CultureInfo.InvariantCulture));
        }

        public override Tabs Tab
        {
            get
            {
                return Tabs.MasterData;
            }
        }

        [NotNull]
        public List<ItemOverview> Overviews { get; set; }

        public int ModuleType { get; set; }

        public SelectList ModuleTypes { get; private set; }
    }
}