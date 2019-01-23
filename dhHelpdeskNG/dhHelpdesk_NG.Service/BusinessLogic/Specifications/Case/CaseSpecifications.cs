using System.Linq.Expressions;
using DH.Helpdesk.Domain.Computers;

namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Case
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Case.Fields;
    using DH.Helpdesk.BusinessData.Models.Reports.Enums;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Domain;

    using UserGroup = DH.Helpdesk.BusinessData.Enums.Admin.Users.UserGroup;

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
                query = query.Where(c => userIds.Contains(c.Performer_User_Id.Value));
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

        public static IQueryable<Case> GetByCaseType(this IQueryable<Case> query, int? caseTypeId)
        {
            if (!caseTypeId.HasValue)
            {
                return query;
            }

            query = query.Where(c => c.CaseType_Id == caseTypeId);

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

        public static IQueryable<Case> GetByResponsibleUser(this IQueryable<Case> query, int? responsibleUserId)
        {
            if (!responsibleUserId.HasValue)
            {
                return query;
            }

            query = query.Where(c => c.CaseResponsibleUser_Id == responsibleUserId);

            return query;
        }

        public static Expression<Func<Case, bool>> GetByAdministratorOrResponsibleUserExpression(
                                int administratorId,
                                int responsibleUserId)
        {
            Expression<Func<Case, bool>> exp = c => c.Performer_User_Id == administratorId || c.CaseResponsibleUser_Id == responsibleUserId;
            return exp;
        }

        public static Expression<Func<Case, bool>> GetByReportedByOrUserId(
                                string reportedBy,
                                int userId)
        {
            Expression<Func<Case, bool>> query = c => c.ReportedBy.Trim().ToLower() == reportedBy.Trim().ToLower() || c.CaseResponsibleUser_Id == userId;
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
            if (until.HasValue)
            {
                until = until.Value.AddMonths(1);
            }

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

        public static IQueryable<Case> GetByFinishingPeriod(
                                        this IQueryable<Case> query,
                                        DateTime? from,
                                        DateTime? until)
        {
            if (from.HasValue)
            {
                query = query.Where(c => c.FinishingDate >= from);
            }

            if (until.HasValue)
            {
                query = query.Where(c => c.FinishingDate <= until);
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

        public static IQueryable<Case> GetByRegistrationSource(
                                        this IQueryable<Case> query,
                                        CaseRegistrationSource registrationSource)
        {
            if (registrationSource != CaseRegistrationSource.Empty)
            {
                query = query.Where(c => c.RegistrationSource == (int)registrationSource);                
            }

            return query;
        }

        public static IQueryable<Case> GetFinished(this IQueryable<Case> query)
        {
            query = query.Where(c => c.FinishingDate.HasValue);

            return query;
        } 

        public static IQueryable<Case> GetActive(this IQueryable<Case> query)
        {
            query = query.Where(c => !c.FinishingDate.HasValue);

            return query;
        } 

        public static IQueryable<Case> GetWaitingForWatch(this IQueryable<Case> query)
        {
            query = query.Where(c => c.WatchDate.HasValue);

            return query;
        } 

        public static IQueryable<Case> HasLeadTime(this IQueryable<Case> query)
        {
            query = query.Where(c => c.LeadTime > 0);

            return query;
        }

        public static IQueryable<Case> GetRelatedCases(this IQueryable<Case> query, int caseId, string userId, UserOverview user)
        {
            query = query.Where(c => c.Id != caseId && c.ReportedBy.Trim().ToLower() == userId.Trim().ToLower());

            if (user.RestrictedCasePermission == 1 && user.UserGroupId == (int)UserGroup.Administrator)
            {
                query = query.Where(c => c.Performer_User_Id == user.Id || c.CaseResponsibleUser_Id == user.Id);
            }

            if (user.RestrictedCasePermission == 1 && user.UserGroupId == (int)UserGroup.User)
            {
                query = query.Where(c => c.ReportedBy.Trim().ToLower() == user.UserId.Trim().ToLower());
            }

            return query;
        }

        public static int GetRelatedInventoriesCount(this IQueryable<Computer> query, string userId, UserOverview user, int customerId)
        {
            query = query.Where(c => c.User.UserId.Trim().Equals(userId.Trim()) && c.Customer_Id == customerId);

            return query.Count();
        }

        public static IQueryable<Case> Search(
                                this IQueryable<Case> query,
                                int customerId,
                                List<int> departmentIds,
                                List<int> workingGroupIds,
                                List<int> caseTypeIds,
                                DateTime? periodFrom,
                                DateTime? periodUntil,
                                string text,
                                SortField sort,
                                int selectCount)
        {
            query = query
                        .GetByCustomer(customerId)
                        .GetByDepartments(departmentIds)
                        .GetByWorkingGroups(workingGroupIds)
                        .GetByCaseTypes(caseTypeIds)
                        .GetByRegistrationPeriod(periodFrom, periodUntil)
                        .GetByText(text)
                        .Sort(sort)
                        .GetNotDeleted();

            if (selectCount > 0)
            {
                query = query.Take(selectCount);
            }

            return query;
        }

        public static IQueryable<Case> GetByText(this IQueryable<Case> query, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return query;
            }

            var search = text.Trim().ToLower();

            query = query.Where(entity => 
                        entity.Id.ToString().Trim().ToLower().Contains(search) || 
                         
                        // User
                        entity.ReportedBy.Trim().ToLower().Contains(search) ||
                        entity.PersonsName.Trim().ToLower().Contains(search) ||
                        entity.PersonsEmail.Trim().ToLower().Contains(search) ||
                        entity.PersonsPhone.Trim().ToLower().Contains(search) ||
                        entity.PersonsCellphone.Trim().ToLower().Contains(search) ||
                        entity.Customer.Name.Trim().ToLower().Contains(search) ||
                        entity.Region.Name.Trim().ToLower().Contains(search) ||
                        entity.Department.DepartmentName.Trim().ToLower().Contains(search) ||
                        entity.Ou.Name.Trim().ToLower().Contains(search) ||
                        entity.Place.Trim().ToLower().Contains(search) || 
                        entity.UserCode.Trim().ToLower().Contains(search) || 

                        // Computer
                        entity.InventoryNumber.Trim().ToLower().Contains(search) ||
                        entity.InventoryType.Trim().ToLower().Contains(search) || 
                        entity.InventoryLocation.Trim().ToLower().Contains(search) ||  

                        // CaseInfo
                        entity.CaseNumber.ToString().Trim().ToLower().Contains(search) ||
                        entity.LastChangedByUser.FirstName.Trim().ToLower().Contains(search) ||
                        entity.LastChangedByUser.SurName.Trim().ToLower().Contains(search) ||
                        entity.CaseType.Name.Trim().ToLower().Contains(search) ||
                        entity.ProductArea.Name.Trim().ToLower().Contains(search) ||
                        entity.System.SystemName.Trim().ToLower().Contains(search) ||
                        entity.Urgency.Name.Trim().ToLower().Contains(search) ||
                        entity.Impact.Name.Trim().ToLower().Contains(search) ||
                        entity.Category.Name.Trim().ToLower().Contains(search) ||
                        entity.Supplier.Name.Trim().ToLower().Contains(search) ||
                        entity.InvoiceNumber.Trim().ToLower().Contains(search) ||
                        entity.ReferenceNumber.Trim().ToLower().Contains(search) ||
                        entity.Caption.Trim().ToLower().Contains(search) ||
                        entity.Description.Trim().ToLower().Contains(search) ||
                        entity.Miscellaneous.Trim().ToLower().Contains(search) ||
                        entity.Available.Trim().ToLower().Contains(search) ||
                        entity.Cost.ToString().Trim().ToLower().Contains(search) ||
                        entity.CaseFiles.Any(f => f.FileName.Trim().ToLower().Contains(search)) ||
 
                        // Other
                        entity.Workinggroup.WorkingGroupName.Trim().ToLower().Contains(search) ||
                        entity.CaseResponsibleUser.FirstName.Trim().ToLower().Contains(search) ||
                        entity.CaseResponsibleUser.SurName.Trim().ToLower().Contains(search) ||
                        entity.Administrator.FirstName.Trim().ToLower().Contains(search) ||
                        entity.Administrator.SurName.Trim().ToLower().Contains(search) ||
                        entity.Priority.Name.Trim().ToLower().Contains(search) ||
                        entity.Status.Name.Trim().ToLower().Contains(search) ||
                        entity.StateSecondary.Name.Trim().ToLower().Contains(search) ||
                        entity.VerifiedDescription.Trim().ToLower().Contains(search) ||
                        entity.SolutionRate.Trim().ToLower().Contains(search) ||
                        entity.CausingPart.Name.Trim().ToLower().Contains(search) ||
 
                        // Log
                        entity.Logs.Any(l => 
                                l.Text_Internal.Trim().ToLower().Contains(search) ||
                                l.Text_External.Trim().ToLower().Contains(search) ||
                                l.FinishingTypeEntity.Name.Trim().ToLower().Contains(search) ||
                                l.LogFiles.Any(f => f.FileName.Trim().ToLower().Contains(search))));

            return query;
        }

        public static IQueryable<Case> GetByCaseNumber(this IQueryable<Case> query, decimal caseNumber)
        {
            query = query.Where(c => c.CaseNumber == caseNumber);

            return query;
        }

        #region Sort

        public static IQueryable<Case> Sort(this IQueryable<Case> query, SortField sort)
        {
            if (sort == null)
            {
                return query;
            }

            switch (sort.SortBy)
            {
                case SortBy.Ascending:
                    // User
                    if (sort.Name == UserFields.User)
                    {
                        query = query.OrderBy(c => c.ReportedBy);
                    }
                    else if (sort.Name == UserFields.Notifier)
                    {
                        query = query.OrderBy(c => c.PersonsName);
                    }
                    else if (sort.Name == UserFields.Email)
                    {
                        query = query.OrderBy(c => c.PersonsEmail);
                    }
                    else if (sort.Name == UserFields.Phone)
                    {
                        query = query.OrderBy(c => c.PersonsPhone);
                    }
                    else if (sort.Name == UserFields.CellPhone)
                    {
                        query = query.OrderBy(c => c.PersonsCellphone);
                    }
                    else if (sort.Name == UserFields.Customer)
                    {
                        query = query.OrderBy(c => c.Customer.Name);
                    }
                    else if (sort.Name == UserFields.Region)
                    {
                        query = query.OrderBy(c => c.Region.Name);
                    }
                    else if (sort.Name == UserFields.Department)
                    {
                        query = query.OrderBy(c => c.Department.DepartmentName);
                    }
                    else if (sort.Name == UserFields.Unit)
                    {
                        query = query.OrderBy(c => c.Ou.Name);
                    }
                    else if (sort.Name == UserFields.Place)
                    {
                        query = query.OrderBy(c => c.Place);
                    }
                    else if (sort.Name == UserFields.OrdererCode)
                    {
                        query = query.OrderBy(c => c.UserCode);
                    }

                    // Computer
                    if (sort.Name == ComputerFields.PcNumber)
                    {
                        query = query.OrderBy(c => c.InventoryNumber);
                    }
                    else if (sort.Name == ComputerFields.ComputerType)
                    {
                        query = query.OrderBy(c => c.InventoryType);
                    }
                    else if (sort.Name == ComputerFields.Place)
                    {
                        query = query.OrderBy(c => c.InventoryLocation);
                    }

                    // CaseInfo
                    if (sort.Name == CaseInfoFields.Case)
                    {
                        query = query.OrderBy(c => c.CaseNumber);
                    }
                    else if (sort.Name == CaseInfoFields.RegistrationDate)
                    {
                        query = query.OrderBy(c => c.RegTime);
                    }
                    else if (sort.Name == CaseInfoFields.ChangeDate)
                    {
                        query = query.OrderBy(c => c.ChangeTime);
                    }
                    else if (sort.Name == CaseInfoFields.RegistratedBy)
                    {
                        query = query.OrderBy(c => c.User.SurName);
                    }
                    else if (sort.Name == CaseInfoFields.CaseType)
                    {
                        query = query.OrderBy(c => c.CaseType.Name);
                    }
                    else if (sort.Name == CaseInfoFields.ProductArea)
                    {
                        query = query.OrderBy(c => c.ProductArea.Name);
                    }
                    else if (sort.Name == CaseInfoFields.System)
                    {
                        query = query.OrderBy(c => c.System.SystemName);
                    }
                    else if (sort.Name == CaseInfoFields.UrgentDegree)
                    {
                        query = query.OrderBy(c => c.Urgency.Name);
                    }
                    else if (sort.Name == CaseInfoFields.Impact)
                    {
                        query = query.OrderBy(c => c.Impact.Name);
                    }
                    else if (sort.Name == CaseInfoFields.Category)
                    {
                        query = query.OrderBy(c => c.Category.Name);
                    }
                    else if (sort.Name == CaseInfoFields.Supplier)
                    {
                        query = query.OrderBy(c => c.Supplier.Name);
                    }
                    else if (sort.Name == CaseInfoFields.InvoiceNumber)
                    {
                        query = query.OrderBy(c => c.InvoiceNumber);
                    }
                    else if (sort.Name == CaseInfoFields.ReferenceNumber)
                    {
                        query = query.OrderBy(c => c.ReferenceNumber);
                    }
                    else if (sort.Name == CaseInfoFields.Caption)
                    {
                        query = query.OrderBy(c => c.Caption);
                    }
                    else if (sort.Name == CaseInfoFields.Description)
                    {
                        query = query.OrderBy(c => c.Description);
                    }
                    else if (sort.Name == CaseInfoFields.Other)
                    {
                        query = query.OrderBy(c => c.Miscellaneous);
                    }
                    else if (sort.Name == CaseInfoFields.PhoneContact)
                    {
                        query = query.OrderBy(c => c.ContactBeforeAction);
                    }
                    else if (sort.Name == CaseInfoFields.Sms)
                    {
                        query = query.OrderBy(c => c.SMS);
                    }
                    else if (sort.Name == CaseInfoFields.AgreedDate)
                    {
                        query = query.OrderBy(c => c.AgreedDate);
                    }
                    else if (sort.Name == CaseInfoFields.Available)
                    {
                        query = query.OrderBy(c => c.Available);
                    }
                    else if (sort.Name == CaseInfoFields.Cost)
                    {
                        query = query.OrderBy(c => c.Cost);
                    }
                    else if (sort.Name == CaseInfoFields.AttachedFile)
                    {
                        query = query.OrderBy(c => c.CaseFiles.Count);
                    }

                    // Other
                    if (sort.Name == OtherFields.WorkingGroup)
                    {
                        query = query.OrderBy(c => c.Workinggroup.WorkingGroupName);
                    }
                    else if (sort.Name == OtherFields.Responsible)
                    {
                        query = query.OrderBy(c => c.CaseResponsibleUser.SurName);
                    }
                    else if (sort.Name == OtherFields.Administrator)
                    {
                        query = query.OrderBy(c => c.Administrator.SurName);
                    }
                    else if (sort.Name == OtherFields.Priority)
                    {
                        query = query.OrderBy(c => c.Priority.Name);
                    }
                    else if (sort.Name == OtherFields.State)
                    {
                        query = query.OrderBy(c => c.Status.Name);
                    }
                    else if (sort.Name == OtherFields.SubState)
                    {
                        query = query.OrderBy(c => c.StateSecondary.Name);
                    }
                    else if (sort.Name == OtherFields.PlannedActionDate)
                    {
                        query = query.OrderBy(c => c.PlanDate);
                    }
                    else if (sort.Name == OtherFields.WatchDate)
                    {
                        query = query.OrderBy(c => c.WatchDate);
                    }
                    else if (sort.Name == OtherFields.Verified)
                    {
                        query = query.OrderBy(c => c.Verified);
                    }
                    else if (sort.Name == OtherFields.VerifiedDescription)
                    {
                        query = query.OrderBy(c => c.VerifiedDescription);
                    }
                    else if (sort.Name == OtherFields.SolutionRate)
                    {
                        query = query.OrderBy(c => c.SolutionRate);
                    }
                    else if (sort.Name == OtherFields.CausingPart)
                    {
                        query = query.OrderBy(c => c.CausingPart.Name);
                    }

                    // Log
                    if (sort.Name == LogFields.FinishingDate)
                    {
                        query = query.OrderBy(c => c.FinishingDate);
                    }

                    if (sort.Name == LogFields.InternalLogNote ||
                        sort.Name == LogFields.ExternalLogNote ||
                        sort.Name == LogFields.Debiting ||
                        sort.Name == LogFields.AttachedFile ||
                        sort.Name == LogFields.FinishingDescription ||
                        sort.Name == LogFields.FinishingCause)
                    {
                        query = query.OrderBy(c => c.Logs.Count);
                    }

                    break;
                case SortBy.Descending:
                    // User
                    if (sort.Name == UserFields.User)
                    {
                        query = query.OrderByDescending(c => c.ReportedBy);
                    }
                    else if (sort.Name == UserFields.Notifier)
                    {
                        query = query.OrderByDescending(c => c.PersonsName);
                    }
                    else if (sort.Name == UserFields.Email)
                    {
                        query = query.OrderByDescending(c => c.PersonsEmail);
                    }
                    else if (sort.Name == UserFields.Phone)
                    {
                        query = query.OrderByDescending(c => c.PersonsPhone);
                    }
                    else if (sort.Name == UserFields.CellPhone)
                    {
                        query = query.OrderByDescending(c => c.PersonsCellphone);
                    }
                    else if (sort.Name == UserFields.Customer)
                    {
                        query = query.OrderByDescending(c => c.Customer.Name);
                    }
                    else if (sort.Name == UserFields.Region)
                    {
                        query = query.OrderByDescending(c => c.Region.Name);
                    }
                    else if (sort.Name == UserFields.Department)
                    {
                        query = query.OrderByDescending(c => c.Department.DepartmentName);
                    }
                    else if (sort.Name == UserFields.Unit)
                    {
                        query = query.OrderByDescending(c => c.Ou.Name);
                    }
                    else if (sort.Name == UserFields.Place)
                    {
                        query = query.OrderByDescending(c => c.Place);
                    }
                    else if (sort.Name == UserFields.OrdererCode)
                    {
                        query = query.OrderByDescending(c => c.UserCode);
                    }

                    // Computer
                    if (sort.Name == ComputerFields.PcNumber)
                    {
                        query = query.OrderByDescending(c => c.InventoryNumber);
                    }
                    else if (sort.Name == ComputerFields.ComputerType)
                    {
                        query = query.OrderByDescending(c => c.InventoryType);
                    }
                    else if (sort.Name == ComputerFields.Place)
                    {
                        query = query.OrderByDescending(c => c.InventoryLocation);
                    }

                    // CaseInfo
                    if (sort.Name == CaseInfoFields.Case)
                    {
                        query = query.OrderByDescending(c => c.CaseNumber);
                    }
                    else if (sort.Name == CaseInfoFields.RegistrationDate)
                    {
                        query = query.OrderByDescending(c => c.RegTime);
                    }
                    else if (sort.Name == CaseInfoFields.ChangeDate)
                    {
                        query = query.OrderByDescending(c => c.ChangeTime);
                    }
                    else if (sort.Name == CaseInfoFields.RegistratedBy)
                    {
                        query = query.OrderByDescending(c => c.User.SurName);
                    }
                    else if (sort.Name == CaseInfoFields.CaseType)
                    {
                        query = query.OrderByDescending(c => c.CaseType.Name);
                    }
                    else if (sort.Name == CaseInfoFields.ProductArea)
                    {
                        query = query.OrderByDescending(c => c.ProductArea.Name);
                    }
                    else if (sort.Name == CaseInfoFields.System)
                    {
                        query = query.OrderByDescending(c => c.System.SystemName);
                    }
                    else if (sort.Name == CaseInfoFields.UrgentDegree)
                    {
                        query = query.OrderByDescending(c => c.Urgency.Name);
                    }
                    else if (sort.Name == CaseInfoFields.Impact)
                    {
                        query = query.OrderByDescending(c => c.Impact.Name);
                    }
                    else if (sort.Name == CaseInfoFields.Category)
                    {
                        query = query.OrderByDescending(c => c.Category.Name);
                    }
                    else if (sort.Name == CaseInfoFields.Supplier)
                    {
                        query = query.OrderByDescending(c => c.Supplier.Name);
                    }
                    else if (sort.Name == CaseInfoFields.InvoiceNumber)
                    {
                        query = query.OrderByDescending(c => c.InvoiceNumber);
                    }
                    else if (sort.Name == CaseInfoFields.ReferenceNumber)
                    {
                        query = query.OrderByDescending(c => c.ReferenceNumber);
                    }
                    else if (sort.Name == CaseInfoFields.Caption)
                    {
                        query = query.OrderByDescending(c => c.Caption);
                    }
                    else if (sort.Name == CaseInfoFields.Description)
                    {
                        query = query.OrderByDescending(c => c.Description);
                    }
                    else if (sort.Name == CaseInfoFields.Other)
                    {
                        query = query.OrderByDescending(c => c.Miscellaneous);
                    }
                    else if (sort.Name == CaseInfoFields.PhoneContact)
                    {
                        query = query.OrderByDescending(c => c.ContactBeforeAction);
                    }
                    else if (sort.Name == CaseInfoFields.Sms)
                    {
                        query = query.OrderByDescending(c => c.SMS);
                    }
                    else if (sort.Name == CaseInfoFields.AgreedDate)
                    {
                        query = query.OrderByDescending(c => c.AgreedDate);
                    }
                    else if (sort.Name == CaseInfoFields.Available)
                    {
                        query = query.OrderByDescending(c => c.Available);
                    }
                    else if (sort.Name == CaseInfoFields.Cost)
                    {
                        query = query.OrderByDescending(c => c.Cost);
                    }
                    else if (sort.Name == CaseInfoFields.AttachedFile)
                    {
                        query = query.OrderByDescending(c => c.CaseFiles.Count);
                    }

                    // Other
                    if (sort.Name == OtherFields.WorkingGroup)
                    {
                        query = query.OrderByDescending(c => c.Workinggroup.WorkingGroupName);
                    }
                    else if (sort.Name == OtherFields.Responsible)
                    {
                        query = query.OrderByDescending(c => c.CaseResponsibleUser.SurName);
                    }
                    else if (sort.Name == OtherFields.Administrator)
                    {
                        query = query.OrderByDescending(c => c.Administrator.SurName);
                    }
                    else if (sort.Name == OtherFields.Priority)
                    {
                        query = query.OrderByDescending(c => c.Priority.Name);
                    }
                    else if (sort.Name == OtherFields.State)
                    {
                        query = query.OrderByDescending(c => c.Status.Name);
                    }
                    else if (sort.Name == OtherFields.SubState)
                    {
                        query = query.OrderByDescending(c => c.StateSecondary.Name);
                    }
                    else if (sort.Name == OtherFields.PlannedActionDate)
                    {
                        query = query.OrderByDescending(c => c.PlanDate);
                    }
                    else if (sort.Name == OtherFields.WatchDate)
                    {
                        query = query.OrderByDescending(c => c.WatchDate);
                    }
                    else if (sort.Name == OtherFields.Verified)
                    {
                        query = query.OrderByDescending(c => c.Verified);
                    }
                    else if (sort.Name == OtherFields.VerifiedDescription)
                    {
                        query = query.OrderByDescending(c => c.VerifiedDescription);
                    }
                    else if (sort.Name == OtherFields.SolutionRate)
                    {
                        query = query.OrderByDescending(c => c.SolutionRate);
                    }
                    else if (sort.Name == OtherFields.CausingPart)
                    {
                        query = query.OrderByDescending(c => c.CausingPart.Name);
                    }

                    // Log
                    if (sort.Name == LogFields.FinishingDate)
                    {
                        query = query.OrderByDescending(c => c.FinishingDate);
                    }

                    if (sort.Name == LogFields.InternalLogNote ||
                        sort.Name == LogFields.ExternalLogNote ||
                        sort.Name == LogFields.Debiting ||
                        sort.Name == LogFields.AttachedFile ||
                        sort.Name == LogFields.FinishingDescription ||
                        sort.Name == LogFields.FinishingCause)
                    {
                        query = query.OrderByDescending(c => c.Logs.Count);
                    }

                    break;
            }

            if (query is IOrderedQueryable)
            {
                query = ((IOrderedQueryable<Case>)query).ThenBy(c => c.Logs.Count);                
            }

            return query;
        }

        #endregion
    }
}