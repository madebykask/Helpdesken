namespace DH.Helpdesk.Dal.Repositories.Servers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.Dal.Dal;

    public interface IServerSoftwareRepository : INewRepository
    {
        List<SoftwareOverview> Find(int computerId);
    }
}