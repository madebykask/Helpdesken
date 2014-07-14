namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.Dal.Dal;

    public interface IComputerUsersRepository : INewRepository
    {
        List<ComputerUserOverview> GetOverviews(int customerId, string userId);
    }
}