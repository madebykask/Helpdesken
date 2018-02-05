using DH.Helpdesk.BusinessData.Models.User.Input;

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
    using DH.Helpdesk.BusinessData.Models.Operation;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Computers;
    using DH.Helpdesk.Dal.Repositories.Inventory;
    using DH.Helpdesk.Dal.Repositories.Inventory.Concrete;
    using DH.Helpdesk.Dal.Repositories.Printers;
    using DH.Helpdesk.Dal.Repositories.Servers;
    using DH.Helpdesk.Dal.Repositories.WorkstationModules;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Inventory;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Inventory;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Inventory;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Inventory;
    using DH.Helpdesk.Services.Exceptions;
    using DH.Helpdesk.Services.Requests.Inventory;
    using DH.Helpdesk.Services.Response.Inventory;

    public class InventoryService : IInventoryService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

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

        private readonly IServerFieldSettingsRepository serverFieldSettingsRepository;

        private readonly IServerRestorer serverRestorer;

        private readonly IServerValidator serverValidator;

        private readonly IOperationObjectRepository operationObjectRepository;

        private readonly IOperationLogEMailLogRepository operationLogEMailLogRepository;

        private readonly IServerLogicalDriveRepository serverLogicalDriveRepository;

        private readonly IServerSoftwareRepository serverSoftwareRepository;

        private readonly IPrinterFieldSettingsRepository printerFieldSettingsRepository;

        private readonly IPrinterRestorer printerRestorer;

        private readonly IPrinterValidator printerValidator;

        private readonly IInventoryValidator inventoryValidator;

        private readonly IInventoryRestorer inventoryRestorer;

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
            ISoftwareRepository softwareRepository,
            IServerFieldSettingsRepository serverFieldSettingsRepository,
            IServerRestorer serverRestorer,
            IServerValidator serverValidator,
            IOperationObjectRepository operationObjectRepository,
            IOperationLogEMailLogRepository operationLogEMailLogRepository,
            IServerLogicalDriveRepository serverLogicalDriveRepository,
            IServerSoftwareRepository serverSoftwareRepository,
            IPrinterFieldSettingsRepository printerFieldSettingsRepository,
            IPrinterRestorer printerRestorer,
            IPrinterValidator printerValidator,
            IInventoryValidator inventoryValidator,
            IInventoryRestorer inventoryRestorer,
            IUnitOfWorkFactory unitOfWorkFactory)
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
            this.serverFieldSettingsRepository = serverFieldSettingsRepository;
            this.serverRestorer = serverRestorer;
            this.serverValidator = serverValidator;
            this.operationObjectRepository = operationObjectRepository;
            this.operationLogEMailLogRepository = operationLogEMailLogRepository;
            this.serverLogicalDriveRepository = serverLogicalDriveRepository;
            this.serverSoftwareRepository = serverSoftwareRepository;
            this.printerFieldSettingsRepository = printerFieldSettingsRepository;
            this.printerRestorer = printerRestorer;
            this.printerValidator = printerValidator;
            this.inventoryValidator = inventoryValidator;
            this.inventoryRestorer = inventoryRestorer;
            this.unitOfWorkFactory = unitOfWorkFactory;
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

        public ReportModelWithInventoryType GetAllConnectedInventory(
            int inventoryTypeId,
            int? departmentId,
            string searchFor)
        {
            return this.inventoryRepository.FindAllConnectedInventory(inventoryTypeId, departmentId, searchFor);
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

        public void AddWorkstation(ComputerForInsert businessModel, OperationContext context)
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

        public void UpdateWorkstation(ComputerForUpdate businessModel, OperationContext context)
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

        public ComputerForRead GetWorkstation(int id)
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
                computersFilter.RecordsOnPage,
                computersFilter.SortField,
                computersFilter.RecordsCount);

            return computerOverviews;
        }

        public int GetWorkstationIdByName(string computerName, int customerId)
        {
            return computerRepository.GetIdByName(computerName, customerId);
        }

        #endregion

        #region Server

        public void AddServer(ServerForInsert businessModel, OperationContext context)
        {
            bool isExist = this.operationObjectRepository.IsExist(businessModel.GeneralFields.Name);
            if (isExist)
            {
                throw new OperationObjectEditExeption(
                    string.Format(
                        "Server marked as operation object with the name {0} is alredy exist",
                        businessModel.GeneralFields.Name));
            }

            var settings = this.serverFieldSettingsRepository.GetFieldSettingsProcessing(context.CustomerId);
            this.serverValidator.Validate(businessModel, settings);
            this.serverRepository.Add(businessModel);
            this.serverRepository.Commit();

            var isOperationObject = businessModel.IsOperationObject;
            if (!isOperationObject)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(businessModel.GeneralFields.Name))
            {
                throw new OperationObjectEditExeption("Server can not be marked as operation object with empty name");
            }

            this.AddOperationObject(context, businessModel.GeneralFields.Name, businessModel.GeneralFields.Description);
        }

        public void DeleteServer(int id)
        {
            this.serverLogicalDriveRepository.DeleteByServerId(id);
            this.serverLogicalDriveRepository.Commit();

            this.serverSoftwareRepository.DeleteByServerId(id);
            this.serverSoftwareRepository.Commit();

            var operationObjectName = this.serverRepository.FindOperationObjectName(id);
            var isOperationObject = !string.IsNullOrWhiteSpace(operationObjectName);

            if (isOperationObject)
            {
                this.DeleteOperationObject(operationObjectName);
            }

            this.serverRepository.DeleteById(id);
            this.serverRepository.Commit();
        }

        public void UpdateServer(ServerForUpdate businessModel, OperationContext context)
        {
            var existingBusinessModel = this.serverRepository.FindById(businessModel.Id);

            var newBusinessModelName = businessModel.GeneralFields.Name;
            var oldBusinessModelName = existingBusinessModel.GeneralFields.Name;

            if (!oldBusinessModelName.Equals(newBusinessModelName))
            {
                bool isExist = this.operationObjectRepository.IsExist(businessModel.GeneralFields.Name);
                if (isExist)
                {
                    throw new OperationObjectEditExeption(
                        string.Format(
                            "Server marked as operation object with the name {0} is alredy exist",
                            businessModel.GeneralFields.Name));
                }
            }

            var isOperationObject = businessModel.IsOperationObject;
            var isWasOperationObject = existingBusinessModel.IsOperationObject;

            var settings = this.serverFieldSettingsRepository.GetFieldSettingsProcessing(context.CustomerId);
            this.serverRestorer.Restore(businessModel, existingBusinessModel, settings);
            this.serverValidator.Validate(businessModel, existingBusinessModel, settings);
            this.serverRepository.Update(businessModel);
            this.serverRepository.Commit();

            if (isOperationObject)
            {
                if (string.IsNullOrWhiteSpace(businessModel.GeneralFields.Name))
                {
                    throw new OperationObjectEditExeption(
                        "Server can not be marked as operation object with empty name");
                }

                if (isWasOperationObject)
                {
                    if (!oldBusinessModelName.Equals(newBusinessModelName))
                    {
                        this.UpdateOperationObject(
                            context,
                            newBusinessModelName,
                            businessModel.GeneralFields.Description,
                            oldBusinessModelName);
                    }
                }
                else
                {
                    this.AddOperationObject(context, newBusinessModelName, businessModel.GeneralFields.Description);
                }
            }
            else
            {
                if (isWasOperationObject)
                {
                    this.DeleteOperationObject(oldBusinessModelName);
                }
            }
        }

        public ServerForRead GetServer(int id)
        {
            return this.serverRepository.FindById(id);
        }

        public List<OperationServerLogOverview> GetOperationServerLogOverviews(int id, int customerId)
        {
            return this.operationLogRepository.GetOperationServerLogOverviews(customerId, id);
        }

        public ServerOverview[] GetServers(ServersFilter computersFilter)
        {
            using (var uow = this.unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                var repository = uow.GetRepository<DH.Helpdesk.Domain.Servers.Server>();
                var servers = repository.GetAll()
                    .Search(computersFilter.CustomerId, computersFilter.SearchFor, computersFilter.SortField);
                if (computersFilter.RecordsCount.HasValue)
                    servers = servers.OrderBy(x => x.ServerName).Take(computersFilter.RecordsCount.Value);
                var overviews = servers.MapToFullOverviews();
                return overviews;
            }
        }

        public int GetServerIdByName(string serverName, int customerId)
        {
            return serverRepository.GetIdByName(serverName, customerId);
        }

        #endregion

        #region Printer

        public void AddPrinter(PrinterForInsert businessModel, OperationContext context)
        {
            var settings = this.printerFieldSettingsRepository.GetFieldSettingsProcessing(context.CustomerId);
            this.printerValidator.Validate(businessModel, settings);
            this.printerRepository.Add(businessModel);
            this.printerRepository.Commit();
        }

        public void DeletePrinter(int id)
        {
            this.printerRepository.DeleteById(id);
            this.printerRepository.Commit();
        }

        public void UpdatePrinter(PrinterForUpdate businessModel, OperationContext context)
        {
            var existingBusinessModel = this.printerRepository.FindById(businessModel.Id);
            var settings = this.printerFieldSettingsRepository.GetFieldSettingsProcessing(context.CustomerId);
            this.printerRestorer.Restore(businessModel, existingBusinessModel, settings);
            this.printerValidator.Validate(businessModel, existingBusinessModel, settings);
            this.printerRepository.Update(businessModel);
            this.printerRepository.Commit();
        }

        public PrinterForRead GetPrinter(int id)
        {
            return this.printerRepository.FindById(id);
        }

        public List<PrinterOverview> GetPrinters(PrintersFilter printersFilter)
        {
            var models = this.printerRepository.FindOverviews(
                printersFilter.CustomerId,
                printersFilter.DepartmentId,
                printersFilter.SearchFor,
                printersFilter.RecordsCount);

            return models;
        }

        public int GetPrinterIdByName(string printerName, int customerId)
        {
            return printerRepository.GetIdByName(printerName, customerId);
        }

        #endregion

        #region DynamicData

        public void AddInventory(InventoryForInsert businessModel)
        {
            var settings = this.inventoryFieldSettingsRepository.GetFieldSettingsForProcessing(businessModel.InventoryTypeId);
            this.inventoryValidator.Validate(businessModel, settings);

            this.inventoryRepository.Add(businessModel);
            this.inventoryRepository.Commit();
        }

        public void AddDynamicFieldsValuesInventory(List<InventoryValueForWrite> dynamicBusinessModels)
        {
            this.inventoryTypePropertyValueRepository.Add(dynamicBusinessModels);
            this.inventoryTypePropertyValueRepository.Commit();
        }

        public void UpdateInventory(InventoryForUpdate businessModel, List<InventoryValueForWrite> dynamicBusinessModels, int inventoryTypeId)
        {
            var existingBusinessModel = this.inventoryRepository.FindById(businessModel.Id);
            var settings = this.inventoryFieldSettingsRepository.GetFieldSettingsForProcessing(inventoryTypeId);
            this.inventoryRestorer.Restore(businessModel, existingBusinessModel, settings);
            this.inventoryValidator.Validate(businessModel, existingBusinessModel, settings);

            this.inventoryRepository.Update(businessModel);
            this.inventoryRepository.Commit();
            this.inventoryTypePropertyValueRepository.Update(dynamicBusinessModels);
            this.inventoryTypePropertyValueRepository.Commit();
        }

        public void DeleteInventory(int id)
        {
            this.computerInventoryRepository.DeleteByInventoryId(id);
            this.computerInventoryRepository.Commit();

            this.inventoryTypePropertyValueRepository.DeleteByInventoryId(id);
            this.inventoryTypePropertyValueRepository.Commit();

            this.inventoryRepository.DeleteById(id);
            this.inventoryRepository.Commit();
        }

        public InventoryOverviewResponse GetInventory(int id)
        {
            var model = this.inventoryRepository.FindById(id);
            var dynamicData = this.inventoryTypePropertyValueRepository.GetData(id);

            var response = new InventoryOverviewResponse(model, dynamicData);
            return response;
        }

        public ComputerForRead GetWorkstationByNumber(string computerName, int customerId)
        {
            var pcId = computerRepository.GetIdByName(computerName, customerId);
            return pcId > 0 ? computerRepository.FindById(pcId) : null;
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

        public int GetCustomInventoryIdByName(string inventoryName, int inventoryTypeId)
        {
            return inventoryRepository.GetIdByName(inventoryName, inventoryTypeId);
        }

        public List<ComputerOverview> GetRelatedInventory(int customerId, string userId)
        {
            return this.computerRepository.GetRelatedOverviews(customerId, userId);
        }

        public List<int> GetRelatedCaseIds(CurrentModes inventoryType, int inventoryId, int customerId)
        {
            return inventoryRepository.GetRelatedCaseIds(inventoryType, inventoryId, customerId);
        }

        public ComputerShortOverview GetWorkstationShortInfo(int computerId)
        {
            return this.computerRepository.FindShortOverview(computerId);
        }

        #endregion

        #region Private

        private void AddUserHistory(int computerId, int connectingUserId)
        {
            var userGuid = this.computerUsersRepository.FindUserGuidById(connectingUserId);
            var computerHistory = new ComputerHistory(computerId, userGuid, DateTime.Now);
            this.computerHistoryRepository.Add(computerHistory);
            this.computerHistoryRepository.Commit();
        }

        private void AddOperationObject(OperationContext context, string newBusinessModelName, string newDescription)
        {
            var insertedOperationObject = new OperationObjectForInsert(
                context.CustomerId,
                context.DateAndTime,
                newBusinessModelName,
                newDescription);
            this.operationObjectRepository.Add(insertedOperationObject);
            this.operationObjectRepository.Commit();
        }

        private void DeleteOperationObject(string oldBusinessModelName)
        {
            OperationObjectForRead operationObject = this.operationObjectRepository.FindByName(oldBusinessModelName);
            int operationObjectId = operationObject.Id;

            List<int> operationLogIds = this.operationLogRepository.FindOperationObjectId(operationObjectId);

            this.operationLogEMailLogRepository.DeleteByOperationLogIds(operationLogIds);
            this.operationLogEMailLogRepository.Commit();

            this.operationLogRepository.DeleteByOperationObjectId(operationObjectId);
            this.operationLogRepository.Commit();

            this.operationObjectRepository.DeleteById(operationObjectId);
            this.operationObjectRepository.Commit();
        }

        private void UpdateOperationObject(
            OperationContext context,
            string newBusinessModelName,
            string newBusinessModelDescription,
            string oldBusinessModelName)
        {
            OperationObjectForRead operationObject = this.operationObjectRepository.FindByName(oldBusinessModelName);
            int operationObjectId = operationObject.Id;

            var updatedOperationObject = new OperationObjectForUpdate(
                operationObjectId,
                context.DateAndTime,
                newBusinessModelName,
                newBusinessModelDescription);
            this.operationObjectRepository.Update(updatedOperationObject);
            this.operationObjectRepository.Commit();
        }

        #endregion
    }
}