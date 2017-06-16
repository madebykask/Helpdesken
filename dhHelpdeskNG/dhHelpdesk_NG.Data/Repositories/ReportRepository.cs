using System.Data.Entity;
using DH.Helpdesk.BusinessData.Models.Reports;
using DH.Helpdesk.Common.Tools;

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Infrastructure.Translate;
    using DH.Helpdesk.Domain;
    using System;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.BusinessData.Models.Case.CaseOverview;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.ReportGenerator;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Dal.Infrastructure.Helpers;

    #region REPORT

  
    public interface IReportRepository : IRepository<Report>
    {
        List<ReportGeneratorFields> GetCaseList(
          int customerId,
          List<int> departmentIds,
          List<int> ouIds,
          List<int> workingGroupIds,
          List<int> productAreaIds,
          List<int> administratorIds,
          List<int> caseStatusIds,
          List<int> caseTypeId,
          DateTime? periodFrom,
          DateTime? periodUntil,
           DateTime? closeFrom,
           DateTime? closeTo);

        Dictionary<DateTime, int> GetCaseAggregation(
            int customerId,
            List<int> departmentIds,
            List<int> ouIds,
            List<int> workingGroupIds,
            List<int> productAreaIds,
            List<int> administratorIds,
            List<int> caseStatusIds,
            List<int> caseTypeId,
            DateTime? periodFrom,
            DateTime? periodUntil,
            DateTime? closeFrom,
            DateTime? closeTo);

    }
    public class ReportRepository : RepositoryBase<Report>, IReportRepository
    {
        public ReportRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ReportGeneratorFields> GetCaseList(
           int customerId,
           List<int> departmentIds,
           List<int> ouIds,
           List<int> workingGroupIds,
           List<int> productAreaIds,
           List<int> administratorIds,
           List<int> caseStatusIds,
           List<int> caseTypeId,
           DateTime? periodFrom,
           DateTime? periodUntil,
           DateTime? closeFrom,
           DateTime? closeTo)
        {
            var query = GetQuery(customerId,
                departmentIds,
                ouIds,
                workingGroupIds,
                productAreaIds,
                administratorIds,
                caseStatusIds,
                caseTypeId,
                periodFrom,
                periodUntil,
                closeFrom,
                closeTo);

            return query.ToList();
        }

        public Dictionary<DateTime, int> GetCaseAggregation(
           int customerId,
           List<int> departmentIds,
           List<int> ouIds,
           List<int> workingGroupIds,
           List<int> productAreaIds,
           List<int> administratorIds,
           List<int> caseStatusIds,
           List<int> caseTypeId,
           DateTime? periodFrom,
           DateTime? periodUntil,
            DateTime? closeFrom,
            DateTime? closeTo)
        {       

            if (!periodFrom.HasValue)
            {
                periodFrom = DateTime.Now;
            }

            if (!periodUntil.HasValue)
            {
                periodUntil = DateTime.Now;
            }
            var closeToEndOfDay = closeTo.HasValue ? closeTo.Value.GetEndOfDay() : (DateTime?)null;

            var query = from c in this.DataContext.Cases
                join cu in this.DataContext.Customers on c.Customer_Id equals cu.Id

                join isAbout in this.DataContext.CaseIsAbout on c.Id equals (int?) isAbout.Id into isabouts
                from _isAbout in isabouts.DefaultIfEmpty()

                join r in this.DataContext.Regions on c.Region_Id equals (int?) r.Id into rs
                from _r in rs.DefaultIfEmpty()

                join d in this.DataContext.Departments on c.Department_Id equals (int?) d.Id into ds
                from _d in ds.DefaultIfEmpty()

                join u1 in this.DataContext.Users on c.User_Id equals (int?) u1.Id into u1s
                from _u1 in u1s.DefaultIfEmpty()

                join s in this.DataContext.Systems on c.System_Id equals (int?) s.Id into ss
                from _s in ss.DefaultIfEmpty()

                join p in this.DataContext.Priorities on c.Priority_Id equals (int?) p.Id into ps
                from _p in ps.DefaultIfEmpty()

                join im in this.DataContext.Impacts on c.Impact_Id equals (int?) im.Id into ims
                from _im in ims.DefaultIfEmpty()

                join cat in this.DataContext.Categories on c.Category_Id equals (int?) cat.Id into cats
                from _cat in cats.DefaultIfEmpty()

                join sup in this.DataContext.Suppliers on c.Supplier_Id equals (int?) sup.Id into sups
                from _sup in sups.DefaultIfEmpty()

                join regsource in this.DataContext.RegistrationSourceCustomer on c.RegistrationSourceCustomer_Id equals
                    (int?) regsource.Id into regsources
                from _regsource in regsources.DefaultIfEmpty()

                join caseStatis in this.DataContext.CaseStatistics on c.Id equals caseStatis.CaseId into casestatiss
                from _caseStatis in casestatiss.DefaultIfEmpty()

                join wg in this.DataContext.WorkingGroups on c.WorkingGroup_Id equals (int?) wg.Id into wgs
                from _wg in wgs.DefaultIfEmpty()

                join u2 in this.DataContext.Users on c.CaseResponsibleUser_Id equals (int?) u2.Id into u2s
                from _u2 in u2s.DefaultIfEmpty()

                join st in this.DataContext.Statuses on c.Status_Id equals (int?) st.Id into sts
                from _st in sts.DefaultIfEmpty()

                join sst in this.DataContext.StateSecondaries on c.StateSecondary_Id equals (int?) sst.Id into ssts
                from _sst in ssts.DefaultIfEmpty()

                join cp in this.DataContext.CausingParts on c.CausingPartId equals (int?) cp.Id into cps
                from _cp in cps.DefaultIfEmpty()

                join ur in this.DataContext.Urgencies on c.Urgency_Id equals (int?) ur.Id into urs
                from _ur in urs.DefaultIfEmpty()

                join user3 in this.DataContext.Users on c.Performer_User_Id equals (int?) user3.Id into user3s
                from _user3 in user3s.DefaultIfEmpty()
       
                where
                    c.Customer_Id == customerId && c.Deleted != 1 &&
                    (DbFunctions.TruncateTime(c.RegTime) >= DbFunctions.TruncateTime(periodFrom) && DbFunctions.TruncateTime(c.RegTime) <= DbFunctions.TruncateTime(periodUntil))

                    && (closeFrom.HasValue ? DbFunctions.TruncateTime(c.FinishingDate) >= DbFunctions.TruncateTime(closeFrom.Value) : true)
                    && (closeToEndOfDay.HasValue ? DbFunctions.TruncateTime(c.FinishingDate) <= DbFunctions.TruncateTime(closeToEndOfDay.Value) : true)

                    && (caseTypeId.Any() ? caseTypeId.Contains(c.CaseType_Id) : true)
                    &&
                    (workingGroupIds.Any()
                        ? (workingGroupIds.Contains(0) ? !c.WorkingGroup_Id.HasValue ||
                                                         c.WorkingGroup_Id.HasValue && workingGroupIds.Contains(c.WorkingGroup_Id.Value)
                                                       : c.WorkingGroup_Id.HasValue && workingGroupIds.Contains(c.WorkingGroup_Id.Value))
                        : false)                    
                    &&

                     (departmentIds.Any()
                        ? (ouIds.Any() ? (c.Department_Id.HasValue && departmentIds.Contains(c.Department_Id.Value)) ||
                                         c.OU_Id.HasValue && ouIds.Contains(c.OU_Id.Value) :
                                         c.Department_Id.HasValue && departmentIds.Contains(c.Department_Id.Value)
                                         )
                        : (ouIds.Any() ? c.OU_Id.HasValue && ouIds.Contains(c.OU_Id.Value) : true))
                    
                    &&
                    (productAreaIds.Any()
                        ? c.ProductArea_Id.HasValue && productAreaIds.Contains(c.ProductArea_Id.Value)
                        : true)
                    &&
                    (administratorIds.Any()
                        ? c.Performer_User_Id.HasValue && administratorIds.Contains(c.Performer_User_Id.Value)
                        : true)
                 
                    && (caseStatusIds.Any() ? (caseStatusIds.FirstOrDefault() == 1 ? c.FinishingDate.HasValue : !c.FinishingDate.HasValue) : true)


                group c by new {c.RegTime.Month, c.RegTime.Year}
                into g
                select new {Date = g.Key, Count = g.Count()};

                     
            return query.ToDictionary(x => new DateTime(x.Date.Year, x.Date.Month, 1), y => y.Count);
   
        }
        
        #region Private Methods

        private IQueryable<ReportGeneratorFields> GetQuery(int customerId,
           List<int> departmentIds,
           List<int> ouIds,
           List<int> workingGroupIds,
           List<int> productAreaIds,
           List<int> administratorIds,
           List<int> caseStatusIds,
           List<int> caseTypeId,
           DateTime? periodFrom,
           DateTime? periodUntil,
           DateTime? closeFrom,
           DateTime? closeTo)
        {
            if (!periodFrom.HasValue)
            {
                periodFrom = DateTime.Now;
            }

            if (!periodUntil.HasValue)
            {
                periodUntil = DateTime.Now;
            }
            var closeToEndOfDay = closeTo.HasValue ? closeTo.Value.GetEndOfDay() : (DateTime?) null;

            var query = from c in this.DataContext.Cases
                        join cu in this.DataContext.Customers on c.Customer_Id equals cu.Id

                        join isAbout in this.DataContext.CaseIsAbout on c.Id equals (int?)isAbout.Id into isabouts
                        from _isAbout in isabouts.DefaultIfEmpty()

                        join r in this.DataContext.Regions on c.Region_Id equals (int?)r.Id into rs
                        from _r in rs.DefaultIfEmpty()

                        join d in this.DataContext.Departments on c.Department_Id equals (int?)d.Id into ds
                        from _d in ds.DefaultIfEmpty()

                        join u1 in this.DataContext.Users on c.User_Id equals (int?)u1.Id into u1s
                        from _u1 in u1s.DefaultIfEmpty()

                        join s in this.DataContext.Systems on c.System_Id equals (int?)s.Id into ss
                        from _s in ss.DefaultIfEmpty()

                        join p in this.DataContext.Priorities on c.Priority_Id equals (int?)p.Id into ps
                        from _p in ps.DefaultIfEmpty()

                        join im in this.DataContext.Impacts on c.Impact_Id equals (int?)im.Id into ims
                        from _im in ims.DefaultIfEmpty()

                        join cat in this.DataContext.Categories on c.Category_Id equals (int?)cat.Id into cats
                        from _cat in cats.DefaultIfEmpty()

                        join sup in this.DataContext.Suppliers on c.Supplier_Id equals (int?)sup.Id into sups
                        from _sup in sups.DefaultIfEmpty()

                        join regsource in this.DataContext.RegistrationSourceCustomer on c.RegistrationSourceCustomer_Id equals
                            (int?)regsource.Id into regsources
                        from _regsource in regsources.DefaultIfEmpty()

                        join caseStatis in this.DataContext.CaseStatistics on c.Id equals caseStatis.CaseId into casestatiss
                        from _caseStatis in casestatiss.DefaultIfEmpty()

                        join wg in this.DataContext.WorkingGroups on c.WorkingGroup_Id equals (int?)wg.Id into wgs
                        from _wg in wgs.DefaultIfEmpty()

                        join u2 in this.DataContext.Users on c.CaseResponsibleUser_Id equals (int?)u2.Id into u2s
                        from _u2 in u2s.DefaultIfEmpty()

                        join st in this.DataContext.Statuses on c.Status_Id equals (int?)st.Id into sts
                        from _st in sts.DefaultIfEmpty()

                        join sst in this.DataContext.StateSecondaries on c.StateSecondary_Id equals (int?)sst.Id into ssts
                        from _sst in ssts.DefaultIfEmpty()

                        join cp in this.DataContext.CausingParts on c.CausingPartId equals (int?)cp.Id into cps
                        from _cp in cps.DefaultIfEmpty()

                        join ur in this.DataContext.Urgencies on c.Urgency_Id equals (int?)ur.Id into urs
                        from _ur in urs.DefaultIfEmpty()

                        join user3 in this.DataContext.Users on c.Performer_User_Id equals (int?)user3.Id into user3s
                        from _user3 in user3s.DefaultIfEmpty()

                        where
                            c.Customer_Id == customerId && c.Deleted != 1 &&
                            (DbFunctions.TruncateTime(c.RegTime) >= DbFunctions.TruncateTime(periodFrom) && DbFunctions.TruncateTime(c.RegTime) <= DbFunctions.TruncateTime(periodUntil))

                            && (closeFrom.HasValue ? DbFunctions.TruncateTime(c.FinishingDate) >= DbFunctions.TruncateTime(closeFrom.Value) : true)
                    && (closeToEndOfDay.HasValue ? DbFunctions.TruncateTime(c.FinishingDate) <= DbFunctions.TruncateTime(closeToEndOfDay.Value) : true)

                            && (caseTypeId.Any() ? caseTypeId.Contains(c.CaseType_Id) : true)
                            &&
                            (workingGroupIds.Any()
                                ? (workingGroupIds.Contains(0) ? !c.WorkingGroup_Id.HasValue ||
                                                                 c.WorkingGroup_Id.HasValue && workingGroupIds.Contains(c.WorkingGroup_Id.Value)
                                                               : c.WorkingGroup_Id.HasValue && workingGroupIds.Contains(c.WorkingGroup_Id.Value))
                                : false)
                            &&

                            (departmentIds.Any()
                                ? (ouIds.Any() ? (c.Department_Id.HasValue && departmentIds.Contains(c.Department_Id.Value)) ||
                                                 c.OU_Id.HasValue && ouIds.Contains(c.OU_Id.Value) :
                                                 c.Department_Id.HasValue && departmentIds.Contains(c.Department_Id.Value)
                                                 )
                                : (ouIds.Any() ? c.OU_Id.HasValue && ouIds.Contains(c.OU_Id.Value) : true))
                            &&
                            (productAreaIds.Any()
                                ? c.ProductArea_Id.HasValue && productAreaIds.Contains(c.ProductArea_Id.Value)
                                : true)
                            &&
                            (administratorIds.Any()
                                ? c.Performer_User_Id.HasValue && administratorIds.Contains(c.Performer_User_Id.Value)
                                : true)

                        orderby c.Id

                        select new ReportGeneratorFields
                        {
                            Id = c.Id,
                            User = c.ReportedBy,
                            Notifier = c.PersonsName,
                            Email = c.PersonsEmail,
                            Phone = c.PersonsPhone,
                            CellPhone = c.PersonsCellphone,
                            Customer = cu.Name,
                            Region = c.Region_Id.HasValue ? _r.Name : "",
                            Department = c.Department_Id.HasValue ? _d.DepartmentName : "",
                            Unit = c.OU_Id.HasValue ? c.OU_Id.ToString() : "",
                            Place = c.Place,
                            OrdererCode = c.UserCode,

                            IsAbout_User = _isAbout.ReportedBy,
                            IsAbout_Persons_Name = _isAbout.Person_Name,
                            IsAbout_Persons_Phone = _isAbout.Person_Phone,
                            IsAbout_Persons_CellPhone = _isAbout.Person_Cellphone,
                            IsAbout_Region = "", //_isAbout.Region_Id.HasValue ? _r1.Name : "",   //Disabled for version 5.3.21
                            IsAbout_Department = "", //_isAbout.Department_Id.HasValue ? _d1.DepartmentName : "",
                            IsAbout_OU = "", //_isAbout.OU_Id.HasValue ? _isAbout.OU_Id.ToString() : "",
                            IsAbout_CostCentre = _isAbout.CostCentre,
                            IsAbout_Place = _isAbout.Place,
                            IsAbout_UserCode = _isAbout.UserCode,
                            IsAbout_Persons_Email = _isAbout.Person_Email,

                            PcNumber = c.InventoryNumber,
                            ComputerType = c.InventoryType,
                            ComputerPlace = c.InventoryLocation,

                            Case = c.CaseNumber,
                            RegistrationDate = c.RegTime,
                            ChangeDate = c.ChangeTime,
                            RegistratedBy = c.User_Id.HasValue ? _u1.FirstName + " " + _u1.SurName : "",
                            CaseType = c.CaseType_Id,
                            ProductArea = c.ProductArea_Id.HasValue ? c.ProductArea_Id.ToString() : "",
                            System = c.System_Id.HasValue ? _s.SystemName : "",
                            UrgentDegree = c.Urgency_Id.HasValue ? _ur.Name : "",
                            Impact = c.Impact_Id.HasValue ? _im.Name : "",
                            Category = c.Category_Id.HasValue ? _cat.Name : "",
                            Supplier = c.Supplier_Id.HasValue ? _sup.Name : "",
                            InvoiceNumber = c.InvoiceNumber,
                            ReferenceNumber = c.ReferenceNumber,
                            Caption = c.Caption,
                            Description = c.Description,
                            Other = c.Miscellaneous,
                            PhoneContact = c.ContactBeforeAction,
                            Sms = c.SMS,
                            AgreedDate = c.AgreedDate,
                            Available = c.Available,
                            Cost = c.Cost,
                            AttachedFile = "",
                            FinishingDate = c.FinishingDate,
                            FinishingDescription = c.FinishingDescription,

                            RegistrationSource = c.RegistrationSourceCustomer_Id.HasValue ? _regsource.SourceName : "",
                            SolvedInTime = _caseStatis.WasSolvedInTime,

                            WorkingGroup = c.WorkingGroup_Id.HasValue ? _wg.WorkingGroupName : string.Empty,
                            Responsible = c.CaseResponsibleUser_Id.HasValue ? _u2.FirstName + " " + _u2.SurName : "",
                            Administrator = c.Performer_User_Id.HasValue ? _user3.FirstName + " " + _user3.SurName : "",
                            Priority = c.Priority_Id.HasValue ? _p.Name : "",
                            State = c.Status_Id.HasValue ? _st.Name : "",
                            SubState = c.StateSecondary_Id.HasValue ? _sst.Name : "",
                            PlannedActionDate = c.PlanDate,
                            WatchDate = c.WatchDate,
                            Verified = c.Verified,
                            VerifiedDescription = c.VerifiedDescription,
                            SolutionRate = c.SolutionRate,
                            CausingPart = c.CausingPartId.HasValue ? _cp.Name : "",

                            LogData = (from r in this.DataContext.Logs
                                       where (r.Case_Id == c.Id)
                                       orderby r.LogDate descending
                                       select r).FirstOrDefault(),

                            AllInternalText = (from r2 in this.DataContext.Logs
                                               where (r2.Case_Id == c.Id && r2.Text_Internal != null && r2.Text_Internal != "")
                                               orderby r2.LogDate
                                               select r2.Text_Internal).ToList(),

                            AllExtenalText = (from r2 in this.DataContext.Logs
                                              where (r2.Case_Id == c.Id && r2.Text_External != null && r2.Text_External != "")
                                              orderby r2.LogDate
                                              select r2.Text_External).ToList()
                        };
            
            if (caseStatusIds.Any())
            {
                if (caseStatusIds.Where(x => x == 1).Any())
                    return query.Where(x => x.FinishingDate.HasValue);
                else if (caseStatusIds.Where(x => x == 2).Any())
                    return query.Where(x => !x.FinishingDate.HasValue);                    
            }
            
            return query;
        }

        #endregion Private Methods
    }

    #endregion

    #region REPORTCUSTOMER

    public interface IReportCustomerRepository : IRepository<ReportCustomer>
    {
        IEnumerable<CustomerReportList> GetCustomerReportListForCustomer(int id);
    }

    public class ReportCustomerRepository : RepositoryBase<ReportCustomer>, IReportCustomerRepository
    {
        private readonly ITranslator translator;

        public ReportCustomerRepository(IDatabaseFactory databaseFactory, ITranslator translator)
            : base(databaseFactory)
        {
            this.translator = translator;
        }

        public IEnumerable<CustomerReportList> GetCustomerReportListForCustomer(int id)
        {
            var query = from rc in this.DataContext.ReportCustomers
                        join c in this.DataContext.Customers on rc.Customer_Id equals c.Id
                        where c.Id == id
                        group rc by new { c.Id, rc.Report_Id, rc.ShowOnPage } into g
                        select new CustomerReportList
                        {
                            CustomerId = g.Key.Id,
                            ReportId = g.Key.Report_Id,
                            ActiveOnPage = g.Key.ShowOnPage
                        };

            return query;
        }

        
    }

    #endregion

    #region REPORTFAVORITE

    public interface IReportFavoriteRepository : IRepository<ReportFavorite>
    {
        IEnumerable<ReportFavoriteList> GetCustomerReportFavoriteList(int customerId, int? userId);
        ReportFavorite GetCustomerReportFavoriteById(int reportFavoriteId, int customerId, int? userId);
        bool IsCustomerReportFavoriteNameUnique(int reportFavoriteId, int customerId, int? userId, string name);
        void DeleteCustomerReportFavoriteById(int reportFavoriteId, int customerId, int? userId);
    }

    public class ReportFavoriteRepository : RepositoryBase<ReportFavorite>, IReportFavoriteRepository
    {
        private readonly ITranslator translator;

        public ReportFavoriteRepository(IDatabaseFactory databaseFactory, ITranslator translator)
            : base(databaseFactory)
        {
            this.translator = translator;
        }

        public IEnumerable<ReportFavoriteList> GetCustomerReportFavoriteList(int customerId, int? userId)
        {
	        var query = DataContext.ReportFavorites.Where(x => x.Customer_Id == customerId);
			if (userId.HasValue)
			{
				query = query.Where(x => x.User_Id == userId.Value);
			}

            return query.Select(x => new ReportFavoriteList
			{
				Id = x.Id,
				Type = x.Type,
				Name = x.Name,
			}).ToList();
        }

        public ReportFavorite GetCustomerReportFavoriteById(int reportFavoriteId, int customerId, int? userId)
        {
            var query =  DataContext.ReportFavorites.Where(x => x.Id == reportFavoriteId && x.Customer_Id == customerId);
	        if (userId.HasValue)
	        {
		        query = query.Where(x => x.User_Id == userId.Value);
	        }

	        return query.FirstOrDefault();
        }

        public bool IsCustomerReportFavoriteNameUnique(int reportFavoriteId, int customerId, int? userId, string name)
        {
			var query = DataContext.ReportFavorites.Where(x => x.Id != reportFavoriteId && x.Customer_Id == customerId && x.Name.ToLower() == name.ToLower());
			if (userId.HasValue)
			{
				query = query.Where(x => x.User_Id == userId.Value);
			}
			return !query.Any();
        }

        public void DeleteCustomerReportFavoriteById(int reportFavoriteId, int customerId, int? userId)
        {
            var query = DataContext.ReportFavorites.Where(x => x.Id == reportFavoriteId && x.Customer_Id == customerId);
			if (userId.HasValue)
			{
				query = query.Where(x => x.User_Id == userId.Value);
			}

	        var report = query.FirstOrDefault();
			if (report != null)
            {
                Delete(report);
            }
        }
    }

    #endregion
}
