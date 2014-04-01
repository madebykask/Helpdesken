namespace DH.Helpdesk.Services.Services.Concrete
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Computers;
    using DH.Helpdesk.Dal.Repositories.Inventory;
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

        private readonly IDepartmentRepository departmentRepository;

        private readonly IRegionRepository regionRepository;

        private readonly IComputerTypeRepository computerTypeRepository;

        public InventoryService(
            IInventoryTypeRepository inventoryTypeRepository,
            IComputerRepository computerRepository,
            IServerRepository serverRepository,
            IPrinterRepository printerRepository,
            IDepartmentRepository departmentRepository,
            IRegionRepository regionRepository,
            IComputerTypeRepository computerTypeRepository)
        {
            this.inventoryTypeRepository = inventoryTypeRepository;
            this.computerRepository = computerRepository;
            this.serverRepository = serverRepository;
            this.printerRepository = printerRepository;
            this.departmentRepository = departmentRepository;
            this.regionRepository = regionRepository;
            this.computerTypeRepository = computerTypeRepository;
        }

        public List<ItemOverview> GetInventoryTypes(int customerId)
        {
            return this.inventoryTypeRepository.FindOverviews(customerId);
        }

        public ComputerFiltersRequest GetComputerFilters(int customerId)
        {
            var regions = this.regionRepository.FindByCustomerId(customerId);
            var departments = this.departmentRepository.FindActiveOverviews(customerId);
            var computerTypes = this.computerTypeRepository.FindOverviews(customerId);

            var filter = new ComputerFiltersRequest(regions, departments, computerTypes);

            return filter;
        }

        public void AddComputer(Computer businessModel)
        {
            throw new NotImplementedException();
        }

        public void DeleteComputer(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateComputer(Computer businessModel)
        {
            throw new NotImplementedException();
        }

        public Computer GetComputerById(int id)
        {
            throw new NotImplementedException();
        }

        public List<ComputerOverview> FindComputerOverviews(ComputersFilter computersFilter)
        {
            var computerOverviews = this.computerRepository.FindOverviews(
                computersFilter.CustomerId,
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
                computersFilter.SearchFor);

            return computerOverviews;
        }

        public ServerFiltersRequest GetServerFilters(int customerId)
        {
            throw new NotImplementedException();
        }

        public PrinterFiltersRequest GetPrinterFilters(int customerId)
        {
            var departments = this.departmentRepository.FindActiveOverviews(customerId);
            var filter = new PrinterFiltersRequest(departments);

            return filter;
        }

        public CustomTypeFiltersRequest GetCustomTypeFilters(int customerId)
        {
            var departments = this.departmentRepository.FindActiveOverviews(customerId);
            var filter = new CustomTypeFiltersRequest(departments);

            return filter;
        }

        public void UpdateComputerFieldsSettings(ComputerFieldsSettings businessModel)
        {
            throw new NotImplementedException();
        }

        public ComputerFieldsSettings GetComputerFieldSettingsForEdit(int customerId, int languageId)
        {
            throw new NotImplementedException();
        }

        public ComputerFieldsSettingsForModelEdit GetComputerFieldSettingsForModelEdit(int customerId, int languageId)
        {
            throw new NotImplementedException();
        }

        public ComputerFieldsSettingsOverview GetComputerFieldSettingsOverview(int customerId, int languageId)
        {
            throw new NotImplementedException();
        }
    }
}