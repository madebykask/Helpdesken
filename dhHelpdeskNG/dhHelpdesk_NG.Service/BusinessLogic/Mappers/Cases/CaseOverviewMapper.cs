namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Cases
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using DH.Helpdesk.BusinessData.Models.Case.CaseOverview;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;

    public static class CaseOverviewMapper
    {
        /*
        public static List<FullCaseOverview> MapToCaseOverviews(this IQueryable<Case> query)
        {
            var entities = query.SelectIncluding(new List<Expression<Func<Case, object>>>
                                                     {
                                                         c => c.ProductArea.Name,
                                                         c => c.LastChangedByUser.FirstName,
                                                         c => c.LastChangedByUser.SurName,
                                                         c => c.Administrator.FirstName,
                                                         c => c.Administrator.SurName,
                                                         c => c.CaseType.Name,
                                                         c => c.Workinggroup.WorkingGroupName,
                                                         c => c.Category.Name,
                                                         c => c.Customer.Name,
                                                         c => c.Department.DepartmentName,
                                                         c => c.RegLanguage.Name,
                                                         c => c.Urgency.Name,
                                                         c => c.Problem.Name,
                                                         c => c.Priority.Name,
                                                         c => c.StateSecondary.Name,
                                                         c => c.CaseFiles.Select(f => f.FileName),
                                                         c => c.Region.Name,
                                                         c => c.CausingPart.Name
                                                     })
                                                     .ToList();

            return entities.Select(
                e =>
                    {
                        var caseEntity = (Case)e.sourceObject;

                        caseEntity.ProductArea = new ProductArea { Name = e.f0 };
                        caseEntity.LastChangedByUser = new User { FirstName = e.f1, SurName = e.f2 };
                        caseEntity.Administrator = new User { FirstName = e.f3, SurName = e.f4 };
                        caseEntity.CaseType = new CaseType { Name = e.f5 };
                        caseEntity.Workinggroup = new WorkingGroupEntity { WorkingGroupName = e.f6 };
                        caseEntity.Category = new Category { Name = e.f7 };
                        caseEntity.Customer = new Customer { Name = e.f8 };

                        return CreateFullOverview(caseEntity);
                    }).ToList();
        }

        private static FullCaseOverview CreateFullOverview(Case entity)
        {
            var id = entity.Id;
            var user = CreateUserOverview(entity);
            var computer = CreateComputerOverview(entity);
            var caseInfo = CreateCaseInfoOverview(entity);
            var other = CreateOtherOverview(entity);
            var log = CreateLogOverview(entity);

            return new FullCaseOverview(
                        id,
                        user,
                        computer,
                        caseInfo,
                        other,
                        log);
        }

        private static UserOverview CreateUserOverview(Case entity)
        {
            
        }

        private static ComputerOverview CreateComputerOverview(Case entity)
        {
            
        }

        private static CaseInfoOverview CreateCaseInfoOverview(Case entity)
        {
            
        }

        private static OtherOverview CreateOtherOverview(Case entity)
        {
            
        }

        private static LogOverview CreateLogOverview(Case entity)
        {
            
        }*/
    }
}