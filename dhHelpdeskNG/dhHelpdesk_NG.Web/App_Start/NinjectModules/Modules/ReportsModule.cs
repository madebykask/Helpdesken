namespace DH.Helpdesk.Web.NinjectModules.Modules
{
    using DH.Helpdesk.Web.Areas.Reports.Infrastructure;
    using DH.Helpdesk.Web.Areas.Reports.Infrastructure.Concrete;
    using DH.Helpdesk.Web.Areas.Reports.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Areas.Reports.Infrastructure.ModelFactories.Concrete;

    using Ninject.Modules;

    internal class ReportsModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IReportModelFactory>().To<ReportModelFactory>().InSingletonScope();
            this.Bind<IReportsBuilder>().To<ReportsBuilder>().InSingletonScope();
            this.Bind<IPrintBuilder>().To<PrintBuilder>().InSingletonScope();
            this.Bind<IExcelBuilder>().To<ExcelBuilder>().InSingletonScope();
        }
    }
}