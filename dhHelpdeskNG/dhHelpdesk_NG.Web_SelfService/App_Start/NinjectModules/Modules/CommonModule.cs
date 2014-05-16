// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommonModule.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CommonModule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.SelfService.NinjectModules.Modules
{
    using DH.Helpdesk.BusinessData.Models.Case.Input;
    using DH.Helpdesk.BusinessData.Models.Customer;
    using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
    using DH.Helpdesk.Dal.Infrastructure.Translate;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity;
    using DH.Helpdesk.Dal.Mappers.Customer.EntityToBusinessModel;
    using DH.Helpdesk.Dal.Mappers.ProductArea.EntityToBusinessModel;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Computers;
    using DH.Helpdesk.SelfService.Infrastructure.Translate;

    using Ninject.Modules;

    /// <summary>
    /// The common module.
    /// </summary>
    public class CommonModule : NinjectModule
    {
        /// <summary>
        /// The load.
        /// </summary>
        public override void Load()
        {
            this.Bind<IEntityToBusinessModelMapper<ProductArea, ProductAreaOverview>>()
                .To<ProductAreaToOverviewMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<Setting, CustomerSettings>>()
                .To<CustomerSettingsToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CaseNotifier, ComputerUser>>()
                .To<CaseNotifierToEntityMapper>()
                .InSingletonScope();

            this.Bind<ITranslator>().To<Translator>().InSingletonScope();
        }
    }
}