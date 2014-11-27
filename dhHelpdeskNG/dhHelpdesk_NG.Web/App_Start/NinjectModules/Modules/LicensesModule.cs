namespace DH.Helpdesk.Web.NinjectModules.Modules
{
    using DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories.Concrete;

    using Ninject.Modules;

    public sealed class LicensesModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IProductsModelFactory>().To<ProductsModelFactory>().InSingletonScope();
            this.Bind<ILicensesModelFactory>().To<LicensesModelFactory>().InSingletonScope();
            this.Bind<IVendorsModelFactory>().To<VendorsModelFactory>().InSingletonScope();
            this.Bind<IManufacturersModelFactory>().To<ManufacturersModelFactory>().InSingletonScope();
            this.Bind<IApplicationsModelFactory>().To<ApplicationsModelFactory>().InSingletonScope();
            this.Bind<IComputersModelFactory>().To<ComputersModelFactory>().InSingletonScope();
        }
    }
}