namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Case
{
    using System;
    using System.Linq;

    using DH.Helpdesk.Domain;

    public static class CaseSpecifications
    {
        public static IQueryable<Case> GetAvaliableCases(this IQueryable<Case> query)
        {
            const int MinEmailLength = 3;
            const int IsDeleted = 0;

            query =
                query.Where(
                    c => c.Deleted == IsDeleted && c.FinishingDate != null && c.PersonsEmail.Length > MinEmailLength);

            return query;
        }

        public static IQueryable<Case> GetAvaliableCustomerCases(this IQueryable<Case> query, int customerId)
        {
            query = query.GetByCustomer(customerId).GetAvaliableCases();

            return query;
        }

        public static IQueryable<Case> GetDepartmentsCases(this IQueryable<Case> query, int[] departments)
        {
            if (departments != null && departments.Any())
            {
                query = query.Where(
                    c => c.Department_Id != null && departments.ToList().Contains(c.Department_Id.Value));
            }

            return query;
        }

        public static IQueryable<Case> GetCaseTypesCases(this IQueryable<Case> query, int[] caseTypes)
        {
            if (caseTypes != null && caseTypes.Any())
            {
                query = query.Where(c => caseTypes.ToList().Contains(c.CaseType_Id));
            }

            return query;
        }

        public static IQueryable<Case> GetProductAreasCases(this IQueryable<Case> query, int[] productAreas)
        {
            if (productAreas != null && productAreas.Any())
            {
                query =
                    query.Where(c => c.ProductArea_Id != null && productAreas.ToList().Contains(c.ProductArea_Id.Value));
            }

            return query;
        }

        public static IQueryable<Case> GetUserCases(this IQueryable<Case> query, IQueryable<int> userIds)
        {
            if (userIds != null)
            {
                query = query.Where(c => userIds.Contains(c.Performer_User_Id));
            }

            return query;
        }

        public static IQueryable<Case> GetCasesFromFinishingDate(this IQueryable<Case> query, DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                query = query.Where(x => x.FinishingDate >= dateTime);
            }

            return query;
        }

        public static IQueryable<Case> GetCasesToFinishingDate(this IQueryable<Case> query, DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                query = query.Where(x => x.FinishingDate <= dateTime);
            }

            return query;
        }

        public static IQueryable<Case> GetCustomersCases(this IQueryable<Case> query, int[] customerIds)
        {
            if (customerIds == null || !customerIds.Any())
            {
                return query;
            }

            query = query.Where(c => customerIds.Contains(c.Customer_Id));

            return query;
        }
    }
}