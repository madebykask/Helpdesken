namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Cases
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.Domain;

    public static class CasesMapper
    {
        public static List<RelatedCase> MapToRelatedCases(this IQueryable<Case> query)
        {
            var entities = query.Select(c => new
                                        {
                                             c.Id,
                                             c.CaseNumber,
                                             c.RegTime,
                                             Status = c.Status.Name,
                                             c.Caption,
                                             c.Description,
                                             c.FinishingDate,
                                             c.ApprovedDate,
                                             c.CaseType.RequireApproving
                                        })
                                .OrderByDescending(c => c.RegTime)
                                .ToList();

            return entities.Select(
                            c => new RelatedCase(
                                            c.Id, 
                                            c.CaseNumber, 
                                            c.RegTime, 
                                            c.Status, 
                                            c.Caption, 
                                            c.Description,
                                            GetCaseIcon(c.FinishingDate, c.ApprovedDate, c.RequireApproving))).ToList();
        }

        public static List<int> MapToUserCaseIds(
                                IQueryable<Case> cases,
                                IQueryable<DepartmentUser> userDepartments,
                                IQueryable<User> users,
                                IQueryable<Customer> customers,
                                IQueryable<CustomerUser> customerUsers)
        {
            var entities = (from c in cases
                            join cus in customers on c.Customer_Id equals cus.Id
                            join cu in customerUsers on cus.Id equals cu.Customer_Id
                            join u in users on cu.User_Id equals u.Id
                            join ud in userDepartments on c.Department_Id equals ud.Department_Id into udgj
                            from department in udgj.DefaultIfEmpty()
                            select new
                            {
                                c.Id
                            })
                            .Distinct()
                            .ToList();

            return entities.Select(c => c.Id).ToList();
        } 

        public static GlobalEnums.CaseIcon GetCaseIcon(
                                            DateTime? finishingDate,
                                            DateTime? approvedDate,
                                            int requireApproving)
        {
            if (finishingDate.HasValue)
            {
                if (!approvedDate.HasValue && requireApproving == 1)
                {
                    return GlobalEnums.CaseIcon.FinishedNotApproved;
                }

                return GlobalEnums.CaseIcon.Finished;
            }

            return GlobalEnums.CaseIcon.Normal;
        }
    }
}