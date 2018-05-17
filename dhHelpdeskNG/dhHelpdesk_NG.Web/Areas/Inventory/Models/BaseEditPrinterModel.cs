using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Areas.Inventory.Models
{
    public abstract class BaseEditPrinterModel
    {
        protected BaseEditPrinterModel(int id)
        {
            this.Id = id;
        }

        public int Id { get; set; }

        public bool IsForDialog { get; set; }

        public string UserId { get; set; }

        public abstract PrinterEditTabs Tab { get; }
    }
}