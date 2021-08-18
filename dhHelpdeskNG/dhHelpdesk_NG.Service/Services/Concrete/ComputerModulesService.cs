using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;

namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Input;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Computers;
    using DH.Helpdesk.Dal.Repositories.Servers;
    using DH.Helpdesk.Dal.Repositories.WorkstationModules;

    public class ComputerModulesService : IComputerModulesService
    {
        private readonly IOperatingSystemRepository operatingSystemRepository;

        private readonly IProcessorRepository processorRepository;

        private readonly IRAMRepository ramRepository;

        private readonly INICRepository nicRepository;

        private readonly IComputerTypeRepository computerTypeRepository;

        private readonly IComputerModelRepository computerModelRepository;

        private readonly IComputerRepository computerRepository;

        private readonly IServerRepository serverRepository;

        private readonly ISystemRepository systemRepository;

        private readonly ISoftwareRepository softwareRepository;

        private readonly IServerSoftwareRepository serverSoftwareRepository;

        private readonly ILogicalDriveRepository logicalDriveRepository;

        private readonly IServerLogicalDriveRepository serverLogicalDriveRepository;

        public ComputerModulesService(
            IOperatingSystemRepository operatingSystemRepository,
            IProcessorRepository processorRepository,
            IRAMRepository ramRepository,
            INICRepository nicRepository,
            IComputerTypeRepository computerTypeRepository,
            IComputerModelRepository computerModelRepository,
            IComputerRepository computerRepository,
            IServerRepository serverRepository,
            ISystemRepository systemRepository,
            ISoftwareRepository softwareRepository,
            IServerSoftwareRepository serverSoftwareRepository,
            ILogicalDriveRepository logicalDriveRepository,
            IServerLogicalDriveRepository serverLogicalDriveRepository)
        {
            this.operatingSystemRepository = operatingSystemRepository;
            this.processorRepository = processorRepository;
            this.ramRepository = ramRepository;
            this.nicRepository = nicRepository;
            this.computerTypeRepository = computerTypeRepository;
            this.computerModelRepository = computerModelRepository;
            this.computerRepository = computerRepository;
            this.serverRepository = serverRepository;
            this.systemRepository = systemRepository;
            this.softwareRepository = softwareRepository;
            this.serverSoftwareRepository = serverSoftwareRepository;
            this.logicalDriveRepository = logicalDriveRepository;
            this.serverLogicalDriveRepository = serverLogicalDriveRepository;
        }

        public List<LogicalDriveOverview> GetComputerLogicalDrive(int id)
        {
            return this.logicalDriveRepository.Find(id);
        }

        public List<LogicalDriveOverview> GetServerLogicalDrive(int id)
        {
            return this.serverLogicalDriveRepository.Find(id);
        }

        public List<SoftwareOverview> GetComputerSoftware(int id)
        {
            return this.softwareRepository.Find(id);
        }

        public List<SoftwareOverview> GetServerSoftware(int id)
        {
            return this.serverSoftwareRepository.Find(id);
        }

        public List<ItemOverview> GetNetAdapters()
        {
            var netAdapters = this.nicRepository.FindOverviews();
            return netAdapters;
        }

        public List<ItemOverview> GetRams()
        {
            var rams = this.ramRepository.FindOverviews();
            return rams;
        }

        public List<ItemOverview> GetProcessors(int customerId)
        {
            var processors = this.processorRepository.FindOverviews(customerId);
            return processors;
        }

        public List<ItemOverview> GetOperatingSystems(int customerId)
        {
            var operatingSystems = this.operatingSystemRepository.FindOverviews(customerId);
            return operatingSystems;
        }

        public List<ItemOverview> GetComputerTypes(int customerId, int? inventoryId = null)
        {
            var computerTypes = this.computerTypeRepository.FindOverviews(customerId, inventoryId);
            return computerTypes;
        }

        public ComputerTypeOverview GetComputerType(int id)
        {
            var computerType = this.computerTypeRepository.Get(id);
            return computerType;
        }

        public List<ItemOverview> GetComputerModels()
        {
            var computerModels = this.computerModelRepository.FindOverviews();
            return computerModels;
        }

        public List<ReportModel> GetConnectedToComputersProcessorsOverviews(int customerId, int? departmentId, string searchFor)
        {
            return this.processorRepository.FindConnectedToComputerOverviews(customerId, departmentId, searchFor);
        }

        public List<ReportModel> GetConnectedToServersProcessorsOverviews(int customerId, string searchFor)
        {
            return this.processorRepository.FindConnectedToServerOverviews(customerId, searchFor);
        }

        public List<ReportModel> GetConnectedToComputersRamOverviews(int customerId, int? departmentId, string searchFor)
        {
            return this.ramRepository.FindConnectedToComputerOverviews(customerId, departmentId, searchFor);
        }

        public List<ReportModel> GetConnectedToServersRamOverviews(int customerId, string searchFor)
        {
            return this.ramRepository.FindConnectedToServerOverviews(customerId, searchFor);
        }

        public List<ReportModel> GetConnectedToComputersNicOverviews(int customerId, int? departmentId, string searchFor)
        {
            return this.nicRepository.FindConnectedToComputerOverviews(customerId, departmentId, searchFor);
        }

        public List<ReportModel> GetConnectedToServersNicOverviews(int customerId, string searchFor)
        {
            return this.nicRepository.FindConnectedToServerOverviews(customerId, searchFor);
        }

        public List<ReportModel> GetConnectedToComputersLocationOverviews(int customerId, int? departmentId, string searchFor)
        {
            return this.computerRepository.FindConnectedToComputerLocationOverviews(customerId, departmentId, searchFor);
        }

        public List<ReportModel> GetConnectedToServersLocationOverviews(int customerId, string searchFor)
        {
            return this.serverRepository.FindConnectedToServerLocationOverviews(customerId, searchFor);
        }

        public List<ReportModel> GetComputersInstaledSoftware(int customerId, int? departmentId, string searchFor)
        {
            return this.softwareRepository.FindAllComputerSoftware(customerId, departmentId, searchFor);
        }

        public List<ReportModel> GetServersInstaledSoftware(int customerId, string searchFor)
        {
            return this.serverSoftwareRepository.FindAllServerSoftware(customerId, searchFor);
        }

        public List<ReportModel> GetConnectedToComputerOperatingSystemOverviews(int customerId, int? departmentId, string searchFor)
        {
            return this.operatingSystemRepository.FindConnectedToComputerOperatingSystemOverviews(
                customerId,
                departmentId,
                searchFor);
        }

        public List<ReportModel> GetConnectedToComputerServicePackOverviews(int customerId, int? departmentId, string searchFor)
        {
            return this.operatingSystemRepository.FindConnectedToComputerServicePackOverviews(
                customerId,
                departmentId,
                searchFor);
        }

        public List<ReportModel> GetConnectedToServerOperatingSystemOverviews(int customerId, string searchFor)
        {
            return this.operatingSystemRepository.FindConnectedToServerOperatingSystemOverviews(customerId, searchFor);
        }

        public List<ReportModel> GetConnectedToServerServicePackOverviews(int customerId, string searchFor)
        {
            return this.operatingSystemRepository.FindConnectedToServerServicePackOverviews(customerId, searchFor);
        }

        public void UpdateNetAdapter(ComputerModule module)
        {
            this.nicRepository.Update(module);
            this.nicRepository.Commit();
        }

        public void UpdateRam(ComputerModule module)
        {
            this.ramRepository.Update(module);
            this.ramRepository.Commit();
        }

        public void UpdateProcessor(ComputerModule module)
        {
            this.processorRepository.Update(module);
            this.processorRepository.Commit();
        }

        public void UpdateOperatingSystem(ComputerModule module)
        {
            this.operatingSystemRepository.Update(module);
            this.operatingSystemRepository.Commit();
        }

        public void UpdateComputerType(ComputerModule module)
        {
            this.computerTypeRepository.Update(module);
            this.computerTypeRepository.Commit();
        }

        public void UpdateComputerModel(ComputerModule module)
        {
            this.computerModelRepository.Update(module);
            this.computerModelRepository.Commit();
        }

        public void AddNetAdapter(ComputerModule module)
        {
            this.nicRepository.Add(module);
            this.nicRepository.Commit();
        }

        public void AddRam(ComputerModule module)
        {
            this.ramRepository.Add(module);
            this.ramRepository.Commit();
        }

        public void AddProcessor(ComputerModule module)
        {
            this.processorRepository.Add(module);
            this.processorRepository.Commit();
        }

        public void AddOperatingSystem(ComputerModule module)
        {
            this.operatingSystemRepository.Add(module);
            this.operatingSystemRepository.Commit();
        }

        public void AddComputerType(ComputerModule module, OperationContext context)
        {
            this.computerTypeRepository.Add(module, context.CustomerId);
            this.computerTypeRepository.Commit();
        }

        public void AddComputerModel(ComputerModule module)
        {
            this.computerModelRepository.Add(module);
            this.computerModelRepository.Commit();
        }

        public void DeleteNetAdapter(int id)
        {
            this.computerRepository.RemoveReferenceOnNic(id);
            this.computerRepository.Commit();

            this.serverRepository.RemoveReferenceOnNic(id);
            this.serverRepository.Commit();

            this.nicRepository.DeleteById(id);
            this.nicRepository.Commit();
        }

        public void DeleteRam(int id)
        {
            this.computerRepository.RemoveReferenceOnRam(id);
            this.computerRepository.Commit();

            this.serverRepository.RemoveReferenceOnRam(id);
            this.serverRepository.Commit();

            this.ramRepository.DeleteById(id);
            this.ramRepository.Commit();
        }

        public void DeleteProcessor(int id)
        {
            this.computerRepository.RemoveReferenceOnProcessor(id);
            this.computerRepository.Commit();

            this.serverRepository.RemoveReferenceOnProcessor(id);
            this.serverRepository.Commit();

            this.processorRepository.DeleteById(id);
            this.processorRepository.Commit();
        }

        public void DeleteOperatingSystem(int id)
        {
            this.computerRepository.RemoveReferenceOnOs(id);
            this.computerRepository.Commit();

            this.serverRepository.RemoveReferenceOnOs(id);
            this.serverRepository.Commit();

            this.systemRepository.RemoveReferenceOnOs(id);
            this.serverRepository.Commit();

            this.operatingSystemRepository.DeleteById(id);
            this.operatingSystemRepository.Commit();
        }

        public void DeleteComputerType(int id)
        {
            this.computerRepository.RemoveReferenceOnComputerType(id);
            this.computerRepository.Commit();

            this.computerTypeRepository.DeleteById(id);
            this.computerTypeRepository.Commit();
        }

        public void DeleteComputerModel(int id)
        {
            this.computerRepository.RemoveReferenceOnComputerModel(id);
            this.computerRepository.Commit();

            this.computerModelRepository.DeleteById(id);
            this.computerModelRepository.Commit();
        }
    }
}
