using System.Collections.Generic;
using ECT.Model.Entities;

namespace ECT.Model.Abstract
{
    public interface IUserRepository
    {
        User Get(string identity, string userId);
        IEnumerable<User> GetUsers(int customerId, int userGroupId);
        IEnumerable<WorkingGroup> GetWorkingGroups(int customerId, int userId);
    }
}