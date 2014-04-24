// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommonModule.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CommonModule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Web.NinjectModules.Common
{
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity;
    using DH.Helpdesk.Dal.Mappers.Cases.EntityToBusinessModel;
    using DH.Helpdesk.Dal.Mappers.ProductArea.EntityToBusinessModel;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Cases;

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

            this.Bind<IEntityToBusinessModelMapper<CausingPart, CausingPartOverview>>()
                .To<CausingPartToOverviewMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CausingPartOverview, CausingPart>>()
                .To<CausingPartToEntityMapper>()
                .InSingletonScope();
        }
    }
}