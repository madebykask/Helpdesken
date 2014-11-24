namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Accounts
{
    using System.Linq;
    using System.Security.Policy;

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
            //if (!string.IsNullOrWhiteSpace(searchString))
            //{
            //    string str = searchString.Trim().ToLower();

            //    query = query.Where(x=> 
            //                        x.UserId.Trim().ToLower().Contains(str) ||
            //                        x.UserFirstName.Trim().ToLower().Contains(str) ||
            //                        x.UserLastName.Trim().ToLower().Contains(str) ||
            //                        x.UserPhone.Trim().ToLower().Contains(str) ||
            //                        x.UserEMail.Trim().ToLower().Contains(str) ||

            //                        x.UserEMail.Trim().ToLower().Contains(str) ||
            //                        x.UserEMail.Trim().ToLower().Contains(str) ||
            //                        x.UserEMail.Trim().ToLower().Contains(str) ||
            //                        x.UserEMail.Trim().ToLower().Contains(str) ||
            //                        x.UserEMail.Trim().ToLower().Contains(str) ||
            //                        x.UserEMail.Trim().ToLower().Contains(str) ||
            //                        x.UserEMail.Trim().ToLower().Contains(str) ||
            //                        x.UserEMail.Trim().ToLower().Contains(str) ||
            //                        x.UserEMail.Trim().ToLower().Contains(str) ||

                    
            //        )

            //}

            return query;
        }
    }
}
