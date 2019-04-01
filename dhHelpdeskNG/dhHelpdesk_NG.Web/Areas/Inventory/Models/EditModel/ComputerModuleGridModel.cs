namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.Extensions;
    using DH.Helpdesk.Web.Infrastructure;
    using System;
    using System.Linq;

    public class ComputerModuleGridModel : IndexModel
    {
        public ComputerModuleGridModel(List<ItemOverview> overviews, ModuleTypes moduleType, bool userInventoryAdminPermission, bool userInventoryViewPermission)
        {
            this.Overviews = overviews;
            this.ModuleType = (int)moduleType;
            this.UserHasInventoryAdminPermission = userInventoryAdminPermission;
			this.UserHasInventoryViewPermission = userInventoryViewPermission;
			this.ModuleTypes = new ModuleTypes().MapToSelectList(moduleType.GetCaption());
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

        public bool UserHasInventoryAdminPermission { get; private set; }
		public bool UserHasInventoryViewPermission { get; private set; }
	}
}