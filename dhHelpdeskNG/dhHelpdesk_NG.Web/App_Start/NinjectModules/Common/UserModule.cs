using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Dal.Mappers.Users.BusinessModelToEntity;
using DH.Helpdesk.Domain.Users;
using Ninject.Modules;

namespace DH.Helpdesk.Web.NinjectModules.Common
{
    public sealed class UserModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IBusinessModelToEntityMapper<BusinessData.Models.Users.Input.UserModule, UserModuleEntity>>()
                .To<UpdatedUserModuleToUserModuleEntityMapper>()
                .InSingletonScope();
        }
    }
}