namespace DH.Helpdesk.Services.Services.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Inventory;
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
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Computers;
    using DH.Helpdesk.Dal.Repositories.Inventory;
    using DH.Helpdesk.Dal.Repositories.Inventory.Concrete;
    using DH.Helpdesk.Dal.Repositories.Printers;
    using DH.Helpdesk.Dal.Repositories.Servers;
    using DH.Helpdesk.Dal.Repositories.WorkstationModules;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Inventory;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Inventory;
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

        private readonly IInventoryFieldSettingsRepository inventoryFieldSettingsRepository;

        private readonly IInventoryDynamicFieldSettingsRepository inventoryDynamicFieldSettingsRepository;

        private readonly IComputerUsersRepository computerUsersRepository;

        private readonly IComputerRestorer computerRestorer;

        private readonly IComputerValidator computerValidator;

        private readonly IComputerFieldSettingsRepository computerFieldSettingsRepository;

        private readonly IComputerHistoryRepository computerHistoryRepository;

        private readonly ILogicalDriveRepository logicalDriveRepository;

        private readonly ISoftwareRepository softwareRepository;

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
            InventoryTypeGroupRepository inventoryTypeGroupRepository,
            IInventoryFieldSettingsRepository inventoryFieldSettingsRepository,
            IInventoryDynamicFieldSettingsRepository inventoryDynamicFieldSettingsRepository,
            IComputerUsersRepository computerUsersRepository,
            IComputerRestorer computerRestorer,
            IComputerValidator computerValidator,
            IComputerFieldSettingsRepository computerFieldSettingsRepository,
            IComputerHistoryRepository computerHistoryRepository,
            ILogicalDriveRepository logicalDriveRepository,
            ISoftwareRepository softwareRepository)
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
            this.inventoryFieldSettingsRepository = inventoryFieldSettingsRepository;
            this.inventoryDynamicFieldSettingsRepository = inventoryDynamicFieldSettingsRepository;
            this.computerUsersRepository = computerUsersRepository;
            this.computerRestorer = computerRestorer;
            this.computerValidator = computerValidator;
            this.computerFieldSettingsRepository = computerFieldSettingsRepository;
            this.computerHistoryRepository = computerHistoryRepository;
            this.logicalDriveRepository = logicalDriveRepository;
            this.softwareRepository = softwareRepository;
        }

        public List<ComputerUserOverview> GetComputerUsers(int customerId, string searchFor)
        {
            return this.computerUsersRepository.GetOverviews(customerId, searchFor);
        }

        public List<ComputerUserOverview> GetComputerUserHistory(int computerId)
        {
            return this.computerUsersRepository.GetConnectedToComputerOverviews(computerId);
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

        public void DeleteInventoryType(int id)
        {
            this.computerInventoryRepository.DeleteByInventoryTypeId(id);
            this.computerInventoryRepository.Commit();

            this.inventoryTypePropertyValueRepository.DeleteByInventoryTypeId(id);
            this.inventoryTypePropertyValueRepository.Commit();

            this.inventoryFieldSettingsRepository.DeleteByInventoryTypeId(id);
            this.inventoryFieldSettingsRepository.Commit();

            this.inventoryDynamicFieldSettingsRepository.DeleteByInventoryTypeId(id);
            this.inventoryDynamicFieldSettingsRepository.Commit();

            this.inventoryRepository.DeleteByInventoryTypeId(id);
            this.inventoryRepository.Commit();

            this.inventoryTypeGroupRepository.DeleteByInventoryTypeId(id);
            this.inventoryTypeGroupRepository.Commit();

            this.inventoryTypeRepository.Delete(id);
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

        public void AddWorkstation(Computer businessModel, OperationContext context)
        {
            var settings = this.computerFieldSettingsRepository.GetFieldSettingsProcessing(context.CustomerId);

            this.computerValidator.Validate(businessModel, settings);
            this.computerRepository.Add(businessModel);
            this.computerRepository.Commit();

            if (businessModel.ContactInformationFields.UserId.HasValue)
            {
                this.AddUserHistory(businessModel.Id, businessModel.ContactInformationFields.UserId.Value);
            }
        }

        public void DeleteWorkstation(int id)
        {
            this.computerHistoryRepository.DeleteByComputerId(id);
            this.computerRepository.Commit();

            this.computerLogRepository.DeleteByComputerId(id);
            this.computerLogRepository.Commit();

            this.computerInventoryRepository.DeleteByComputerId(id);
            this.computerInventoryRepository.Commit();

            this.logicalDriveRepository.DeleteByComputerId(id);
            this.logicalDriveRepository.Commit();

            this.softwareRepository.DeleteByComputerId(id);
            this.softwareRepository.Commit();

            this.computerRepository.DeleteById(id);
            this.computerRepository.Commit();
        }

        public void UpdateWorkstation(Computer businessModel, OperationContext context)
        {
            var existingBusinessModel = this.computerRepository.FindById(businessModel.Id);
            var settings = this.computerFieldSettingsRepository.GetFieldSettingsProcessing(context.CustomerId);
            this.computerRestorer.Restore(businessModel, existingBusinessModel, settings);
            this.computerValidator.Validate(businessModel, existingBusinessModel, settings);
            this.computerRepository.Update(businessModel);
            this.computerRepository.Commit();

            if (businessModel.ContactInformationFields.UserId.HasValue
                && (businessModel.ContactInformationFields.UserId.Value
                    != existingBusinessModel.ContactInformationFields.UserId))
            {
                this.AddUserHistory(businessModel.Id, businessModel.ContactInformationFields.UserId.Value);
            }
        }

        public void UpdateWorkstationInfo(int id, string info)
        {
            this.computerRepository.UpdateInfo(id, info);
            this.computerRepository.Commit();
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

        public ComputerShortOverview GetWorkstationShortInfo(int computerId)
        {
            return this.computerRepository.FindShortOverview(computerId);
        }

        private void AddUserHistory(int computerId, int connectingUserId)
        {
            var userGuid = this.computerUsersRepository.FindUserGuidById(connectingUserId);
            var computerHistory = new ComputerHistory(computerId, userGuid, DateTime.Now);
            this.computerHistoryRepository.Add(computerHistory);
            this.computerHistoryRepository.Commit();
        }
        #endregion
    }
}