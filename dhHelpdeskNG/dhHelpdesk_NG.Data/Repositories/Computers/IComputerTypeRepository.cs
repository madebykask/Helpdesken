namespace DH.Helpdesk.Dal.Repositories.Computers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Dal.Dal;

    public interface IComputerTypeRepository : INewRepository
    {
        List<ItemOverview> FindOverviews(int customerId);
    }
}