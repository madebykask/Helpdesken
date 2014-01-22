namespace dhHelpdesk_NG.Web.NinjectModules.Changes
{
    using System;

    using dhHelpdesk_NG.Data.Dal.Mappers;
    using dhHelpdesk_NG.Data.Dal.Mappers.Changes;
    using dhHelpdesk_NG.Domain.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Change;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.Service.BusinessModelFactories.Changes;
    using dhHelpdesk_NG.Service.BusinessModelFactories.Changes.Concrete;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.Concrete;

    using Ninject.Modules;

    public sealed class ChangesModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IEntityToBusinessModelMapper<ChangeEntity, Change>>()
                .To<ChangeEntityToChangeMapper>()
                .InSingletonScope();

            this.Bind<IEntityChangerFromBusinessModel<Contact, ChangeContactEntity>>()
                .To<Changer>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<Contact, ChangeContactEntity>>()
                .To<ContactToChangeContactEntityMapper>()
                .InSingletonScope();

            this.Bind<IChangeFactory>().To<ChangeFactory>().InSingletonScope();
            this.Bind<IChangeModelFactory>().To<ChangeModelFactory>().InSingletonScope();
        }
    }
}