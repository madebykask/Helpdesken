namespace DH.Helpdesk.Dal.Repositories.WorkstationModules
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Input;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Dal;

    public interface IOperatingSystemRepository : INewRepository
    {
        void Add(ComputerModule businessModel);

        void DeleteById(int id);

        void Update(ComputerModule businessModel);

        List<ItemOverview> FindOverviews(int customerId);

        List<ReportModel> FindConnectedToComputerOperatingSystemOverviews(
            int customerId,
            int? departmentId,
            string searchFor);

        List<ReportModel> FindConnectedToComputerServicePackOverviews(
            int customerId,
            int? departmentId,
            string searchFor);

        List<ReportModel> FindConnectedToServerOperatingSystemOverviews(int customerId, string searchFor);

        List<ReportModel> FindConnectedToServerServicePackOverviews(int customerId, string searchFor);
    }
}