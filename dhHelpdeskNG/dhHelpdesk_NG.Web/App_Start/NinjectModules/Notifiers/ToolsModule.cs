namespace dhHelpdesk_NG.Web.App_Start.NinjectModules.Notifiers
{
    using Ninject.Modules;

    using dhHelpdesk_NG.Service.Validators.Notifier;
    using dhHelpdesk_NG.Service.Validators.Notifier.Concrete;

    public sealed class ToolsModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<INotifierDynamicRulesValidator>().To<NotifierDynamicRulesValidator>().InSingletonScope();
        }
    }
}