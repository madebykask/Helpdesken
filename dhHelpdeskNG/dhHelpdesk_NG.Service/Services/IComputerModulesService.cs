namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Input;
    using DH.Helpdesk.BusinessData.Models.Shared;

    public interface IComputerModulesService
    {
        List<ItemOverview> GetNetAdapters();

        List<ItemOverview> GetRams();

        List<ItemOverview> GetProcessors();

        List<ItemOverview> GetOperatingSystems();

        List<ItemOverview> GetComputerTypes(int customerId);

        List<ItemOverview> GetComputerModels();

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

        void AddComputerType(ComputerModule module);

        void AddComputerModel(ComputerModule module);

        void DeleteNetAdapter(int id);

        void DeleteRam(int id);

        void DeleteProcessor(int id);

        void DeleteOperatingSystem(int id);

        void DeleteComputerType(int id);

        void DeleteComputerModel(int id);
    }
}