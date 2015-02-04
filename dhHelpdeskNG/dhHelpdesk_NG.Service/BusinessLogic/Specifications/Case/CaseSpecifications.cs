namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Case
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Reports.Enums;
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

        public static IQueryable<Case> GetByDepartment(this IQueryable<Case> query, int? departmetId)
        {
            if (!departmetId.HasValue)
            {
                return query;
            }

            query = query.Where(c => c.Department_Id == departmetId);

            return query;
        }

        public static IQueryable<Case> GetByDepartments(this IQueryable<Case> query, List<int> departmetIds)
        {
            if (departmetIds == null || !departmetIds.Any())
            {
                return query;
            }

            query = query.Where(c => departmetIds.Contains(c.Department_Id.Value));

            return query;
        }

        public static IQueryable<Case> GetByCaseTypes(this IQueryable<Case> query, List<int> caseTypeIds)
        {
            if (caseTypeIds == null || !caseTypeIds.Any())
            {
                return query;
            }

            query = query.Where(c => caseTypeIds.Contains(c.CaseType_Id));

            return query;
        }

        public static IQueryable<Case> GetByWorkingGroup(this IQueryable<Case> query, int? workingGroupId)
        {
            if (!workingGroupId.HasValue)
            {
                return query;
            }

            query = query.Where(c => c.WorkingGroup_Id == workingGroupId);

            return query;
        }

        public static IQueryable<Case> GetByWorkingGroups(this IQueryable<Case> query, List<int> workingGroupIds)
        {
            if (workingGroupIds == null || !workingGroupIds.Any())
            {
                return query;
            }

            query = query.Where(c => workingGroupIds.Contains(c.WorkingGroup_Id.Value));

            return query;
        }

        public static IQueryable<Case> GetByAdministrator(this IQueryable<Case> query, int? administratorId)
        {
            if (!administratorId.HasValue)
            {
                return query;
            }

            query = query.Where(c => c.Performer_User_Id == administratorId);

            return query;
        }

        public static IQueryable<Case> GetByProductArea(this IQueryable<Case> query, int? productAreaId)
        {
            if (!productAreaId.HasValue)
            {
                return query;
            }

            query = query.Where(c => c.ProductArea_Id == productAreaId);

            return query;
        }

        public static IQueryable<Case> GetByProductAreas(this IQueryable<Case> query, List<int> productAreaIds)
        {
            if (productAreaIds == null || !productAreaIds.Any())
            {
                return query;
            }

            query = query.Where(c => productAreaIds.Contains(c.ProductArea_Id.Value));

            return query;
        }

        public static IQueryable<Case> GetByRegistrationPeriod(
                                        this IQueryable<Case> query,
                                        DateTime? from,
                                        DateTime? until)
        {
            if (from.HasValue)
            {
                query = query.Where(c => c.RegTime >= from);
            }

            if (until.HasValue)
            {
                query = query.Where(c => c.RegTime <= until);
            }

            return query;
        }

        public static IQueryable<Case> GetNotDeleted(this IQueryable<Case> query)
        {
            query = query.Where(c => c.Deleted == 0);

            return query;
        }

        public static IQueryable<Case> GetInProgress(this IQueryable<Case> query)
        {
            query = query.Where(c => c.FinishingDate == null && c.Deleted == 0);

            return query;
        }

        public static IQueryable<Case> GetByShowCases(this IQueryable<Case> query, ShowCases showCases)
        {
            if (showCases == ShowCases.CasesInProgress)
            {
                query = query.GetInProgress();
            }

            return query;
        } 
    }
}