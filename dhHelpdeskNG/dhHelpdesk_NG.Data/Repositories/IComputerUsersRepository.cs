using DH.Helpdesk.Domain.Computers;

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.Dal.Dal;
    using BusinessData.Models.Employee;

    public interface IComputerUsersRepository : INewRepository
    {
        List<ComputerUserOverview> GetOverviews(int customerId, string userId);

        List<ComputerUserOverview> GetConnectedToComputerOverviews(int computerId);

        string FindUserGuidById(int id);
        ComputerUser GetComputerUserByUserId(string userId, int customerId, int? domainId = null);
        List<string> GetEmailByUserIds(List<string> userIds, int customerId);

        IList<SubordinateResponseItem> GetEmployeesByUserId(int customerId, IList<string> userIds);

    }
}