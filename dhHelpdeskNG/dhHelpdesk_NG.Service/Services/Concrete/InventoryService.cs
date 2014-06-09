namespace DH.Helpdesk.Services.Services.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Inventory;
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
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Computers;
    using DH.Helpdesk.Dal.Repositories.Inventory;
    using DH.Helpdesk.Dal.Repositories.Inventory.Concrete;
    using DH.Helpdesk.Dal.Repositories.Printers;
    using DH.Helpdesk.Dal.Repositories.Servers;
    using DH.Helpdesk.Services.Requests.Inventory;
    using DH.Helpdesk.Services.Response.Inventory;

    public class InventoryService : IInventoryService
    {
        private readonly IInventoryTypeRepository inventoryTypeRepository;

        private readonly IComputerRepository computerRepository;

        private readonly IServerRepository serverRepository;

        private readonly IPrinterRepository printerRepository;

        private readonly IInventoryRepository inventoryRepository;

        private readonly IInventoryTypePropertyValueRepository inventoryTypePropertyValueRepository;

        private readonly IComputerLogRepository computerLogRepository;

        private readonly IComputerInventoryRepository computerInventoryRepository;

        private readonly IOperationLogRepository operationLogRepository;

        private readonly InventoryTypeGroupRepository inventoryTypeGroupRepository;

        public InventoryService(
            IInventoryTypeRepository inventoryTypeRepository,
            IComputerRepository computerRepository,
            IServerRepository serverRepository,
            IPrinterRepository printerRepository,
            IInventoryRepository inventoryRepository,
            IInventoryTypePropertyValueRepository inventoryTypePropertyValueRepository,
            IComputerLogRepository computerLogRepository,
            IComputerInventoryRepository computerInventoryRepository,
            IOperationLogRepository operationLogRepository,
            InventoryTypeGroupRepository inventoryTypeGroupRepository)
        {
            this.inventoryTypeRepository = inventoryTypeRepository;
            this.computerRepository = computerRepository;
            this.serverRepository = serverRepository;
            this.printerRepository = printerRepository;
            this.inventoryRepository = inventoryRepository;
            this.inventoryTypePropertyValueRepository = inventoryTypePropertyValueRepository;
            this.computerLogRepository = computerLogRepository;
            this.computerInventoryRepository = computerInventoryRepository;
            this.operationLogRepository = operationLogRepository;
            this.inventoryTypeGroupRepository = inventoryTypeGroupRepository;
        }

        public void AddInventoryType(InventoryType businessModel)
        {
            this.inventoryTypeRepository.Add(businessModel);
            this.inventoryTypeRepository.Commit();
        }

        public void UpdateInventoryType(InventoryType businessModel)
        {
            this.inventoryTypeRepository.Update(businessModel);
            this.inventoryTypeRepository.Commit();
        }

        public InventoryType GetInventoryType(int id)
        {
            return this.inventoryTypeRepository.FindById(id);
        }

        public List<ItemOverview> GetInventoryTypes(int customerId)
        {
            return this.inventoryTypeRepository.FindOverviews(customerId);
        }

        public List<ItemOverview> GetNotConnectedInventory(int inventoryType, int computerId)
        {
            return this.inventoryRepository.FindNotConnectedOverviews(inventoryType, computerId);
        }

        public List<InventoryReportModel> GetInventoryCounts(int customerId, int? departmentId)
        {
            var workstationCount = new InventoryReportModel(
                CurrentModes.Workstations.ToString(),
                this.computerRepository.GetComputerCount(customerId, departmentId));
            var serverCount = new InventoryReportModel(
                CurrentModes.Servers.ToString(),
                this.serverRepository.GetServerCount(customerId));
            var printerCount = new InventoryReportModel(
                CurrentModes.Printers.ToString(),
                this.printerRepository.GetPrinterCount(customerId, departmentId));

            var inventoryCounts = this.inventoryTypeRepository.FindInventoriesWithCount(customerId, departmentId);
            var models = new List<InventoryReportModel> { workstationCount, serverCount, printerCount };
            models.AddRange(inventoryCounts);

            return models;
        }

        public ReportModelWithInventoryType GetAllConnectedInventory(int inventoryTypeId, int? departmentId, string searchFor)
        {
            return this.inventoryRepository.FindAllConnectedInventory(
                inventoryTypeId,
                departmentId,
                searchFor);
        }

        #region Workstation

        public void AddComputerLog(ComputerLog businessModel)
        {
            this.computerLogRepository.Add(businessModel);
            this.computerLogRepository.Commit();
        }

        public void DeleteComputerLog(int id)
        {
            this.computerLogRepository.DeleteById(id);
            this.computerLogRepository.Commit();
        }

        public void AddWorkstation(Computer businessModel)
        {
            throw new NotImplementedException();
        }

        public void DeleteWorkstation(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateWorkstation(Computer businessModel)
        {
            throw new NotImplementedException();
        }

        public Computer GetWorkstation(int id)
        {
            return this.computerRepository.FindById(id);
        }

        public List<ComputerLogOverview> GetWorkstationLogOverviews(int id)
        {
            return this.computerLogRepository.Find(id);
        }

        public List<ComputerOverview> GetWorkstations(ComputersFilter computersFilter)
        {
            var computerOverviews = this.computerRepository.FindOverviews(
                computersFilter.CustomerId,
                computersFilter.RegionId,
                computersFilter.DepartmentId,
                computersFilter.ComputerTypeId,
                computersFilter.ContractStatusId,
                computersFilter.ContractStartDateFrom,
                computersFilter.ContractStartDateTo,
                computersFilter.ContractEndDateFrom,
                computersFilter.ContractEndDateTo,
                computersFilter.ScanDateFrom,
                computersFilter.ScanDateTo,
                computersFilter.ScrapDateFrom,
                computersFilter.ScrapDateTo,
                computersFilter.SearchFor,
                computersFilter.IsShowScrapped,
                computersFilter.RecordsOnPage);

            return computerOverviews;
        }

        #endregion

        #region Server

        public void AddServer(Server businessModel)
        {
            throw new NotImplementedException();
        }

        public void DeleteServer(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateWorkstation(Server businessModel)
        {
            throw new NotImplementedException();
        }

        public Server GetServer(int id)
        {
            return this.serverRepository.FindById(id);
        }

        public List<OperationServerLogOverview> GetOperationServerLogOverviews(int id, int customerId)
        {
            return this.operationLogRepository.GetOperationServerLogOverviews(customerId, id);
        }

        public List<ServerOverview> GetServers(ServersFilter computersFilter)
        {
            var models = this.serverRepository.FindOverviews(computersFilter.CustomerId, computersFilter.SearchFor);

            return models;
        }

        #endregion

        #region Printer

        public void AddPrinter(Printer businessModel)
        {
            throw new NotImplementedException();
        }

        public void DeletePrinter(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdatePrinter(Printer businessModel)
        {
            throw new NotImplementedException();
        }

        public Printer GetPrinter(int id)
        {
            return this.printerRepository.FindById(id);
        }

        public List<PrinterOverview> GetPrinters(PrintersFilter printersFilter)
        {
            var models = this.printerRepository.FindOverviews(
                printersFilter.CustomerId,
                printersFilter.DepartmentId,
                printersFilter.SearchFor);

            return models;
        }

        #endregion

        #region DynamicData

        public InventoryOverviewResponse GetInventory(int id)
        {
            var model = this.inventoryRepository.FindById(id);
            var dynamicData = this.inventoryTypePropertyValueRepository.GetData(id);

            var response = new InventoryOverviewResponse(model, dynamicData);
            return response;
        }

        public InventoriesOverviewResponse GetInventories(InventoriesFilter filter)
        {
            var models = this.inventoryRepository.FindOverviews(
                filter.InventoryTypeId,
                filter.DepartmentId,
                filter.SearchFor,
                filter.RecordsOnPage);

            var ids = models.Select(x => x.Id).ToList();

            var dynamicData = this.inventoryTypePropertyValueRepository.GetData(ids);

            var response = new InventoriesOverviewResponse(models, dynamicData);

            return response;
        }

        public InventoryOverviewResponseWithType GetConnectedToComputerInventories(int computerId)
        {
            var inventories = this.inventoryRepository.FindConnectedToComputerInventories(computerId);
            var inventoryIds = new List<int>();
            foreach (var item in inventories)
            {
                inventoryIds.AddRange(item.InventoryOverviews.Select(x => x.Id));
            }

            var dynamicData = this.inventoryTypePropertyValueRepository.GetData(inventoryIds);
            var inventoryResponse = new InventoryOverviewResponseWithType(inventories, dynamicData);

            return inventoryResponse;
        }

        public List<TypeGroupModel> GetTypeGroupModels(int inventoryTypeId)
        {
            return this.inventoryTypeGroupRepository.Find(inventoryTypeId);
        }

        public void ConnectInventoryToComputer(int inventoryId, int computerId)
        {
            this.computerInventoryRepository.Add(new ComputerInventory(computerId, inventoryId));
            this.computerInventoryRepository.Commit();
        }

        public void RemoveInventoryFromComputer(int inventoryId, int computerId)
        {
            this.computerInventoryRepository.DeleteById(computerId, inventoryId);
            this.computerInventoryRepository.Commit();
        }

        #endregion
    }
}