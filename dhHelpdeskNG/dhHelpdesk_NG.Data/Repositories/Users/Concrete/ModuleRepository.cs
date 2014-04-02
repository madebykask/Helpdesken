using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Users.Output;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain.Users;

namespace DH.Helpdesk.Dal.Repositories.Users.Concrete
{
    public sealed class ModuleRepository : RepositoryBase<ModuleEntity>, IModuleRepository
    {
        public ModuleRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        public IEnumerable<ModuleOverview> GetModules()
        {
            return DataContext.Modules
                .Select(m => new ModuleOverview()
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description
                })
                .OrderBy(m => m.Name);
        }
    }
}