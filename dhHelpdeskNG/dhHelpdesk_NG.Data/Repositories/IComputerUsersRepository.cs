using DH.Helpdesk.Domain.Computers;

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.Dal.Dal;

    public interface IComputerUsersRepository : INewRepository
    {
        List<ComputerUserOverview> GetOverviews(int customerId, string userId);

        List<ComputerUserOverview> GetConnectedToComputerOverviews(int computerId);

        string FindUserGuidById(int id);
        string GetEmailByUserId(string userId, int customerId);
        string GetEmailByName(string fullName, int customerId);
        ComputerUser GetComputerUserByUserId(string userId, int customerId, int? domainId = null);
    }
}