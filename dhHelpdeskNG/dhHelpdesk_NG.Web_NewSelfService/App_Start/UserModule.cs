namespace DH.Helpdesk.NewSelfService
{
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Dal.Mappers.Users.BusinessModelToEntity;
    using DH.Helpdesk.Domain.Users;

    using Ninject.Modules;

    public sealed class UserModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IBusinessModelToEntityMapper<BusinessData.Models.Users.Input.UserModule, UserModuleEntity>>()
                .To<UpdatedUserModuleToUserModuleEntityMapper>()
                .InSingletonScope();
        }
    }
}