using System.Linq;
using DH.Helpdesk.Web.Infrastructure;

namespace DH.Helpdesk.Web.Areas.Inventory.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Shared;

    public abstract class BaseIndexModel : IndexModel
    {
        public const string Separator = "Separator";

        protected BaseIndexModel(int currentMode, List<ItemOverview> types)
        {
            CurrentMode = currentMode;
            InventoryTypes = new SelectList(types.Select(x => new { Name = Translation.GetCoreTextTranslation(x.Name), x.Value }), "Value", "Name");
        }

        public SelectList InventoryTypes { get; set; }

        public int CurrentMode { get; set; }

        public sealed override Tabs Tab
        {
            get { return Tabs.Inventory; }
        }
    }
}