namespace DH.Helpdesk.Dal.Repositories.WorkstationModules
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.Dal.Dal;

    public interface ILogicalDriveRepository : INewRepository
    {
        List<LogicalDriveOverview> Find(int computerId);

        void DeleteByComputerId(int id);
    }
}