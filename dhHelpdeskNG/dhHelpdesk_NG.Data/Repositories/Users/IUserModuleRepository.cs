using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Users.Input;
using DH.Helpdesk.BusinessData.Models.Users.Output;

namespace DH.Helpdesk.Dal.Repositories.Users
{
    public interface IUserModuleRepository
    {
        IEnumerable<UserModuleOverview> GetUserModules(int user);
        
        void UpdateUserModules(IEnumerable<UserModule> modules);
    }
}