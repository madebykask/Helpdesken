namespace DH.Helpdesk.Web.Areas.Inventory.Models.SearchModels
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Web.Enums.Inventory;

    public class SettingsIndexViewModel : IndexModel
    {
        public SettingsIndexViewModel(List<ItemOverview> inventoryTypes)
        {
            this.InventoryTypes = this.GetTypes(inventoryTypes);
        }

        public List<ItemOverview> InventoryTypes { get; set; }

        public override Tabs Tab
        {
            get
            {
                return Tabs.Settings;
            }
        }

        private List<ItemOverview> GetTypes(List<ItemOverview> types)
        {
            var inventoryTypes = (from Enum d in Enum.GetValues(typeof(CurrentModes))
                                  select
                                      new
                                      {
                                          Value = Convert.ToInt32(d).ToString(CultureInfo.InvariantCulture),
                                          Name = d.ToString()
                                      }).ToList();

            List<ItemOverview> inventoryTypeList =
                inventoryTypes.Select(x => new ItemOverview(x.Name, x.Value)).ToList();
            inventoryTypeList.AddRange(types);

            return inventoryTypeList;
        }
    }
}