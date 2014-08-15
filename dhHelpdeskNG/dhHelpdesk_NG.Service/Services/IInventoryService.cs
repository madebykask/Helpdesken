namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server;
    using DH.Helpdesk.BusinessData.Models.Inventory.Input;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Printer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Server;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Services.Requests.Inventory;
    using DH.Helpdesk.Services.Response.Inventory;

    public interface IInventoryService
    {
        List<ComputerUserOverview> GetComputerUsers(int customerId, string searchFor);

        List<ComputerUserOverview> GetComputerUserHistory(int computerId);

        void AddInventoryType(InventoryType businessModel);

        void UpdateInventoryType(InventoryType businessModel);

        void UpdateWorkstationInfo(int id, string info);

        void DeleteInventoryType(int id);

        InventoryType GetInventoryType(int id);

        List<ItemOverview> GetInventoryTypes(int customerId);

        List<ItemOverview> GetNotConnectedInventory(int inventoryType, int computerId);

        List<InventoryReportModel> GetInventoryCounts(int customerId, int? departmentId);

        ReportModelWithInventoryType GetAllConnectedInventory(int inventoryTypeId, int? departmentId, string searchFor);

        ComputerShortOverview GetWorkstationShortInfo(int computerId);

        #region Workstation

        void AddComputerLog(ComputerLog businessModel);

        void DeleteComputerLog(int id);

        void AddWorkstation(Computer businessModel, OperationContext context);

        void DeleteWorkstation(int id);

        void UpdateWorkstation(Computer businessModel, OperationContext context);

        Computer GetWorkstation(int id);

        List<ComputerLogOverview> GetWorkstationLogOverviews(int id);

        List<ComputerOverview> GetWorkstations(ComputersFilter computersFilter);

        #endregion

        #region Server

        void AddServer(Server businessModel, OperationContext context);

        void DeleteServer(int id);

        void UpdateServer(Server businessModel, OperationContext context);

        Server GetServer(int id);

        List<OperationServerLogOverview> GetOperationServerLogOverviews(int id, int customerId);

        List<ServerOverview> GetServers(ServersFilter computersFilter);

        #endregion

        #region Printer

        void AddPrinter(Printer businessModel);

        void DeletePrinter(int id);

        void UpdatePrinter(Printer businessModel);

        Printer GetPrinter(int id);

        List<PrinterOverview> GetPrinters(PrintersFilter printersFilter);

        #endregion

        #region DynamicType

        InventoryOverviewResponse GetInventory(int id);

        InventoriesOverviewResponse GetInventories(InventoriesFilter filter);

        InventoryOverviewResponseWithType GetConnectedToComputerInventories(int computerId);

        List<TypeGroupModel> GetTypeGroupModels(int inventoryTypeId);

        void ConnectInventoryToComputer(int inventoryId, int computerId);

        void RemoveInventoryFromComputer(int inventoryId, int computerId);

        #endregion
    }
}