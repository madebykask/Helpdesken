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
        public static string GetCaseTypeFullName(this int caseTypeId, IQueryable<CaseType> caseTypeQuery)
        {
            var res = string.Empty;
            var ct = caseTypeQuery.Where(c => c.Id == caseTypeId).FirstOrDefault();
            if (ct != null)
            {
                if (ct.Parent_CaseType_Id.HasValue)
                    res = ct.Parent_CaseType_Id.Value.GetCaseTypeFullName(caseTypeQuery) + " - " + ct.Name;
                else
                    res = ct.Name;
            }
            return res;
        }

        public static string GetProductAreaFullName(this int? productId, IQueryable<ProductArea> productAreaQuery)
        {
            if (productId == null)
                return string.Empty;

            var res = string.Empty;            
            var pa = productAreaQuery.Where(p => p.Id == productId).FirstOrDefault();
            if (pa != null)
            {
                if (pa.Parent_ProductArea_Id.HasValue)
                    res = pa.Parent_ProductArea_Id.GetProductAreaFullName(productAreaQuery) + " - " + pa.Name;
                else
                    res = pa.Name;
            }
            return res;
        }

        public static string GetOUFullName(this int? ouId, IQueryable<OU> organizationUnitQuery)
        {
            if (ouId == null)
                return string.Empty;

            var res = string.Empty;
            var ou = organizationUnitQuery.Where(o => o.Id == ouId).FirstOrDefault();
            if (ou != null)
            {
                if (ou.Parent_OU_Id.HasValue)
                    res = ou.Parent_OU_Id.GetOUFullName(organizationUnitQuery) + " - " + ou.Name;
                else
                    res = ou.Name;
            }
            return res;
        }


        public static string GetClosingReasonFullName(this int closingReasonId, IQueryable<FinishingCause> closingReasonQuery)
        {            
            var res = string.Empty;
            var cr = closingReasonQuery.Where(c => c.Id == closingReasonId).FirstOrDefault();
            if (cr != null)
            {
                if (cr.Parent_FinishingCause_Id.HasValue)
                    res = cr.Parent_FinishingCause_Id.Value.GetClosingReasonFullName(closingReasonQuery) + " - " + cr.Name;
                else
                    res = cr.Name;
            }
            return res;
        }

        public static List<FullCaseOverview> MapToCaseOverviews(this IQueryable<Case> query, 
                                                                IQueryable<CaseType> caseTypeQuery,
                                                                IQueryable<ProductArea> productAreaQuery,
                                                                IQueryable<FinishingCause> closingReasonQuery,
                                                                IQueryable<OU> organizationUnitQuery,
                                                                IQueryable<CaseStatistic> caseStatisticsQuery)
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
                                                         c => c.User.FirstName,
                                                         c => c.User.SurName,
                                                         // http://redmine.fastdev.se/issues/10639
                                                         /*c => c.Logs.Select(
                                                             l => l.Text_Internal + separator +
                                                                    l.Text_External + separator +                                                                     
                                                                    l.Charge + separator +
                                                                    l.LogFiles.FirstOrDefault().FileName + separator +
                                                                    l.FinishingDate + separator +
                                                                    l.FinishingTypeEntity.Name)*/
                                                         c => c.Logs.Select(l => l.Text_Internal),
                                                         c => c.Logs.Select(l => l.Text_External),
                                                         c => c.Logs.Select(l => l.Charge),
                                                         c => c.Logs.Select(l => l.LogFiles.FirstOrDefault().FileName),
                                                         c => c.Logs.Select(l => l.FinishingDate),
                                                         c => c.Logs.Select(l => l.FinishingTypeEntity.Id)
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
                        caseEntity.Ou = new OU { Name =  e.f18 };
                        caseEntity.System = new System { SystemName = e.f19 };
                        caseEntity.Impact = new Impact { Name = e.f20 };
                        caseEntity.Supplier = new Supplier { Name = e.f21 };
                        caseEntity.CaseResponsibleUser = new User { FirstName = e.f22, SurName = e.f23 };
                        caseEntity.Status = new Status { Name = e.f24 };
                        caseEntity.User = new User { FirstName = e.f25, SurName = e.f26 };
                        
                        // http://redmine.fastdev.se/issues/10639
                        /*caseEntity.Logs = ((List<string>)e.f27).Select(
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
                            }).ToList();*/

                        caseEntity.Logs = new List<Log>();
                        var internalLogNotes = (List<string>)e.f27;
                        var externalLogNotes = (List<string>)e.f28;
                        var charges = (List<int>)e.f29;
                        var files = (List<string>)e.f30;
                        var finishingDates = (List<DateTime?>)e.f31;
                        var finishingTypes = (List<int>)e.f32;
                        for (var i = 0; i < internalLogNotes.Count; i++)
                        {
                            var logFiles = new List<LogFile>();
                            if (!string.IsNullOrEmpty(files[i]))
                            {
                                logFiles.Add(new LogFile { FileName = files[i] });
                            }

                            var finishingType = new FinishingCause { Name = finishingTypes[i].GetClosingReasonFullName(closingReasonQuery) };

                            caseEntity.Logs.Add(new Log
                                                    {
                                                        Text_Internal = internalLogNotes[i],
                                                        Text_External = externalLogNotes[i],
                                                        Charge = charges[i],
                                                        LogFiles = logFiles,
                                                        FinishingDate = finishingDates[i],
                                                        FinishingTypeEntity = finishingType
                                                    });
                        }

                        
                        caseEntity.CaseType.Name = caseEntity.CaseType_Id.GetCaseTypeFullName(caseTypeQuery);
                        caseEntity.ProductArea.Name = caseEntity.ProductArea_Id.GetProductAreaFullName(productAreaQuery);
                        caseEntity.Ou.Name = caseEntity.OU_Id.GetOUFullName(organizationUnitQuery);

                        var caseStatistic = caseStatisticsQuery.Where(cs => cs.CaseId == caseEntity.Id).FirstOrDefault();                        
                        return CreateFullOverview(caseEntity, caseStatistic);
                    }).ToList();
        }

        private static FullCaseOverview CreateFullOverview(Case entity, CaseStatistic caseStatistic)
        {                        
            var id = entity.Id;
            var user = CreateUserOverview(entity);
            var computer = CreateComputerOverview(entity);
            var caseInfo = CreateCaseInfoOverview(entity, caseStatistic);
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

        private static CaseInfoOverview CreateCaseInfoOverview(Case entity, CaseStatistic caseStatistics)
        {
            var registratedBy = new UserName(entity.User.FirstName, entity.User.SurName);
            var attachedFile = entity.CaseFiles.Any() ? entity.CaseFiles.First().FileName : string.Empty;
            var solvedInTime = (caseStatistics != null ? caseStatistics.WasSolvedInTime : null);

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
                        attachedFile,
                        solvedInTime);
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
                                                l.FinishingTypeEntity.Name)).ToList();
            return new LogsOverview(logs);
        }
    }
}