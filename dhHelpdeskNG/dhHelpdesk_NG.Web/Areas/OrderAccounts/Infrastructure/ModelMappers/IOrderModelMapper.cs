namespace DH.Helpdesk.Web.Areas.OrderAccounts.Infrastructure.ModelMappers
{
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.ModelEdit;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Edit;
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.Services.Response.Account;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit;

    public interface IOrderModelMapper
    {
        AccountModel BuildViewModel(AccountForEdit model, AccountOptionsResponse options, AccountFieldsSettingsForModelEdit settings, HeadersFieldSettings headers);

        AccountModel BuildViewModel(int activityId, AccountOptionsResponse options, AccountFieldsSettingsForModelEdit settings, UserOverview userOverview, HeadersFieldSettings headers);
    }
}