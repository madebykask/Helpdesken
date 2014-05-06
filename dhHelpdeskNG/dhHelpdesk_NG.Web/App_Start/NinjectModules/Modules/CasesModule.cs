namespace DH.Helpdesk.Web.NinjectModules.Modules
{
    using DH.Helpdesk.BusinessData.Models.Case.Input;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity;
    using DH.Helpdesk.Domain.Computers;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Case;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Case.Concrete;

    using Ninject.Modules;

    internal sealed class CasesModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<ICaseNotifierModelFactory>().To<CaseNotifierModelFactory>().InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CaseNotifier, ComputerUser>>().To<CaseNotifierToEntityMapper>().InSingletonScope();
        }
    }
}