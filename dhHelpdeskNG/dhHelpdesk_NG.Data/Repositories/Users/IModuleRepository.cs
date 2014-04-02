using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Users.Output;

namespace DH.Helpdesk.Dal.Repositories.Users
{
    public interface IModuleRepository
    {
        IEnumerable<ModuleOverview> GetModules();
    }
}