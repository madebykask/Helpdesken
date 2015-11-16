using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Users.Input;
using DH.Helpdesk.BusinessData.Models.Users.Output;
using DH.Helpdesk.Domain.Users;
using DH.Helpdesk.Dal.Infrastructure;

namespace DH.Helpdesk.Dal.Repositories.Users
{
    public interface IUserModuleRepository : IRepository<UserModuleEntity>
    {
        IEnumerable<UserModuleOverview> GetUserModules(int user);
        
        void UpdateUserModules(IEnumerable<UserModule> modules);

        UserModule GetUserModule(int userId, int moduleId);
    }
}