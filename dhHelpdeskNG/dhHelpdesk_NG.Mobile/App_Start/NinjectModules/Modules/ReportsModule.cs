namespace DH.Helpdesk.Web.NinjectModules.Modules
{
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Reports;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Reports.Concrete;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Infrastructure.Tools.Concrete;

    using Ninject.Modules;
    using Ninject.Web.Common;

    internal class ReportsModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IReportsModelFactory>().To<ReportsModelFactory>().InRequestScope();
            this.Bind<IReportsHelper>().To<ReportsHelper>().InRequestScope();
        }
    }
}