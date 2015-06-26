﻿namespace DH.Helpdesk.Web.NinjectModules.Modules
{
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Case.Input;
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity;
    using DH.Helpdesk.Dal.Mappers.Cases.EntityToBusinessModel;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Cases;
    using DH.Helpdesk.Domain.Computers;
    using DH.Helpdesk.Services.Infrastructure.Cases;
    using DH.Helpdesk.Services.Infrastructure.Cases.Concrete;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Case;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Case.Concrete;
    using Ninject.Modules;
    using Ninject.Web.Common;

    internal sealed class CasesModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<ICaseNotifierModelFactory>().To<CaseNotifierModelFactory>().InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CaseNotifier, ComputerUser>>().To<CaseNotifierToEntityMapper>().InSingletonScope();

            this.Bind<ICasesCalculator>().To<CasesCalculator>().InRequestScope();

            this.Bind<ICaseModelFactory>().To<CaseModelFactory>().InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<Case, CaseOverview>>()
                .To<CaseToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CaseLock, CaseLockEntity>>()
                .To<CaseLockToEntityMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<CaseLockEntity, CaseLock>>()
                .To<CaseLockToBusinessModelMapper>()
                .InSingletonScope();

        }
    }
}