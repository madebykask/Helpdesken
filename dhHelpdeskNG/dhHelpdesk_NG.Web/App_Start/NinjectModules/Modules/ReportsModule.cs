namespace DH.Helpdesk.Web.NinjectModules.Modules
{
    using DH.Helpdesk.Web.Areas.Reports.Infrastructure;
    using DH.Helpdesk.Web.Areas.Reports.Infrastructure.Concrete;
    using DH.Helpdesk.Web.Areas.Reports.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Areas.Reports.Infrastructure.ModelFactories.Concrete;
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

            this.Bind<IReportModelFactory>().To<ReportModelFactory>().InSingletonScope();
            this.Bind<IReportsBuilder>().To<ReportsBuilder>().InSingletonScope();
            this.Bind<IPrintBuilder>().To<PrintBuilder>().InSingletonScope();
        }
    }
}