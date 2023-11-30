using System.Globalization;
using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings;
using DH.Helpdesk.BusinessData.Models.User.Input;
using DH.Helpdesk.Domain.Computers;

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
        #region Fields

        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IInventoryTypeRepository _inventoryTypeRepository;
        private readonly IComputerRepository _computerRepository;
        private readonly IServerRepository _serverRepository;
        private readonly IPrinterRepository _printerRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IInventoryTypePropertyValueRepository _inventoryTypePropertyValueRepository;
        private readonly IComputerLogRepository _computerLogRepository;
        private readonly IComputerInventoryRepository _computerInventoryRepository;
        private readonly IOperationLogRepository _operationLogRepository;
        private readonly InventoryTypeGroupRepository _inventoryTypeGroupRepository;
        private readonly IInventoryFieldSettingsRepository _inventoryFieldSettingsRepository;
        private readonly IInventoryDynamicFieldSettingsRepository _inventoryDynamicFieldSettingsRepository;
        private readonly IComputerUsersRepository _computerUsersRepository;
        private readonly IComputerRestorer _computerRestorer;
        private readonly IComputerStatusRepository _computerStatusRepository;
        private readonly IComputerValidator _computerValidator;
        private readonly IComputerFieldSettingsRepository _computerFieldSettingsRepository;
        private readonly IComputerHistoryRepository _computerHistoryRepository;
        private readonly ILogicalDriveRepository _logicalDriveRepository;
        private readonly ISoftwareRepository _softwareRepository;
        private readonly IServerFieldSettingsRepository _serverFieldSettingsRepository;
        private readonly IServerRestorer _serverRestorer;
        private readonly IServerValidator _serverValidator;
        private readonly IOperationObjectRepository _operationObjectRepository;
        private readonly IOperationLogEMailLogRepository _operationLogEMailLogRepository;
        private readonly IServerLogicalDriveRepository _serverLogicalDriveRepository;
        private readonly IServerSoftwareRepository _serverSoftwareRepository;
        private readonly IPrinterFieldSettingsRepository _printerFieldSettingsRepository;
        private readonly IPrinterRestorer _printerRestorer;
        private readonly IPrinterValidator _printerValidator;
        private readonly IInventoryValidator _inventoryValidator;
        private readonly IInventoryRestorer _inventoryRestorer;
        private readonly IInventoryTypeStandardSettingsRepository _inventoryTypeStandardSettingsRepository;

        #endregion

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
            IInventoryTypeStandardSettingsRepository inventoryTypeStandardSettingsRepository,
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
            IUnitOfWorkFactory unitOfWorkFactory, IComputerStatusRepository computerStatusRepository)
        {
            _inventoryTypeStandardSettingsRepository = inventoryTypeStandardSettingsRepository;
            _inventoryTypeRepository = inventoryTypeRepository;
            _computerRepository = computerRepository;
            _serverRepository = serverRepository;
            _printerRepository = printerRepository;
            _inventoryRepository = inventoryRepository;
            _inventoryTypePropertyValueRepository = inventoryTypePropertyValueRepository;
            _computerLogRepository = computerLogRepository;
            _computerInventoryRepository = computerInventoryRepository;
            _operationLogRepository = operationLogRepository;
            _inventoryTypeGroupRepository = inventoryTypeGroupRepository;
            _inventoryFieldSettingsRepository = inventoryFieldSettingsRepository;
            _inventoryDynamicFieldSettingsRepository = inventoryDynamicFieldSettingsRepository;
            _computerUsersRepository = computerUsersRepository;
            _computerRestorer = computerRestorer;
            _computerValidator = computerValidator;
            _computerFieldSettingsRepository = computerFieldSettingsRepository;
            _computerHistoryRepository = computerHistoryRepository;
            _logicalDriveRepository = logicalDriveRepository;
            _softwareRepository = softwareRepository;
            _serverFieldSettingsRepository = serverFieldSettingsRepository;
            _serverRestorer = serverRestorer;
            _serverValidator = serverValidator;
            _operationObjectRepository = operationObjectRepository;
            _operationLogEMailLogRepository = operationLogEMailLogRepository;
            _serverLogicalDriveRepository = serverLogicalDriveRepository;
            _serverSoftwareRepository = serverSoftwareRepository;
            _printerFieldSettingsRepository = printerFieldSettingsRepository;
            _printerRestorer = printerRestorer;
            _printerValidator = printerValidator;
            _inventoryValidator = inventoryValidator;
            _inventoryRestorer = inventoryRestorer;
            _unitOfWorkFactory = unitOfWorkFactory;
            _computerStatusRepository = computerStatusRepository;
        }

        public List<ComputerUserOverview> GetComputerUsers(int customerId, string searchFor)
        {
            return _computerUsersRepository.GetOverviews(customerId, searchFor);
        }

        public List<ComputerUserOverview> GetComputerUserHistory(int computerId)
        {
            return _computerUsersRepository.GetConnectedToComputerOverviews(computerId);
        }

        public void AddInventoryType(InventoryType businessModel)
        {
            _inventoryTypeRepository.Add(businessModel);
            _inventoryTypeRepository.Commit();
        }

        public void UpdateInventoryType(InventoryType businessModel)
        {
            _inventoryTypeRepository.Update(businessModel);
            _inventoryTypeRepository.Commit();
        }

        public void UpdateStandardInventoryTypeSettings(int customerId, CurrentModes modes, bool isActive)
        {
            var settings = _inventoryTypeStandardSettingsRepository.GetCustomerSettings(customerId);
            if (settings != null)
            {
                switch (modes)
                {
                    case CurrentModes.Workstations:
                        settings.ShowWorkstations = isActive;
                    break;

                    case CurrentModes.Printers:
                        settings.ShowPrinters = isActive;
                    break;

                    case CurrentModes.Servers:
                        settings.ShowServers = isActive;
                    break;
                }
            }

            _inventoryTypeStandardSettingsRepository.Commit();
        }

        public void DeleteInventoryType(int id)
        {
            _computerInventoryRepository.DeleteByInventoryTypeId(id);
            _computerInventoryRepository.Commit();

            _inventoryTypePropertyValueRepository.DeleteByInventoryTypeId(id);
            _inventoryTypePropertyValueRepository.Commit();

            _inventoryFieldSettingsRepository.DeleteByInventoryTypeId(id);
            _inventoryFieldSettingsRepository.Commit();

            _inventoryDynamicFieldSettingsRepository.DeleteByInventoryTypeId(id);
            _inventoryDynamicFieldSettingsRepository.Commit();

            _inventoryRepository.DeleteByInventoryTypeId(id);
            _inventoryRepository.Commit();

            _inventoryTypeGroupRepository.DeleteByInventoryTypeId(id);
            _inventoryTypeGroupRepository.Commit();

            _inventoryTypeRepository.Delete(id);
            _inventoryTypeRepository.Commit();
        }

        public InventoryType GetInventoryType(int id)
        {
            return _inventoryTypeRepository.FindById(id);
        }

        #region GetInventoryTypes

        public List<ItemOverview> GetInventoryTypes(int customerId)
        {
            return _inventoryTypeRepository.FindOverviews(customerId);
        }

        public List<ItemOverview> GetInventoryTypes(int customerId, bool includeStandard, ItemOverview separatorItem = null)
        {
            var items = new List<ItemOverview>();
            var customInventoryTpes = _inventoryTypeRepository.FindOverviews(customerId);
            if (includeStandard)
            {
                var standardInventoryTypes = GetStandardInvetoryTypes(customerId);
                
                //add separator item only if standard items exist
                if (standardInventoryTypes.Any() && separatorItem != null)
                {
                    standardInventoryTypes.Add(separatorItem);
                }

                items.AddRange(standardInventoryTypes);
            }

            items.AddRange(customInventoryTpes);
            return items;
        }

        public IList<InventoryTypeOverview> GetInventoryTypesWithSettings(int customerId)
        {
            var inventoryTypes = new List<InventoryTypeOverview>();

            var activeInventoryStandardTypeIds =
                GetActiveInventoryStandardTypes(customerId).Select(x => (int)x).ToList();

            // 1. add fixed/standard inventory types with active settings
            var standardInventoryTypes = GetStandardInvetoryTypes(customerId, false).Select(t => new InventoryTypeOverview()
            {
                Name = t.Name,
                Value = Int32.Parse(t.Value),
                IsStandard = true,
                IsActive = activeInventoryStandardTypeIds.Contains(Int32.Parse(t.Value))
            });

            inventoryTypes.AddRange(standardInventoryTypes);

            // 2.add user defined inventory types
            var customInventoryTypes = 
                _inventoryTypeRepository.FindOverviews(customerId).Select(t => new InventoryTypeOverview()
                {
                    Name = t.Name,
                    Value = Int32.Parse(t.Value),
                    IsStandard = false,
                    //IsActive //not used since makes sense only for standard types
                });
            
            inventoryTypes.AddRange(customInventoryTypes);

            return inventoryTypes;
        }

        private List<ItemOverview> GetStandardInvetoryTypes(int customerId, bool activeOnly = true)
        {
            var activeTypes = activeOnly ? GetActiveInventoryStandardTypes(customerId) : new List<CurrentModes>();
            
            //select standard types based on settings
            var inventoryTypes = 
                Enum.GetValues(typeof(CurrentModes))
                    .Cast<CurrentModes>()
                    .Where(d => d < 0 && (!activeOnly || activeTypes.Contains(d)))
                    .Select(d => new 
                    {
                        Value = Convert.ToInt32(d).ToString(CultureInfo.InvariantCulture),
                        Name = GetInventoryTypeCaption(d)
                    }).OrderBy(d => d.Name).ToList();

            var inventoryTypeList = 
                inventoryTypes.Select(x => new ItemOverview(x.Name, x.Value)).ToList();
            
            return inventoryTypeList;
        }

        private List<CurrentModes> GetActiveInventoryStandardTypes(int customerId)
        {
            var inventoryTypes = new List<CurrentModes>();
            
            //inventoryTypes
            var settings = _inventoryTypeStandardSettingsRepository.GetCustomerSettings(customerId);
            if (settings != null)
            {
                if (settings.ShowPrinters)
                    inventoryTypes.Add(CurrentModes.Printers);

                if (settings.ShowServers)
                    inventoryTypes.Add(CurrentModes.Servers);

                if (settings.ShowWorkstations)
                    inventoryTypes.Add(CurrentModes.Workstations);
            }

            return inventoryTypes;
        }

        private string GetInventoryTypeCaption(CurrentModes mode)
        {
            switch (mode)
            {
                case CurrentModes.Workstations:
                    return "Arbetsstationer";

                case CurrentModes.Servers:
                    return "Server";

                case CurrentModes.Printers:
                    return "Skrivare";

                default:
                    return string.Empty;
            }
        }

        #endregion

        public List<ItemOverview> GetNotConnectedInventory(int inventoryType, int computerId)
        {
            return _inventoryRepository.FindNotConnectedOverviews(inventoryType, computerId);
        }

        public List<InventoryReportModel> GetInventoryCounts(int customerId, int? departmentId)
        {
            var workstationCount = 
                new InventoryReportModel(CurrentModes.Workstations.ToString(), 
                                         _computerRepository.GetComputerCount(customerId, departmentId));

            var serverCount = 
                new InventoryReportModel(CurrentModes.Servers.ToString(), 
                                         _serverRepository.GetServerCount(customerId));

            var printerCount = 
                new InventoryReportModel(CurrentModes.Printers.ToString(),
                                         _printerRepository.GetPrinterCount(customerId, departmentId));

            var inventoryCounts = _inventoryTypeRepository.FindInventoriesWithCount(customerId, departmentId);
            var models = new [] { workstationCount, serverCount, printerCount }.ToList();
            models.AddRange(inventoryCounts);

            return models;
        }

        public ReportModelWithInventoryType GetAllConnectedInventory(
            int inventoryTypeId,
            int? departmentId,
            string searchFor)
        {
            return _inventoryRepository.FindAllConnectedInventory(inventoryTypeId, departmentId, searchFor);
        }

        #region Workstation

        public void AddComputerLog(ComputerLog businessModel)
        {
            _computerLogRepository.Add(businessModel);
            _computerLogRepository.Commit();
        }

        public void DeleteComputerLog(int id)
        {
            _computerLogRepository.DeleteById(id);
            _computerLogRepository.Commit();
        }

        public void AddWorkstation(ComputerForInsert businessModel, OperationContext context)
        {
            var settings = _computerFieldSettingsRepository.GetFieldSettingsProcessing(context.CustomerId);
            _computerValidator.Validate(businessModel, settings);
            _computerRepository.Add(businessModel);
            _computerRepository.Commit();
            
            if (businessModel.ContactInformationFields.UserId.HasValue)
            {
                this.AddUserHistory(businessModel.Id, businessModel.ContactInformationFields.UserId.Value);
            }
        }

        public void DeleteWorkstation(int id)
        {
            _computerHistoryRepository.DeleteByComputerId(id);
            _computerRepository.Commit();

            _computerLogRepository.DeleteByComputerId(id);
            _computerLogRepository.Commit();

            _computerInventoryRepository.DeleteByComputerId(id);
            _computerInventoryRepository.Commit();

            _logicalDriveRepository.DeleteByComputerId(id);
            _logicalDriveRepository.Commit();

            _softwareRepository.DeleteByComputerId(id);
            _softwareRepository.Commit();

            _computerRepository.DeleteById(id);
            _computerRepository.Commit();
        }

        public void UpdateWorkstation(ComputerForUpdate businessModel, OperationContext context)
        {
            var existingBusinessModel = _computerRepository.FindById(businessModel.Id);
            var settings = _computerFieldSettingsRepository.GetFieldSettingsProcessing(context.CustomerId);
            _computerRestorer.Restore(businessModel, existingBusinessModel, settings);
            _computerValidator.Validate(businessModel, existingBusinessModel, settings);
            _computerRepository.Update(businessModel);
            _computerRepository.Commit();

            if (businessModel.ContactInformationFields.UserId.HasValue
                && (businessModel.ContactInformationFields.UserId.Value
                    != existingBusinessModel.ContactInformationFields.UserId))
            {
                this.AddUserHistory(businessModel.Id, businessModel.ContactInformationFields.UserId.Value);
            }
        }

        public bool IsMacAddressUnique(int exceptId, string macAddress)
        {
            return _computerRepository.IsMacAddressUnique(exceptId, macAddress.Trim());
        }
        public bool IsTheftMarkUnique(int exceptId, string theftMark)
        {
            return _computerRepository.IsTheftMarkUnique(exceptId, theftMark.Trim());
        }
        public bool IsComputerNameUnique(int exceptId, string computerName)
        {
            return _computerRepository.IsComputerNameUnique(exceptId, computerName.Trim());
        }
        public void UpdateWorkstationInfo(int id, string info)
        {
            _computerRepository.UpdateInfo(id, info);
            _computerRepository.Commit();
        }

        public ComputerForRead GetWorkstation(int id)
        {
            return _computerRepository.FindById(id);
        }

        public ComputerFile GetWorkstationFile(int id)
        {
            return _computerRepository.GetFile(id);
        }

        public void SaveWorkstationFile(int id, string fileName, byte[] data)
        {
            _computerRepository.SaveFile(id, fileName, data);
        }

        public void DeleteWorkstationFile(int id)
        {
            _computerRepository.DeleteFile(id);
        }

        public int? GetComputerTypePrice(int id)
        {
            return _computerRepository.GetComputerTypeById(id);
        }

        public List<ComputerLogOverview> GetWorkstationLogOverviews(int id)
        {
            return _computerLogRepository.Find(id);
        }

        public List<ComputerOverview> GetWorkstations(ComputersFilter computersFilter, bool isComputerDepartmentSource)
        {
            var computerOverviews = _computerRepository.FindOverviews(
                computersFilter.CustomerId,
                computersFilter.DomainId,
                computersFilter.DepartmentId,
                computersFilter.RegionId,
                computersFilter.UnitId,
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
                computersFilter.RecordsCount,
                isComputerDepartmentSource);

            return computerOverviews;
        }

        public int GetWorkstationIdByName(string computerName, int customerId)
        {
            return _computerRepository.GetIdByName(computerName, customerId);
        }

        public List<ItemOverview> GetComputerContractStatuses(int customerId)
        {
            return _computerStatusRepository.GetByCustomer(customerId)
                .Where(cs => cs.Type == ComputerStatusType.Contract)
                .Select(cs => new ItemOverview
                {
                    Name = cs.Name,
                    Value = cs.Id.ToString()
                })
                .OrderBy(cs => cs.Name)
                .ToList();
        }

        public List<ItemOverview> GetWorkstationStatuses(int customerId)
        {
            return _computerStatusRepository.GetByCustomer(customerId)
                .Where(cs => cs.Type == ComputerStatusType.Computer)
                .Select(cs => new ItemOverview
                {
                    Name = cs.Name,
                    Value = cs.Id.ToString()
                })
                .OrderBy(cs => cs.Name)
                .ToList();
        }

        public List<ComputerStatus> GetFullComputerStatuses(int customerId)
        {
            return _computerStatusRepository.GetByCustomer(customerId)
                .OrderBy(cs => cs.Name)
                .ToList();
        }

        public void SaveComputerStatus(ComputerStatus newCustomerStatus, out IDictionary<string, string> errors)
        {
            if (newCustomerStatus == null)
                throw new ArgumentNullException("newCustomerStatus");

            errors = new Dictionary<string, string>();
            
            if (string.IsNullOrEmpty(newCustomerStatus.Name))
                errors.Add("newCustomerStatus.Name", "Du måste ange en avslutsorsak");
            newCustomerStatus.CreatedDate = newCustomerStatus.ChangedDate = DateTime.UtcNow;
            if (newCustomerStatus.Id == 0)
            {
                newCustomerStatus.Id = _computerStatusRepository.GetLastId() + 1;
                _computerStatusRepository.Add(newCustomerStatus);
            }
            //else
            //    _computerStatusRepository.Update(newCustomerStatus);

            if (errors.Count == 0)
                _computerStatusRepository.Commit();
        }

        #endregion

        #region Server

        public void AddServer(ServerForInsert businessModel, OperationContext context)
        {
            bool isExist = _operationObjectRepository.IsExist(businessModel.GeneralFields.Name);
            if (isExist)
            {
                throw new OperationObjectEditExeption(
                    string.Format(
                        "Server marked as operation object with the name {0} is alredy exist",
                        businessModel.GeneralFields.Name));
            }

            var settings = _serverFieldSettingsRepository.GetFieldSettingsProcessing(context.CustomerId);
            _serverValidator.Validate(businessModel, settings);
            _serverRepository.Add(businessModel);
            _serverRepository.Commit();

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
            _serverLogicalDriveRepository.DeleteByServerId(id);
            _serverLogicalDriveRepository.Commit();

            _serverSoftwareRepository.DeleteByServerId(id);
            _serverSoftwareRepository.Commit();

            var operationObjectName = _serverRepository.FindOperationObjectName(id);
            var isOperationObject = !string.IsNullOrWhiteSpace(operationObjectName);

            if (isOperationObject)
            {
                this.DeleteOperationObject(operationObjectName);
            }

            _serverRepository.DeleteById(id);
            _serverRepository.Commit();
        }

        public void UpdateServer(ServerForUpdate businessModel, OperationContext context)
        {
            var existingBusinessModel = _serverRepository.FindById(businessModel.Id);

            var newBusinessModelName = businessModel.GeneralFields.Name;
            var oldBusinessModelName = existingBusinessModel.GeneralFields.Name;

            if (!oldBusinessModelName.Equals(newBusinessModelName))
            {
                bool isExist = _operationObjectRepository.IsExist(businessModel.GeneralFields.Name);
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

            var settings = _serverFieldSettingsRepository.GetFieldSettingsProcessing(context.CustomerId);
            _serverRestorer.Restore(businessModel, existingBusinessModel, settings);
            _serverValidator.Validate(businessModel, existingBusinessModel, settings);
            _serverRepository.Update(businessModel);
            _serverRepository.Commit();

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

        public DocumentFile GetServerFile(int id)
        {
            return _serverRepository.GetFile(id);
        }

        public void SaveServerFile(int id, string fileName, byte[] data)
        {
            _serverRepository.SaveFile(id, fileName, data);
        }

        public void DeleteServerFile(int id)
        {
            _serverRepository.DeleteFile(id);
        }

        public ServerForRead GetServer(int id)
        {
            return _serverRepository.FindById(id);
        }

        public List<OperationServerLogOverview> GetOperationServerLogOverviews(int id, int customerId)
        {
            return _operationLogRepository.GetOperationServerLogOverviews(customerId, id);
        }

        public ServerOverview[] GetServers(ServersFilter computersFilter)
        {
            using (var uow = _unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                var repository = uow.GetRepository<Domain.Servers.Server>();

                var servers = 
                    repository.GetAll().Search(computersFilter.CustomerId, computersFilter.SearchFor, computersFilter.SortField);

                if (computersFilter.RecordsCount.HasValue)
                    servers = servers.Take(computersFilter.RecordsCount.Value);

                var overviews = servers.MapToFullOverviews();
                return overviews;
            }
        }

        public int GetServerIdByName(string serverName, int customerId)
        {
            return _serverRepository.GetIdByName(serverName, customerId);
        }

        #endregion

        #region Printer

        public void AddPrinter(PrinterForInsert businessModel, OperationContext context)
        {
            var settings = _printerFieldSettingsRepository.GetFieldSettingsProcessing(context.CustomerId);
            _printerValidator.Validate(businessModel, settings);
            _printerRepository.Add(businessModel);
            _printerRepository.Commit();
        }

        public void DeletePrinter(int id)
        {
            _printerRepository.DeleteById(id);
            _printerRepository.Commit();
        }

        public void UpdatePrinter(PrinterForUpdate businessModel, OperationContext context)
        {
            var existingBusinessModel = _printerRepository.FindById(businessModel.Id);
            var settings = _printerFieldSettingsRepository.GetFieldSettingsProcessing(context.CustomerId);
            _printerRestorer.Restore(businessModel, existingBusinessModel, settings);
            _printerValidator.Validate(businessModel, existingBusinessModel, settings);
            _printerRepository.Update(businessModel);
            _printerRepository.Commit();
        }

        public PrinterForRead GetPrinter(int id)
        {
            return _printerRepository.FindById(id);
        }

        public List<PrinterOverview> GetPrinters(PrintersFilter printersFilter)
        {
            var models = _printerRepository.FindOverviews(
                printersFilter.CustomerId,
                printersFilter.DepartmentId,
                printersFilter.SearchFor,
                printersFilter.RecordsCount);

            return models;
        }

        public int GetPrinterIdByName(string printerName, int customerId)
        {
            return _printerRepository.GetIdByName(printerName, customerId);
        }

        #endregion

        #region DynamicData

        public void AddInventory(InventoryForInsert businessModel)
        {
            var settings = _inventoryFieldSettingsRepository.GetFieldSettingsForProcessing(businessModel.InventoryTypeId);
            _inventoryValidator.Validate(businessModel, settings);

            _inventoryRepository.Add(businessModel);
            _inventoryRepository.Commit();
        }

        public void AddDynamicFieldsValuesInventory(List<InventoryValueForWrite> dynamicBusinessModels)
        {
            _inventoryTypePropertyValueRepository.Add(dynamicBusinessModels);
            _inventoryTypePropertyValueRepository.Commit();
        }

        public void UpdateInventory(InventoryForUpdate businessModel, List<InventoryValueForWrite> dynamicBusinessModels, int inventoryTypeId)
        {
            var existingBusinessModel = _inventoryRepository.FindById(businessModel.Id);
            var settings = _inventoryFieldSettingsRepository.GetFieldSettingsForProcessing(inventoryTypeId);
            _inventoryRestorer.Restore(businessModel, existingBusinessModel, settings);
            _inventoryValidator.Validate(businessModel, existingBusinessModel, settings);

            _inventoryRepository.Update(businessModel);
            _inventoryRepository.Commit();
            _inventoryTypePropertyValueRepository.Update(dynamicBusinessModels);
            _inventoryTypePropertyValueRepository.Commit();
        }

        public void DeleteInventory(int id)
        {
            _computerInventoryRepository.DeleteByInventoryId(id);
            _computerInventoryRepository.Commit();

            _inventoryTypePropertyValueRepository.DeleteByInventoryId(id);
            _inventoryTypePropertyValueRepository.Commit();

            _inventoryRepository.DeleteById(id);
            _inventoryRepository.Commit();
        }

        public InventoryOverviewResponse GetInventory(int id)
        {
            var model = _inventoryRepository.FindById(id);
            var dynamicData = _inventoryTypePropertyValueRepository.GetData(id);

            var response = new InventoryOverviewResponse(model, dynamicData);
            return response;
        }

        public ComputerForRead GetWorkstationByNumber(string computerName, int customerId)
        {
            var pcId = _computerRepository.GetIdByName(computerName, customerId);
            return pcId > 0 ? _computerRepository.FindById(pcId) : null;
        }

        public InventoriesOverviewResponse GetInventories(InventoriesFilter filter)
        {
            var models = _inventoryRepository.FindOverviews(
                filter.InventoryTypeId,
                filter.DepartmentId,
                filter.SearchFor,
                filter.RecordsOnPage);

            var ids = models.Select(x => x.Id).ToList();

            var dynamicData = _inventoryTypePropertyValueRepository.GetData(ids, filter.InventoryTypeId);

            var response = new InventoriesOverviewResponse(models, dynamicData);

            return response;
        }

        public InventoryOverviewResponseWithType GetConnectedToComputerInventories(int computerId)
        {
            var inventories = _inventoryRepository.FindConnectedToComputerInventories(computerId);
            var inventoryIds = new List<int>();
            foreach (var item in inventories)
            {
                inventoryIds.AddRange(item.InventoryOverviews.Select(x => x.Id));
            }

            var dynamicData = _inventoryTypePropertyValueRepository.GetData(inventoryIds);
            var inventoryResponse = new InventoryOverviewResponseWithType(inventories, dynamicData);

            return inventoryResponse;
        }

        public List<TypeGroupModel> GetTypeGroupModels(int inventoryTypeId)
        {
            return _inventoryTypeGroupRepository.Find(inventoryTypeId);
        }

        public void ConnectInventoryToComputer(int inventoryId, int computerId)
        {
            _computerInventoryRepository.Add(new ComputerInventory(computerId, inventoryId));
            _computerInventoryRepository.Commit();
        }

        public void RemoveInventoryFromComputer(int inventoryId, int computerId)
        {
            _computerInventoryRepository.DeleteById(computerId, inventoryId);
            _computerInventoryRepository.Commit();
        }

        public int GetCustomInventoryIdByName(string inventoryName, int inventoryTypeId)
        {
            return _inventoryRepository.GetIdByName(inventoryName, inventoryTypeId);
        }

        public List<ComputerOverview> GetRelatedInventory(int customerId, string userId)
        {
            return _computerRepository.GetRelatedOverviews(customerId, userId);
        }

        public List<int> GetRelatedCaseIds(CurrentModes inventoryType, int inventoryId, int customerId)
        {
            return _inventoryRepository.GetRelatedCaseIds(inventoryType, inventoryId, customerId);
        }

        public ComputerShortOverview GetWorkstationShortInfo(int computerId)
        {
            return _computerRepository.FindShortOverview(computerId);
        }

        #endregion

        #region Private

        private void AddUserHistory(int computerId, int connectingUserId)
        {
            var userGuid = _computerUsersRepository.FindUserGuidById(connectingUserId);
            var computerHistory = new ComputerHistory(computerId, userGuid, DateTime.Now);
            _computerHistoryRepository.Add(computerHistory);
            _computerHistoryRepository.Commit();
        }

        private void AddOperationObject(OperationContext context, string newBusinessModelName, string newDescription)
        {
            var insertedOperationObject = new OperationObjectForInsert(
                context.CustomerId,
                context.DateAndTime,
                newBusinessModelName,
                newDescription);
            _operationObjectRepository.Add(insertedOperationObject);
            _operationObjectRepository.Commit();
        }

        private void DeleteOperationObject(string oldBusinessModelName)
        {
            OperationObjectForRead operationObject = _operationObjectRepository.FindByName(oldBusinessModelName);
            int operationObjectId = operationObject.Id;

            List<int> operationLogIds = _operationLogRepository.FindOperationObjectId(operationObjectId);

            _operationLogEMailLogRepository.DeleteByOperationLogIds(operationLogIds);
            _operationLogEMailLogRepository.Commit();

            _operationLogRepository.DeleteByOperationObjectId(operationObjectId);
            _operationLogRepository.Commit();

            _operationObjectRepository.DeleteById(operationObjectId);
            _operationObjectRepository.Commit();
        }

        private void UpdateOperationObject(
            OperationContext context,
            string newBusinessModelName,
            string newBusinessModelDescription,
            string oldBusinessModelName)
        {
            OperationObjectForRead operationObject = _operationObjectRepository.FindByName(oldBusinessModelName);
            int operationObjectId = operationObject.Id;

            var updatedOperationObject = new OperationObjectForUpdate(
                operationObjectId,
                context.DateAndTime,
                newBusinessModelName,
                newBusinessModelDescription);
            _operationObjectRepository.Update(updatedOperationObject);
            _operationObjectRepository.Commit();
        }

       

        #endregion
    }
}