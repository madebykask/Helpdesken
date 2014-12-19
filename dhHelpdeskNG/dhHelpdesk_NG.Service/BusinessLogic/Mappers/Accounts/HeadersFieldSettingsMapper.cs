namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Accounts
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.ModelEdit;
    using DH.Helpdesk.Domain.Accounts;

    public static class HeadersFieldSettingsMapper
    {
        public static HeadersFieldSettings ExtractHeadersFieldSettings(this IQueryable<AccountActivity> query)
        {
            var anonymus =
                query.Select(
                    x =>
                    new
                        {
                            x.OrdererInfo,
                            x.UserInfo,
                            x.AccountInfo,
                            x.ContactInfo,
                            x.DeliveryInfo,
                            x.ProgramInfo
                        }).Single();

            var model = new HeadersFieldSettings(
                anonymus.OrdererInfo,
                anonymus.UserInfo,
                anonymus.AccountInfo,
                anonymus.ContactInfo,
                anonymus.DeliveryInfo,
                anonymus.ProgramInfo);

            return model;
        }
    }
}