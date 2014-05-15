namespace DH.Helpdesk.Web.NinjectModules.Modules
{
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Reports;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Reports.Concrete;

    using Ninject.Modules;

    internal class ReportsModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IReportsModelFactory>().To<ReportsModelFactory>().InSingletonScope();
        }
    }
}