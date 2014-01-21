namespace dhHelpdesk_NG.Web.NinjectModules.Notifiers
{
    using dhHelpdesk_NG.Service.Validators.Notifier;
    using dhHelpdesk_NG.Service.Validators.Notifier.Concrete;

    using Ninject.Modules;

    public sealed class ToolsModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<INotifierDynamicRulesValidator>().To<NotifierDynamicRulesValidator>().InSingletonScope();
        }
    }
}