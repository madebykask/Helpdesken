namespace DH.Helpdesk.Web.Areas.Inventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Web.Enums.Inventory;
    using DH.Helpdesk.Web.Infrastructure;

    public abstract class BaseIndexModel : IndexModel
    {
        public const string Separator = "Separator";

        protected BaseIndexModel(int currentMode, List<ItemOverview> types)
        {
            this.CurrentMode = currentMode;

            this.GetTypes(types);
        }

        public SelectList InventoryTypes { get; set; }

        public int CurrentMode { get; set; }

        public sealed override Tabs Tab
        {
            get { return Tabs.Inventory; }
        }

        private void GetTypes(List<ItemOverview> types)
        {
            var inventoryTypes = (from CurrentModes d in Enum.GetValues(typeof(CurrentModes))
                                  select
                                      new
                                          {
                                              Value = Convert.ToInt32(d).ToString(CultureInfo.InvariantCulture),
                                              Name = Translation.GetCoreTextTranslation(d.GetCaption())
                                          }).OrderBy(d => d.Name).ToList();

            var inventoryTypeList = inventoryTypes.Select(x => new ItemOverview(x.Name, x.Value)).ToList();
            inventoryTypeList.AddRange(types);

            inventoryTypes.Add(new { Value = Separator, Name = "-------------" });
            var inventoryTypeListWithSeparator = inventoryTypes.Union(types.Select(x => new { x.Value, x.Name }));
            var inventoryTypeSelectList = new SelectList(inventoryTypeListWithSeparator, "Value", "Name");

            this.InventoryTypes = inventoryTypeSelectList;
        }
    }
}