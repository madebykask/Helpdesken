namespace DH.Helpdesk.Mobile.NinjectModules.Modules
{
    using DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Reports;
    using DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Reports.Concrete;
    using DH.Helpdesk.Mobile.Infrastructure.Tools;
    using DH.Helpdesk.Mobile.Infrastructure.Tools.Concrete;

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