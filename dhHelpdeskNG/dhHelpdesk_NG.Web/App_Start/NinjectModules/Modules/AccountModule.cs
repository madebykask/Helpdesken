namespace DH.Helpdesk.Web.NinjectModules.Modules
{
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Account;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Account.Concrete;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Accounts;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Accounts.Concrete;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Infrastructure.BusinessModelMappers;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Infrastructure.BusinessModelMappers.Concrete;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Infrastructure.ModelMappers;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Infrastructure.ModelMappers.Concrete;

    using Ninject.Modules;

    public class AccountModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IOrderModelMapper>().To<OrderModelMapper>().InSingletonScope();
            this.Bind<ISettingsViewModelMapper>().To<SettingsViewModelMapper>().InSingletonScope();

            this.Bind<IAccountDtoMapper>().To<AccountDtoMapper>().InSingletonScope();
            this.Bind<ISettingsDtoMapper>().To<SettingsDtoMapper>().InSingletonScope();

            this.Bind<IAccountValidator>().To<AccountValidator>().InSingletonScope();
            this.Bind<IAccountRestorer>().To<AccountRestorer>().InSingletonScope();
        }
    }
}