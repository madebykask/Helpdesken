namespace DH.Helpdesk.Dal.Repositories.WorkstationModules
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.Dal.Dal;

    public interface ISoftwareRepository : INewRepository
    {
        List<SoftwareOverview> Find(int computerId);

        List<ReportModel> FindAllComputerSoftware(int customerId, int? departmentId, string searchFor);

        void DeleteByComputerId(int computerId);
    }
}