using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Printer
{
    public class PrinterEditViewModel : BaseEditPrinterModel
    {
        public PrinterEditViewModel(int id, PrinterViewModel printerViewModel) : base(id)
        {
            PrinterViewModel = printerViewModel;
        }

        public PrinterViewModel PrinterViewModel { get; set; }

        public bool UserHasInventoryAdminPermission { get; set; }

        public override PrinterEditTabs Tab
        {
            get { return PrinterEditTabs.Printer; }
        }
    }
}