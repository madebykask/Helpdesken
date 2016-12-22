using System.Collections.Generic;
using DH.Helpdesk.EForm.Model.Entities;

namespace DH.Helpdesk.EForm.Model.Abstract
{
    public interface IUserRepository
    {
        User Get(string identity, string userId);
        IEnumerable<User> GetUsers(int customerId, int userGroupId);
        IEnumerable<WorkingGroup> GetWorkingGroups(int customerId, int userId);
    }
}