namespace DH.Helpdesk.Web.NinjectModules.Modules
{
    using Ninject.Modules;
    using Ninject.Web.Common;

    internal class EFormModule : NinjectModule
    {
        public override void Load()
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DSN"].ConnectionString;

            Bind<EForm.Model.Abstract.IGlobalViewRepository>()
            .To<EForm.Model.Contrete.GlobalViewRepository>().InRequestScope().WithConstructorArgument("connectionString", connectionString);

            Bind<EForm.Model.Abstract.IContractRepository>()
            .To<EForm.Model.Contrete.ContractRepository>().InRequestScope().WithConstructorArgument("connectionString", connectionString);

            Bind<EForm.Model.Abstract.IUserRepository>()
            .To<EForm.Model.Contrete.UserRepository>().InRequestScope().WithConstructorArgument("connectionString", connectionString);

            Bind<EForm.Core.Service.IFileService>().To<EForm.Service.FileService>().InRequestScope();
        }
    }
}