namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Cases
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using DH.Helpdesk.BusinessData.Models.Case.CaseOverview;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Cases;
    using DH.Helpdesk.Domain.Problems;

    public static class CaseOverviewMapper
    {
        public static List<FullCaseOverview> MapToCaseOverviews(this IQueryable<Case> query)
        {
            var separator = Guid.NewGuid().ToString();
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
                                                         c => c.CausingPart.Name,
                                                         c => c.Ou.Name,
                                                         c => c.System.SystemName,
                                                         c => c.Impact.Name,
                                                         c => c.Supplier.Name,
                                                         c => c.CaseResponsibleUser.FirstName,
                                                         c => c.CaseResponsibleUser.SurName,
                                                         c => c.Status.Name,
                                                         c => c.Logs.Select(
                                                             l => l.Text_Internal + separator +
                                                                    l.Text_External + separator +                                                                     
                                                                    l.Charge + separator +
                                                                    l.LogFiles.FirstOrDefault().FileName + separator +
                                                                    l.FinishingDate + separator +
                                                                    l.FinishingTypeEntity.Name),
                                                        c => c.User.FirstName,
                                                        c => c.User.SurName
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
                        caseEntity.Department = new Department { DepartmentName = e.f9 };
                        caseEntity.RegLanguage = new Language { Name = e.f10 };
                        caseEntity.Urgency = new Urgency { Name = e.f11 };
                        caseEntity.Problem = new Problem { Name = e.f12 };
                        caseEntity.Priority = new Priority { Name = e.f13 };
                        caseEntity.StateSecondary = new StateSecondary { Name = e.f14 };
                        caseEntity.CaseFiles = ((List<string>)e.f15).Select(f => new CaseFile { FileName = f }).ToList();
                        caseEntity.Region = new Region { Name = e.f16 };
                        caseEntity.CausingPart = new CausingPart { Name = e.f17 };
                        caseEntity.Ou = new OU { Name = e.f18 };
                        caseEntity.System = new System { SystemName = e.f19 };
                        caseEntity.Impact = new Impact { Name = e.f20 };
                        caseEntity.Supplier = new Supplier { Name = e.f21 };
                        caseEntity.CaseResponsibleUser = new User { FirstName = e.f22, SurName = e.f23 };
                        caseEntity.Status = new Status { Name = e.f24 };
                        caseEntity.Logs = ((List<string>)e.f25).Select(
                            l =>
                                {
                                    var values = l.Split(new[] { separator }, StringSplitOptions.None);
                                    var internalLogNote = values[0];
                                    var externalLogNote = values[1];
                                    var charge = int.Parse(values[2]);
                                    var files = new List<LogFile>();
                                    if (!string.IsNullOrEmpty(values[3]))
                                    {
                                        files.Add(new LogFile { FileName = values[3] });
                                    }

                                    DateTime? finishingDate = null;
                                    DateTime finishingDateVal;
                                    if (DateTime.TryParse(values[4], out finishingDateVal))
                                    {
                                        finishingDate = finishingDateVal;
                                    }

                                    var finishingType = new FinishingCause { Name = values[5] };

                                    return new Log
                                               {
                                                   Text_Internal = internalLogNote,
                                                   Text_External = externalLogNote,
                                                   Charge = charge,
                                                   LogFiles = files,
                                                   FinishingDate = finishingDate,
                                                   FinishingTypeEntity = finishingType
                                               };
                                }).ToList();
                        caseEntity.User = new User { FirstName = e.f26, SurName = e.f27 };

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
            return new UserOverview(
                        entity.ReportedBy,
                        entity.PersonsName,
                        entity.PersonsEmail,
                        entity.PersonsPhone,
                        entity.PersonsCellphone,
                        entity.Customer.Name,
                        entity.Region.Name,
                        entity.Department.DepartmentName,
                        entity.Ou.Name,
                        entity.Place,
                        entity.UserCode);
        }

        private static ComputerOverview CreateComputerOverview(Case entity)
        {
            return new ComputerOverview(
                        entity.InventoryNumber,
                        entity.InventoryType,
                        entity.InventoryLocation);
        }

        private static CaseInfoOverview CreateCaseInfoOverview(Case entity)
        {
            var registratedBy = new UserName(entity.User.FirstName, entity.User.SurName);
            var attachedFile = entity.CaseFiles.Any() ? entity.CaseFiles.First().FileName : string.Empty;

            return new CaseInfoOverview(
                        entity.CaseNumber,
                        entity.RegTime,
                        entity.ChangeTime,
                        registratedBy.GetFullName(),
                        entity.CaseType.Name,
                        entity.ProductArea.Name,
                        entity.System.SystemName,
                        entity.Urgency.Name,
                        entity.Impact.Name,
                        entity.Category.Name,
                        entity.Supplier.Name,
                        entity.InvoiceNumber,
                        entity.ReferenceNumber,
                        entity.Caption,
                        entity.Description,
                        entity.Miscellaneous,
                        entity.ContactBeforeAction.ToBool(),
                        entity.SMS.ToBool(),
                        entity.AgreedDate,
                        entity.Available,
                        entity.Cost,
                        attachedFile);
        }

        private static OtherOverview CreateOtherOverview(Case entity)
        {
            var caseResponsibleUser = new UserName(entity.CaseResponsibleUser.FirstName, entity.CaseResponsibleUser.SurName);
            var administrator = new UserName(entity.Administrator.FirstName, entity.Administrator.SurName);

            return new OtherOverview(
                        entity.Workinggroup.WorkingGroupName,
                        caseResponsibleUser.GetFullName(),
                        administrator.GetFullName(),
                        entity.Priority.Name,
                        entity.Status.Name,
                        entity.StateSecondary.Name,
                        entity.PlanDate,
                        entity.WatchDate,
                        entity.Verified.ToBool(),
                        entity.VerifiedDescription,
                        entity.SolutionRate,
                        entity.CausingPart.Name);
        }

        private static LogsOverview CreateLogOverview(Case entity)
        {
            var logs = entity.Logs.Select(l => new LogOverview(
                                                l.Text_Internal,
                                                l.Text_External,
                                                l.Charge.ToBool(),
                                                l.LogFiles.Any() ? l.LogFiles.First().FileName : string.Empty,
                                                entity.FinishingDescription,
                                                l.FinishingDate,
                                                l.FinishingTypeEntity.Name))
                                                .ToList();
            return new LogsOverview(logs);
        }
    }
}