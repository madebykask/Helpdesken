using DH.Helpdesk.Common.Serializers;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Dal.Infrastructure.Concrete;
using Ninject.Modules;

namespace DH.Helpdesk.VBCSharpBridge.DI.Modules
{
    public class InfrastructureModule : NinjectModule
    {
        public override void Load()
        {
            
            Bind<IFilesStorage>().To<FilesStorage>().InSingletonScope();
            Bind<IJsonSerializeService>().To<JsonSerializeService>().InSingletonScope();
        }
    }
}