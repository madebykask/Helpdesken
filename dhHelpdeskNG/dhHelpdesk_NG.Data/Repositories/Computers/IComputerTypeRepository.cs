namespace DH.Helpdesk.Dal.Repositories.Computers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Dal;

    public interface IComputerTypeRepository : INewRepository
    {
        List<ItemOverview> FindOverviews(int customerId);
    }
}