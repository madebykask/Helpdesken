namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Accounts
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Accounts;
    using DH.Helpdesk.BusinessData.Enums.Accounts.Fields;
    using DH.Helpdesk.Domain.Accounts;

    public static class AccountSpecifications
    {
        public static IQueryable<Account> GetActivityTypeAccounts(this IQueryable<Account> query, int? activitTypeId)
        {
            if (activitTypeId.HasValue)
            {
                query = query.Where(x => x.AccountActivity_Id == activitTypeId);
            }

            return query;
        }

        public static IQueryable<Account> GetAdministratorAccounts(this IQueryable<Account> query, int? userId)
        {
            if (userId.HasValue)
            {
            }

            return query;
        }

        public static IQueryable<Account> GetAccountsBySearchString(this IQueryable<Account> query, string searchString)
        {
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                string str = searchString.Trim().ToLower();

                query =
                    query.Where(
                        x =>
                        x.OrdererId.Trim().ToLower().Contains(str) || x.OrdererFirstName.Trim().ToLower().Contains(str)
                        || x.OrdererLastName.Trim().ToLower().Contains(str)
                        || x.OrdererPhone.Trim().ToLower().Contains(str)
                        || x.OrdererEmail.Trim().ToLower().Contains(str) || x.UserId.Trim().ToLower().Contains(str)
                        || x.UserFirstName.Trim().ToLower().Contains(str)
                        || x.UserInitials.Trim().ToLower().Contains(str)
                        || x.UserLastName.Trim().ToLower().Contains(str)
                        || x.UserPersonalIdentityNumber.Trim().ToLower().Contains(str)
                        || x.UserPhone.Trim().ToLower().Contains(str) || x.UserExtension.Trim().ToLower().Contains(str)
                        || x.UserEMail.Trim().ToLower().Contains(str) || x.UserTitle.Trim().ToLower().Contains(str)
                        || x.UserLocation.Trim().ToLower().Contains(str)
                        || x.UserRoomNumber.Trim().ToLower().Contains(str)
                        || x.Department.DepartmentName.Trim().ToLower().Contains(str)
                        || x.OU.Name.Trim().ToLower().Contains(str)
                        || x.Department2.DepartmentName.Trim().ToLower().Contains(str)
                        || x.InfoUser.Trim().ToLower().Contains(str) || x.Responsibility.Trim().ToLower().Contains(str)
                        || x.Activity.Trim().ToLower().Contains(str) || x.Manager.Trim().ToLower().Contains(str)
                        || x.ReferenceNumber.Trim().ToLower().Contains(str)
                        || x.InventoryNumber.Trim().ToLower().Contains(str) || x.Info.Trim().ToLower().Contains(str)
                        || x.ContactId.Trim().ToLower().Contains(str) || x.ContactName.Trim().ToLower().Contains(str)
                        || x.ContactPhone.Trim().ToLower().Contains(str)
                        || x.ContactEMail.Trim().ToLower().Contains(str) || x.ContactId.Trim().ToLower().Contains(str)
                        || x.ContactName.Trim().ToLower().Contains(str) || x.ContactPhone.Trim().ToLower().Contains(str)
                        || x.ContactEMail.Trim().ToLower().Contains(str)
                        || x.DeliveryName.Trim().ToLower().Contains(str)
                        || x.DeliveryAddress.Trim().ToLower().Contains(str)
                        || x.DeliveryPhone.Trim().ToLower().Contains(str)
                        || x.DeliveryPostalAddress.Trim().ToLower().Contains(str)
                        || x.InfoProduct.Trim().ToLower().Contains(str) || x.InfoOther.Trim().ToLower().Contains(str)
                        || x.AccountFileName.Trim().ToLower().Contains(str));
            }

            return query;
        }

        public static IQueryable<Account> GetStateAccounts(this IQueryable<Account> query, AccountStates accountState)
        {
            if (accountState == AccountStates.All)
            {
                return query;
            }

            query = accountState == AccountStates.Active
                        ? query.Where(x => x.FinishingDate != null)
                        : query.Where(x => x.FinishingDate == null);

            return query;
        }
    }
}
