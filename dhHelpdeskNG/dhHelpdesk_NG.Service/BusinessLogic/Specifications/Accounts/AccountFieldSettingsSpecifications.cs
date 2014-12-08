namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Accounts
{
    using System.Linq;

    using DH.Helpdesk.Domain.Accounts;

    public static class AccountFieldSettingsSpecifications
    {
        public static IQueryable<AccountFieldSettings> GetActivityTypeSettings(this IQueryable<AccountFieldSettings> query, int? activitTypeId)
        {
            if (activitTypeId.HasValue)
            {
                query = query.Where(x => x.AccountActivity_Id == activitTypeId);
            }

            return query;
        }
    }
}
