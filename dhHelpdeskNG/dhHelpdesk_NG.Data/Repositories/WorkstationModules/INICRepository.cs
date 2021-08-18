namespace DH.Helpdesk.Dal.Repositories.WorkstationModules
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Input;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Dal;

    public interface INICRepository : INewRepository
    {
        void Add(ComputerModule businessModel);

        void DeleteById(int id);

        void Update(ComputerModule businessModel);

        List<ItemOverview> FindOverviews(int customerId);

        List<ReportModel> FindConnectedToComputerOverviews(int customerId, int? departmentId, string searchFor);

        List<ReportModel> FindConnectedToServerOverviews(int customerId, string searchFor);
    }
}