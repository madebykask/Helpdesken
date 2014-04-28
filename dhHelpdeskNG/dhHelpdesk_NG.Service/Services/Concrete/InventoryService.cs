namespace DH.Helpdesk.Services.Services.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ServerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Printer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Server;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.PrinterSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ServerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.PrinterFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ServerFieldSettings;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Computers;
    using DH.Helpdesk.Dal.Repositories.Inventory;
    using DH.Helpdesk.Dal.Repositories.Printers;
    using DH.Helpdesk.Dal.Repositories.Servers;
    using DH.Helpdesk.Dal.Repositories.WorkstationModules;
    using DH.Helpdesk.Services.Requests.Inventory;
    using DH.Helpdesk.Services.Response.Inventory;

    public class InventoryService : IInventoryService
    {
        private readonly IInventoryTypeRepository inventoryTypeRepository;

        private readonly IComputerRepository computerRepository;

        private readonly IComputerFieldSettingsRepository computerFieldSettingsRepository;

        private readonly IServerRepository serverRepository;

        private readonly IServerFieldSettingsRepository serverFieldSettingsRepository;

        private readonly IPrinterRepository printerRepository;

        private readonly IPrinterFieldSettingsRepository printerFieldSettingsRepository;

        private readonly IDepartmentRepository departmentRepository;

        private readonly IRegionRepository regionRepository;

        private readonly IComputerTypeRepository computerTypeRepository;

        private readonly IInventoryFieldSettingsRepository inventoryFieldSettingsRepository;

        private readonly IInventoryDynamicFieldSettingsRepository inventoryDynamicFieldSettingsRepository;

        private readonly IInventoryRepository inventoryRepository;

        private readonly IInventoryTypePropertyValueRepository inventoryTypePropertyValueRepository;

        private readonly IOperatingSystemRepository operatingSystemRepository;

        private readonly IProcessorRepository processorRepository;

        private readonly IRAMRepository ramRepository;

        private readonly INICRepository nicRepository;

        private readonly IDomainRepository domainRepository;

        private readonly IOrganizationUnitRepository organizationUnitRepository;

        private readonly IBuildingRepository buildingRepository;

        private readonly IFloorRepository floorRepository;

        private readonly IRoomRepository roomRepository;

        public InventoryService(
            IInventoryTypeRepository inventoryTypeRepository,
            IComputerRepository computerRepository,
            IComputerFieldSettingsRepository computerFieldSettingsRepository,
            IServerRepository serverRepository,
            IServerFieldSettingsRepository serverFieldSettingsRepository,
            IPrinterRepository printerRepository,
            IPrinterFieldSettingsRepository printerFieldSettingsRepository,
            IDepartmentRepository departmentRepository,
            IRegionRepository regionRepository,
            IComputerTypeRepository computerTypeRepository,
            IInventoryFieldSettingsRepository inventoryFieldSettingsRepository,
            IInventoryDynamicFieldSettingsRepository inventoryDynamicFieldSettingsRepository,
            IInventoryRepository inventoryRepository,
            IInventoryTypePropertyValueRepository inventoryTypePropertyValueRepository,
            IOperatingSystemRepository operatingSystemRepository,
            IProcessorRepository processorRepository,
            IRAMRepository ramRepository,
            INICRepository nicRepository,
            IDomainRepository domainRepository,
            IOrganizationUnitRepository organizationUnitRepository,
            IBuildingRepository buildingRepository,
            IFloorRepository floorRepository,
            IRoomRepository roomRepository)
        {
            this.inventoryTypeRepository = inventoryTypeRepository;
            this.computerRepository = computerRepository;
            this.computerFieldSettingsRepository = computerFieldSettingsRepository;
            this.serverRepository = serverRepository;
            this.serverFieldSettingsRepository = serverFieldSettingsRepository;
            this.printerRepository = printerRepository;
            this.printerFieldSettingsRepository = printerFieldSettingsRepository;
            this.departmentRepository = departmentRepository;
            this.regionRepository = regionRepository;
            this.computerTypeRepository = computerTypeRepository;
            this.inventoryFieldSettingsRepository = inventoryFieldSettingsRepository;
            this.inventoryDynamicFieldSettingsRepository = inventoryDynamicFieldSettingsRepository;
            this.inventoryRepository = inventoryRepository;
            this.inventoryTypePropertyValueRepository = inventoryTypePropertyValueRepository;
            this.operatingSystemRepository = operatingSystemRepository;
            this.processorRepository = processorRepository;
            this.ramRepository = ramRepository;
            this.nicRepository = nicRepository;
            this.domainRepository = domainRepository;
            this.organizationUnitRepository = organizationUnitRepository;
            this.buildingRepository = buildingRepository;
            this.floorRepository = floorRepository;
            this.roomRepository = roomRepository;
        }

        public List<ItemOverview> GetInventoryTypes(int customerId)
        {
            return this.inventoryTypeRepository.FindOverviews(customerId);
        }

        #region Workstation

        public ComputerFiltersResponse GetWorkstationFilters(int customerId)
        {
            var regions = this.regionRepository.FindByCustomerId(customerId);
            var departments = this.departmentRepository.FindActiveOverviews(customerId);
            var computerTypes = this.computerTypeRepository.FindOverviews(customerId);

            var filter = new ComputerFiltersResponse(regions, departments, computerTypes);

            return filter;
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

        public ComputerEditResponse GetComputerEditResponse(int id, int customerId)
        {
            var model = this.computerRepository.FindById(id);
            var computerTypes = this.computerTypeRepository.FindOverviews(customerId);
            var operatingSystems = this.operatingSystemRepository.FindOverviews();
            var processors = this.processorRepository.FindOverviews();
            var rams = this.ramRepository.FindOverviews();
            var netAdapters = this.nicRepository.FindOverviews();
            var domains = this.domainRepository.FindByCustomerId(customerId);
            var ous = this.organizationUnitRepository.FindActiveAndShowable();
            var buildings = this.buildingRepository.FindOverviews(customerId);
            var floors = this.floorRepository.FindOverviews(customerId);
            var rooms = this.roomRepository.FindOverviews(customerId);

            return new ComputerEditResponse(
                model,
                computerTypes,
                operatingSystems,
                processors,
                rams,
                netAdapters,
                domains,
                ous,
                buildings,
                floors,
                rooms);
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

        public void UpdateWorkstationFieldsSettings(ComputerFieldsSettings businessModel)
        {
            throw new NotImplementedException();
        }

        public ComputerFieldsSettings GetWorkstationFieldSettingsForEdit(int customerId, int languageId)
        {
            throw new NotImplementedException();
        }

        public ComputerFieldsSettingsForModelEdit GetWorkstationFieldSettingsForModelEdit(int customerId, int languageId)
        {
            var models = this.computerFieldSettingsRepository.GetFieldSettingsForModelEdit(customerId, languageId);

            return models;
        }

        public ComputerFieldsSettingsOverview GetWorkstationFieldSettingsOverview(int customerId, int languageId)
        {
            var models = this.computerFieldSettingsRepository.GetFieldSettingsOverview(customerId, languageId);

            return models;
        }

        public ComputerFieldsSettingsOverviewForFilter GetWorkstationFieldSettingsOverviewForFilter(int customerId, int languageId)
        {
            var models = this.computerFieldSettingsRepository.GetFieldSettingsOverviewForFilter(customerId, languageId);

            return models;
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

        public Server GetServerById(int id)
        {
            throw new NotImplementedException();
        }

        public List<ServerOverview> GetServers(ServersFilter computersFilter)
        {
            var models = this.serverRepository.FindOverviews(computersFilter.CustomerId, computersFilter.SearchFor);

            return models;
        }

        public void UpdateServerFieldsSettings(ServerFieldsSettings businessModel)
        {
            throw new NotImplementedException();
        }

        public ServerFieldsSettings GetServerFieldSettingsForEdit(int customerId, int languageId)
        {
            throw new NotImplementedException();
        }

        public ServerFieldsSettingsForModelEdit GetServerFieldSettingsForModelEdit(int customerId, int languageId)
        {
            throw new NotImplementedException();
        }

        public ServerFieldsSettingsOverview GetServerFieldSettingsOverview(int customerId, int languageId)
        {
            var models = this.serverFieldSettingsRepository.GetFieldSettingsOverview(customerId, languageId);

            return models;
        }

        #endregion

        #region Printer

        public PrinterFiltersResponse GetPrinterFilters(int customerId)
        {
            var departments = this.departmentRepository.FindActiveOverviews(customerId);
            var filter = new PrinterFiltersResponse(departments);

            return filter;
        }

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

        public Printer GetPrinterById(int id)
        {
            throw new NotImplementedException();
        }

        public List<PrinterOverview> GetPrinters(PrintersFilter printersFilter)
        {
            var models = this.printerRepository.FindOverviews(
                printersFilter.CustomerId,
                printersFilter.DepartmentId,
                printersFilter.SearchFor);

            return models;
        }

        public void UpdatePrinterFieldsSettings(PrinterFieldsSettings businessModel)
        {
            throw new NotImplementedException();
        }

        public PrinterFieldsSettings GetPrinterFieldSettingsForEdit(int customerId, int languageId)
        {
            throw new NotImplementedException();
        }

        public PrinterFieldsSettingsForModelEdit GetPrinterFieldSettingsForModelEdit(int customerId, int languageId)
        {
            throw new NotImplementedException();
        }

        public PrinterFieldsSettingsOverview GetPrinterFieldSettingsOverview(int customerId, int languageId)
        {
            var models = this.printerFieldSettingsRepository.GetFieldSettingsOverview(customerId, languageId);

            return models;
        }

        public PrinterFieldsSettingsOverviewForFilter GetPrinterFieldSettingsOverviewForFilter(int customerId, int languageId)
        {
            var models = this.printerFieldSettingsRepository.GetFieldSettingsOverviewForFilter(customerId, languageId);

            return models;
        }

        #endregion

        #region DynamicData

        public CustomTypeFiltersResponse GetInventoryFilters(int customerId)
        {
            var departments = this.departmentRepository.FindActiveOverviews(customerId);
            var filter = new CustomTypeFiltersResponse(departments);

            return filter;
        }

        public InventoryFieldSettingsOverviewResponse GetInventoryFieldSettingsOverview(int inventoryTypeId)
        {
            var setings = this.inventoryFieldSettingsRepository.GetFieldSettingsOverview(inventoryTypeId);
            var dynamicSettings = this.inventoryDynamicFieldSettingsRepository.GetFieldSettingsOverview(
                inventoryTypeId);

            var response = new InventoryFieldSettingsOverviewResponse(setings, dynamicSettings);

            return response;
        }

        public InventoryOverviewResponse GetInventories(InventoriesFilter filter)
        {
            var models = this.inventoryRepository.FindOverviews(
                filter.InventoryTypeId,
                filter.DepartmentId,
                filter.SearchFor,
                filter.RecordsOnPage);

            var ids = models.Select(x => x.Id).ToList();

            var dynamicData = this.inventoryTypePropertyValueRepository.GetData(ids);

            var response = new InventoryOverviewResponse(models, dynamicData);

            return response;
        }

        public InventoryFieldsSettingsOverviewForFilter GetInventoryFieldSettingsOverviewForFilter(int inventoryTypeId)
        {
            var models = this.inventoryFieldSettingsRepository.GetFieldSettingsOverviewForFilter(inventoryTypeId);

            return models;
        }

        #endregion
    }
}