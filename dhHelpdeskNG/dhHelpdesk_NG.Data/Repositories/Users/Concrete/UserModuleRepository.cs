using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Users.Input;
using DH.Helpdesk.BusinessData.Models.Users.Output;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Domain.Users;

namespace DH.Helpdesk.Dal.Repositories.Users.Concrete
{
    public sealed class UserModuleRepository : RepositoryBase<UserModuleEntity>, IUserModuleRepository
    {
        private readonly IBusinessModelToEntityMapper<UserModule, UserModuleEntity> _updatedUserModuleToUserModuleEntityMapper;

        public UserModuleRepository(IDatabaseFactory databaseFactory,
            IBusinessModelToEntityMapper<UserModule, UserModuleEntity> updatedUserModuleToUserModuleEntityMapper)
            : base(databaseFactory)
        {
            _updatedUserModuleToUserModuleEntityMapper = updatedUserModuleToUserModuleEntityMapper;
        }

        public IEnumerable<UserModuleOverview> GetUserModules(int user)
        {
            return DataContext.UsersModules
                .Where(u => u.User_Id == user)
                .ToList()
                .Select(u => new UserModuleOverview()
                {
                    User_Id = u.User_Id,
                    Module_Id = u.Module_Id,
                    Position = u.Position,
                    isVisible = u.isVisible,
                    NumberOfRows = u.NumberOfRows,
                    Module = new ModuleOverview()
                    {
                        Id = u.Module.Id,
                        Name = u.Module.Name,
                        Description = u.Module.Description
                    }
                })
                .OrderBy(u => u.Module.Name);
        }

        public void UpdateUserModules(IEnumerable<UserModule> modules)
        {
            foreach (var module in modules)
            {
                var entity = DataContext.UsersModules
                                .FirstOrDefault(m => m.User_Id == module.User_Id &&
                                                    m.Module_Id == module.Module_Id);
                if (entity == null)
                {
                    entity = new UserModuleEntity();
                    _updatedUserModuleToUserModuleEntityMapper.Map(module, entity);   
                    Add(entity);
                    continue;
                }
                _updatedUserModuleToUserModuleEntityMapper.Map(module, entity);   
                Update(entity);
            }
        }
    }
}