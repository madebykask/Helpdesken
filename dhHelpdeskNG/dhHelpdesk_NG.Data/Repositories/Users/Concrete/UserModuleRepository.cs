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
                .Select(u => new UserModuleOverview()
                {
                    User_Id = u.Module_Id,
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
            var entities = DataContext.UsersModules.Where(m => modules.Select(md => md.Id).Contains(m.Id));
            foreach (var entity in entities)
            {
                var businessModel = modules.First(m => m.Id == entity.Id);
                _updatedUserModuleToUserModuleEntityMapper.Map(businessModel, entity);
            }
        }
    }
}