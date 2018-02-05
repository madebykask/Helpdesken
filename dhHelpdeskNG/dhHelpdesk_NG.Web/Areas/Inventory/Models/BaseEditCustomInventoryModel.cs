using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Areas.Inventory.Models
{
    public abstract class BaseEditCustomInventoryModel
    {
        protected BaseEditCustomInventoryModel(int id)
        {
            this.Id = id;
        }

        public int Id { get; set; }

        public bool IsForDialog { get; set; }

        public string UserId { get; set; }

        public int InventoryTypeId { get; set; }

        public string InventoryName { get; set; }

        public bool UserHasInventoryAdminPermission { get; set; }

        public abstract CustomInventoryTabs Tab { get; }
    }
}