using DH.Helpdesk.BusinessData.Enums.Inventory;
using DH.Helpdesk.BusinessData.Models.User.Input;

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
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.Types;
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

        void AddWorkstation(ComputerForInsert businessModel, OperationContext context);

        void DeleteWorkstation(int id);

        void UpdateWorkstation(ComputerForUpdate businessModel, OperationContext context);

        ComputerForRead GetWorkstation(int id);

        List<ComputerLogOverview> GetWorkstationLogOverviews(int id);

        List<ComputerOverview> GetWorkstations(ComputersFilter computersFilter);

        int GetWorkstationIdByName(string computerName, int customerId);

        ComputerForRead GetWorkstationByNumber(string computerName, int customerId);

        #endregion

        #region Server

        void AddServer(ServerForInsert businessModel, OperationContext context);

        void DeleteServer(int id);

        void UpdateServer(ServerForUpdate businessModel, OperationContext context);

        ServerForRead GetServer(int id);

        List<OperationServerLogOverview> GetOperationServerLogOverviews(int id, int customerId);

        ServerOverview[] GetServers(ServersFilter computersFilter);

        int GetServerIdByName(string serverName, int customerId);

        #endregion

        #region Printer

        void AddPrinter(PrinterForInsert businessModel, OperationContext context);

        void DeletePrinter(int id);

        void UpdatePrinter(PrinterForUpdate businessModel, OperationContext context);

        PrinterForRead GetPrinter(int id);

        List<PrinterOverview> GetPrinters(PrintersFilter printersFilter);

        int GetPrinterIdByName(string printerName, int customerId);

        #endregion

        #region DynamicType

        void AddInventory(InventoryForInsert businessModel);

        void AddDynamicFieldsValuesInventory(List<InventoryValueForWrite> dynamicBusinessModels);

        void UpdateInventory(InventoryForUpdate businessModel, List<InventoryValueForWrite> dynamicBusinessModels, int inventoryTypeId);

        void DeleteInventory(int id);

        InventoryOverviewResponse GetInventory(int id);

        InventoriesOverviewResponse GetInventories(InventoriesFilter filter);

        InventoryOverviewResponseWithType GetConnectedToComputerInventories(int computerId);

        List<TypeGroupModel> GetTypeGroupModels(int inventoryTypeId);

        void ConnectInventoryToComputer(int inventoryId, int computerId);

        void RemoveInventoryFromComputer(int inventoryId, int computerId);

        int GetCustomInventoryIdByName(string inventoryName, int inventoryTypeId);

        #endregion

        List<ComputerOverview> GetRelatedInventory(int customerId, string userId);

        List<int> GetRelatedCaseIds(CurrentModes inventoryType, int inventoryId, int customerId);
    }
}