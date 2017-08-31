namespace DH.Helpdesk.Web.NinjectModules.Modules
{
    using Ninject.Modules;
    using Ninject.Web.Common;

    internal class EFormModule : NinjectModule
    {
        public override void Load()
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DSN"].ConnectionString;

            Bind<ECT.Model.Abstract.IGlobalViewRepository>()
            .To<ECT.Model.Contrete.GlobalViewRepository>().InRequestScope().WithConstructorArgument("connectionString", connectionString);

            Bind<ECT.Model.Abstract.IContractRepository>()
            .To<ECT.Model.Contrete.ContractRepository>().InRequestScope().WithConstructorArgument("connectionString", connectionString);

            Bind<ECT.Model.Abstract.IUserRepository>()
            .To<ECT.Model.Contrete.UserRepository>().InRequestScope().WithConstructorArgument("connectionString", connectionString);

            Bind<ECT.Core.Service.IFileService>().To<ECT.Service.FileService>().InRequestScope();
        }
    }
}