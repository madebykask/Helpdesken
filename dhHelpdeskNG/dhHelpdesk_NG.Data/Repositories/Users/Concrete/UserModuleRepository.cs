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
                    Id = u.Id,
                    User_Id = u.User_Id,
                    Module_Id = u.Module_Id,
                    Position = u.Position,
                    isVisible = u.isVisible,
                    NumberOfRows = u.NumberOfRows,
                    Module = u.Module != null ? new ModuleOverview()
                    {
                        Id = u.Module.Id,
                        Name = u.Module.Name,
                        Description = u.Module.Description
                    } : new ModuleOverview(){}
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

        public UserModule GetUserModule(int userId, int moduleId)
        {
            return DataContext.UsersModules
                .Where(m => m.User_Id == userId && m.Module_Id == moduleId)
                .Select(m => new UserModule()
                {
                    Id = m.Id,
                    User_Id = m.User_Id,
                    Module_Id = m.Module_Id,
                    isVisible = m.isVisible,
                    NumberOfRows = m.NumberOfRows,
                    Position = m.Position
                })
                .FirstOrDefault();

        }
    }
}