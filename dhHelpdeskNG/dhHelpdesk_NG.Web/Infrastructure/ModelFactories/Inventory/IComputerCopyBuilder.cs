using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer;
using DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory.Concrete;

namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory
{

    public interface IComputerCopyBuilder
    {
        ComputerViewModel CopyWorkstation(ComputerViewModel destModel, ComputerViewModel sourceModel, OperationContext operationContext);
    }
}