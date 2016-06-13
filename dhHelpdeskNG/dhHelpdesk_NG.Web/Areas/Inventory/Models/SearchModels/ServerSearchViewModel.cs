namespace DH.Helpdesk.Web.Areas.Inventory.Models.SearchModels
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ServerSearchViewModel : BaseIndexModel
    {
        public ServerSearchViewModel(int currentMode, List<ItemOverview> types, ServerSearchFilter serverSearchFilter)
            : base(currentMode, types)
        {
            this.Filter = serverSearchFilter;
        }

        [NotNull]
        public ServerSearchFilter Filter { get; set; }

        public bool UserHasInventoryAdminPermission { get; set; }
    }
}