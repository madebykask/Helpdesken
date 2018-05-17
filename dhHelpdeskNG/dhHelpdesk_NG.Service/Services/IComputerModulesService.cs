using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;

namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Input;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.BusinessData.Models.Shared;

    public interface IComputerModulesService
    {
        List<LogicalDriveOverview> GetComputerLogicalDrive(int id);

        List<LogicalDriveOverview> GetServerLogicalDrive(int id);

        List<SoftwareOverview> GetComputerSoftware(int id);

        List<SoftwareOverview> GetServerSoftware(int id);

        List<ItemOverview> GetNetAdapters();

        List<ItemOverview> GetRams();

        List<ItemOverview> GetProcessors();

        List<ItemOverview> GetOperatingSystems();

        List<ItemOverview> GetComputerTypes(int customerId);

        ComputerTypeOverview GetComputerType(int id);

        List<ItemOverview> GetComputerModels();

        List<ReportModel> GetConnectedToComputersProcessorsOverviews(
            int customerId,
            int? departmentId,
            string searchFor);

        List<ReportModel> GetConnectedToServersProcessorsOverviews(int customerId, string searchFor);

        List<ReportModel> GetConnectedToComputersRamOverviews(int customerId, int? departmentId, string searchFor);

        List<ReportModel> GetConnectedToServersRamOverviews(int customerId, string searchFor);

        List<ReportModel> GetConnectedToComputersNicOverviews(int customerId, int? departmentId, string searchFor);

        List<ReportModel> GetConnectedToServersNicOverviews(int customerId, string searchFor);

        List<ReportModel> GetConnectedToComputersLocationOverviews(int customerId, int? departmentId, string searchFor);

        List<ReportModel> GetConnectedToServersLocationOverviews(int customerId, string searchFor);

        List<ReportModel> GetComputersInstaledSoftware(int customerId, int? departmentId, string searchFor);

        List<ReportModel> GetServersInstaledSoftware(int customerId, string searchFor);

        List<ReportModel> GetConnectedToComputerOperatingSystemOverviews(
            int customerId,
            int? departmentId,
            string searchFor);

        List<ReportModel> GetConnectedToComputerServicePackOverviews(
            int customerId,
            int? departmentId,
            string searchFor);

        List<ReportModel> GetConnectedToServerOperatingSystemOverviews(int customerId, string searchFor);

        List<ReportModel> GetConnectedToServerServicePackOverviews(int customerId, string searchFor);

        void UpdateNetAdapter(ComputerModule module);

        void UpdateRam(ComputerModule module);

        void UpdateProcessor(ComputerModule module);

        void UpdateOperatingSystem(ComputerModule module);

        void UpdateComputerType(ComputerModule module);

        void UpdateComputerModel(ComputerModule module);

        void AddNetAdapter(ComputerModule module);

        void AddRam(ComputerModule module);

        void AddProcessor(ComputerModule module);

        void AddOperatingSystem(ComputerModule module);

        void AddComputerType(ComputerModule module, OperationContext context);

        void AddComputerModel(ComputerModule module);

        void DeleteNetAdapter(int id);

        void DeleteRam(int id);

        void DeleteProcessor(int id);

        void DeleteOperatingSystem(int id);

        void DeleteComputerType(int id);

        void DeleteComputerModel(int id);
    }
}