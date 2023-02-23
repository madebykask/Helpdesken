using System.Data.Entity;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Models.Case.ChidCase;
using DH.Helpdesk.BusinessData.Models.Case.MergedCase;
using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.Dal.Repositories
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using DH.Helpdesk.BusinessData.Models.Case;
	using DH.Helpdesk.BusinessData.Models.Case.Output;
	using DH.Helpdesk.BusinessData.Models.User.Input;
	using DH.Helpdesk.Dal.Infrastructure;
	using DH.Helpdesk.Dal.Infrastructure.Context;
	using DH.Helpdesk.Dal.Mappers.Cases.EntityToBusinessModel;
	using DH.Helpdesk.Domain;
	using Mappers;
	using System.Linq.Expressions;
	using Z.EntityFramework.Plus;
	using BusinessData.OldComponents;
	using Common.Linq;
	using System.Text.RegularExpressions;

	public interface ICaseRepository : IRepository<Case>
    {
        IQueryable<Case> GetCustomerCases(int customerId);
        IQueryable<Case> GetDetachedCaseQuery(int id, bool includeLogs = false);
        CaseOverview GetCaseBasic(int id);
        Case GetCaseById(int id, bool markCaseAsRead = false);
        Task<Case> GetCaseByIdAsync(int id, bool markCaseAsRead = false);
        Case GetCaseByGUID(Guid GUID);
        int GetCaseIdByEmailGUID(Guid GUID);
        Case GetCaseByEmailGUID(Guid GUID);
        Case GetDetachedCaseById(int id);
        Case GetDetachedCaseIncludesById(int id);
        DynamicCase GetDynamicCase(int id);
        IList<Case> GetProjectCases(int customerId, int projectId);
        IList<Case> GetProblemCases(int customerId, int problemId);
        void SetNullProblemByProblemId(int problemId);
        void UpdateFinishedDate(int problemId, DateTime? time);
        void UpdateFollowUpDate(int caseId, DateTime? time);
        void Activate(int caseId, int calculatedLeadTime, int externalTimeToAdd = 0);
        void MarkCaseAsUnread(int id);
        void MarkCaseAsRead(int id);
        IEnumerable<CaseRelation> GetRelatedCases(int id, int customerId, string reportedBy, UserOverview user);
        IEnumerable<CaseOverview> GetCaseOverviews(int[] customers);
        int LookupLanguage(int custid, string notid, int regid, int depid, string notifierid);
        List<DynamicCase> GetAllDynamicCases(int customerId, int[] caseIds);
        bool IsCaseExists(int id);
        Case GetCaseIncluding(int id);

        CaseModel GetCase(int id);

        /// <summary>
        /// The get case overview.
        /// </summary>
        /// <param name="caseId">
        /// The case id.
        /// </param>
        /// <returns>
        /// The <see cref="CaseOverview"/>.
        /// </returns>
        CaseOverview GetCaseOverview(int caseId);

        MyCase[] GetMyCases(int userId, int? count = null);

		IQueryable<CustomerUserCase> GetActiveCustomerUserCases(bool myCases, int currentUserId, int? customerId, string freeTextSearch, int from, int count, string orderby = null, bool orderbyAscending = true, bool searchInternaLog = false);

		IList<Case> GetProblemCases(int problemId);
        IList<int> GetCasesIdsByType(int caseTypeId);
        StateSecondary GetCaseSubStatus(int caseId);
        List<ChildCaseOverview> GetChildCasesFor(int parentCaseId);
        ParentCaseInfo GetParentInfo(int childCaseId);
        List<MergedChildOverview> GetMergedCasesFor(int parentCaseId);
        MergedParentInfo GetMergedParentInfo(int childCaseId);
        List<Case> GetTop100CasesToTest();
        Case GetCaseQuickOpen(UserOverview user, Expression<Func<Case, bool>> casePermissionFilter, string searchFor);
        int GetCaseCustomerId(int caseId);
        List<Case> GetCasesByCaseIds(int[] caseIds);
        IList<Case> GetCustomerCasesForWebpart(int customerId);
    }

    public class CaseRepository : RepositoryBase<Case>, ICaseRepository
    {
        private readonly IWorkContext workContext;
        private readonly IEntityToBusinessModelMapper<Case, CaseModel> _caseToBusinessModelMapper;
        private readonly IBusinessModelToEntityMapper<CaseModel, Case> _caseModelToEntityMapper;

        public CaseRepository(
            IDatabaseFactory databaseFactory,
            IWorkContext workContext, 
            IEntityToBusinessModelMapper<Case, CaseModel> caseToBusinessModelMapper,
            IBusinessModelToEntityMapper<CaseModel, Case> caseModelToEntityMapper)
            : base(databaseFactory)
        {
            this.workContext = workContext;
            _caseModelToEntityMapper = caseModelToEntityMapper;
            _caseToBusinessModelMapper = caseToBusinessModelMapper;
        }

        public IQueryable<Case> GetCustomerCases(int customerId)
        {
            return Table.Where(x => x.Customer_Id == customerId);
        }
        
        public CaseOverview GetCaseBasic(int id)
        {
            var caseInfo = DataContext.Cases.Where(c => c.Id == id).Select(c => new CaseOverview()
            {
                Id = c.Id,
                CustomerId = c.Customer_Id, 
                CaseNumber = c.CaseNumber,
            }).FirstOrDefault();
            return caseInfo;
        }

        public bool IsCaseExists(int id)
        {
            return DataContext.Cases.Any(c => c.Id == id);
        }

        public Task<Case> GetCaseByIdAsync(int id, bool markCaseAsRead = false)
        {
            if (markCaseAsRead)
                MarkCaseAsRead(id);

            return GetByIdAsync(id);
        }

        public Case GetCaseById(int id, bool markCaseAsRead = false)
        {
            if (markCaseAsRead)
                MarkCaseAsRead(id);

            return (from w in this.DataContext.Cases
                    where w.Id == id
                    select w).FirstOrDefault();
        }

        public List<DynamicCase> GetAllDynamicCases(int customerId, int[] caseIds)
        {
            var query = from f in this.DataContext.Forms
                        join ff in this.DataContext.FormField on f.Id equals ff.Form_Id
                        join ffv in this.DataContext.FormFieldValue on ff.Id equals ffv.FormField_Id
                        where f.Customer_Id == customerId && f.ExternalPage == 1 && caseIds.Contains(ffv.Case_Id)
                        select new DynamicCase
                        {
                            CaseId = ffv.Case_Id,
                            FormPath = f.FormPath
                        };

            return query.Distinct().ToList();
        }

        public List<Case> GetCasesByCaseIds(int[] caseIds)
        {
            return (from f in this.DataContext.Cases
                         where caseIds.Contains(f.Id)
                         select f).ToList();
        }

        public IList<Case> GetProjectCases(int customerId, int projectId)
        {
            var cases = this.DataContext.Cases.Where(c => c.Customer_Id == customerId && c.Project_Id == projectId).ToList();
            return cases;
        }

        public IList<Case> GetCustomerCasesForWebpart(int customerId)
        {
            var cases = this.DataContext.Cases.Where(c => c.Customer_Id == customerId).ToList();
            return cases;
        }

        public IList<Case> GetProblemCases(int customerId, int problemId)
        {
            var cases = this.DataContext.Cases.Where(c => c.Customer_Id == customerId && c.Problem_Id == problemId).ToList();
            return cases;
        }

        public StateSecondary GetCaseSubStatus(int caseId)
        {
            var stateSecondary = 
                DataContext.Cases.Where(x => x.Id == caseId).Select(x => x.StateSecondary).FirstOrDefault();

            return stateSecondary;
        }

        public DynamicCase GetDynamicCase(int id)
        {
            var query = from f in this.DataContext.Forms
                        join ffu in this.DataContext.FormUrls on f.FormUrl_Id equals ffu.Id
                        join ff in this.DataContext.FormField on f.Id equals ff.Form_Id
                        join ffv in this.DataContext.FormFieldValue on ff.Id equals ffv.FormField_Id
                        where ffv.Case_Id == id
                        select new DynamicCase
                        {
                            CaseId = ffv.Case_Id,
                            FormPath = f.FormPath,
                            FormName = f.FormName,
                            ViewMode = f.ViewMode,
                            ExternalPage = f.ExternalPage == 1 ? true : false,
                            Scheme = ffu.Scheme,
                            Host = ffu.Host,
                            ExternalSite = ffu.ExternalSite
                        };

            return query.FirstOrDefault();
        }

        public Case GetCaseByGUID(Guid GUID)
        {
            var caseEntity = DataContext.Cases.Where(c => c.CaseGUID == GUID)
                                              .FirstOrDefault();

            return caseEntity;
        }

        public int GetCaseIdByEmailGUID(Guid GUID)
        {
            var caseId = DataContext.EmailLogs.Where(e => e.EmailLogGUID == GUID && e.CaseHistory_Id > 0)
                                              .Select(e => e.CaseHistory.Case_Id)
                                              .FirstOrDefault();
            return caseId;
        }

        public Case GetCaseByEmailGUID(Guid GUID)// TODO: optimise to 1 query
        {
            Case ret = null;
            var caseHistoryId = DataContext.EmailLogs.Where(e => e.EmailLogGUID == GUID && e.CaseHistory_Id != null)
                                                     .Select(e => e.CaseHistory_Id)
                                                     .FirstOrDefault();
            if (caseHistoryId != null)
            {
                var caseId = DataContext.CaseHistories.Where(h => h.Id == caseHistoryId).Select(h => h.Case_Id).FirstOrDefault();
                ret = DataContext.Cases.Where(c => c.Id == caseId).FirstOrDefault();
            }

            return ret;
        }

        public Case GetDetachedCaseById(int id)
        {
            return GetDetachedCaseQuery(id).FirstOrDefault();
        }

        public IQueryable<Case> GetDetachedCaseQuery(int id, bool includeLogs = false)
        {
            var query = DataContext.Cases.Where(w => w.Id == id);
                //.Include(c => c.CaseType)
                //.Include(c => c.Priority)
                //.Include(c => c.Workinggroup)
                //.Include(c => c.ProductArea)
                //.Include(c => c.StateSecondary);
            if (includeLogs)
                query = query.Include(c => c.Logs);
            return query.AsNoTracking();
        }

        public Case GetDetachedCaseIncludesById(int id)
        {
            var result = DataContext.Cases.AsNoTracking()
            .Include(x => x.CaseFollowers)
            .Include(x => x.IsAbout)
            .FirstOrDefault(x => x.Id == id);
            return result;
        }

        public void SetNullProblemByProblemId(int problemId)
        {
            var cases =
                this.DataContext.Cases.Where(x => x.Problem_Id == problemId).ToList();

            foreach (var item in cases)
            {
                item.Problem_Id = null;
            }
        }

        public void Activate(int caseId, int calculatedLeadTime, int externalTimeToAdd = 0)
        {
            var cases = this.DataContext.Cases.Where(x => x.Id == caseId).FirstOrDefault();
            if (cases != null)
            {
                cases.FinishingDate = null;
                cases.ApprovedBy_User_Id = null;
                cases.ApprovedDate = null;
                cases.LeadTime = 0;
                cases.ChangeTime = DateTime.UtcNow;
                cases.LeadTime = calculatedLeadTime;
                cases.ExternalTime += externalTimeToAdd;

                foreach (var log in cases.Logs)
                {
                    log.FinishingType = null;
                }

                this.Update(cases);
                this.Commit();
            }
        }

        public void UpdateFinishedDate(int problemId, DateTime? time)
        {
            var cases = this.DataContext.Cases.Where(x => x.Problem_Id == problemId).ToList();

            foreach (var item in cases)
            {
                item.FinishingDate = time;
            }
        }

        public void UpdateFollowUpDate(int caseId, DateTime? time)
        {
            var cases = this.DataContext.Cases.Where(x => x.Id == caseId).FirstOrDefault();
            if (cases != null)
            {
                cases.FollowUpDate = time;
                this.Update(cases);
                this.Commit();
            }
        }

        public void SetNullProjectByProjectId(int projectId)
        {
            var cases = this.DataContext.Cases.Where(x => x.Project_Id == projectId).ToList();

            foreach (var item in cases)
            {
                item.Project_Id = null;
            }
        }

        public void MarkCaseAsUnread(int id)
        {
            SetCaseUnreadFlag(id, true);
        }

        public IEnumerable<CaseRelation> GetRelatedCases(int id, int customerId, string reportedBy, UserOverview user)
        {
            var query = DataContext.Cases.Where(c => c.Customer_Id == customerId
                                                     && c.Id != id
                                                     && c.ReportedBy.ToLower() == reportedBy.ToLower());

            var restrictToOwnCasesOnly = 
                DataContext.CustomerUsers.Single(x => x.Customer_Id == customerId && x.User_Id == user.Id).RestrictedCasePermission;
            
            //handläggare
            if (restrictToOwnCasesOnly && user.UserGroupId == UserGroups.Administrator)
                query = query.Where(c => c.Performer_User_Id == user.Id || c.CaseResponsibleUser_Id == user.Id);

            //anmälare
            if (restrictToOwnCasesOnly && user.UserGroupId == UserGroups.User)
                query = query.Where(c => c.ReportedBy.ToLower() == user.UserId.ToLower());

            return query.Select(c => new CaseRelation()
            {
                Id = c.Id,
                Caption = c.Caption,
                Description = c.Description,
                CaseNumber = c.CaseNumber,
                FinishingDate = c.FinishingDate,
                Regtime = c.RegTime
            });
        }

        public IEnumerable<CaseOverview> GetCaseOverviews(int[] customers)
        {
            return DataContext.Cases
                .Where(c => customers.Contains(c.Customer_Id))
                .Select(c => new CaseOverview()
                {
                    Id = c.Id,
                    CustomerId = c.Customer_Id,
                    Deleted = c.Deleted,
                    FinishingDate = c.FinishingDate,
                    StatusId = c.Status_Id,
                    UserId = c.User_Id
                });
        }
        
        /// <summary>
        /// The get case overview.
        /// </summary>
        /// <param name="caseId">
        /// The case id.
        /// </param>
        /// <returns>
        /// The <see cref="CaseOverview"/>.
        /// </returns>
        public CaseOverview GetCaseOverview(int caseId)
        {
            var query = (from _case in DataContext.Cases.AsNoTracking()
                         //from caseHistory in _case.CaseHistories.DefaultIfEmpty() //will load CaseHistories with left join
                         where _case.Id == caseId
                         select _case)
                        .Include(c => c.CaseHistories)
                        .Include(c => c.CaseType)
                        .Include(c => c.Department)
                        .Include(c => c.Region)
                        .Include(c => c.Priority)
                        .Include(c => c.Workinggroup)
                        .Include(c => c.ProductArea)
                        .Include(c => c.StateSecondary);

            return query.Select(CaseToCaseOverviewMapper.Map).FirstOrDefault();
        }

        public MyCase[] GetMyCases(int userId, int? count = null)
        {
            var entities = from cs in this.DataContext.Cases
                           join cus in this.DataContext.Customers on cs.Customer_Id equals cus.Id
                           where (cs.Performer_User_Id == userId && cs.FinishingDate == null && cs.Deleted == 0)
                           orderby (cs.ChangeTime) descending
                           select new
                           {
                               Id = cs.Id,
                               CaseNumber = cs.CaseNumber,
                               RegistrationDate = cs.RegTime,
                               ChangedDate = cs.ChangeTime,
                               Subject = cs.Caption,
                               InitiatorName = cs.PersonsName,
                               Description = cs.Description,
                               CustomerName = cus.Name
                           };

            if (count.HasValue)
            {
                entities = entities.Take(count.Value);
            }

            return entities
                .ToArray()
                .Select(c => new MyCase(
                                c.Id,
                                c.CaseNumber,
                                c.RegistrationDate,
                                c.ChangedDate,
                                c.Subject,
                                c.InitiatorName,
                                c.Description,
                                c.CustomerName))
                                .ToArray();
        }


		public IQueryable<CustomerUserCase> GetActiveCustomerUserCases(bool myCases, int currentUserId, int? customerId, string freeTextSearch, int from, int count, string orderby = null, bool orderbyAscending = true, bool searchInternalLog = false)
		{
			var workingGroupsIds = (from wgsu in this.DataContext.UserWorkingGroups
									where wgsu.User_Id == currentUserId
									select wgsu.WorkingGroup_Id)
									.ToList();
			var departments = (from du in this.DataContext.DepartmentUsers
							   where du.User_Id == currentUserId
							   join d in this.DataContext.Departments on du.Department_Id equals d.Id
							   join cu in this.DataContext.CustomerUsers on new { d.Customer_Id, du.User_Id } equals new { cu.Customer_Id, cu.User_Id }
							   select d)
							  .ToList();
			var departmentCustomerIds = departments.Select(p => p.Customer_Id).Distinct().ToList();
			var departmentIds = departments.Select(o => o.Id).ToList();

			var user = this.DataContext.Users.Single(o => o.Id == currentUserId);
			var isUserOrNormalAdmin = new int[] { UserGroups.Administrator, UserGroups.User }.Contains(user.UserGroup_Id);
			var showNotAssignedWorkingGroups = !isUserOrNormalAdmin ? true : user.ShowNotAssignedWorkingGroups == 1;
			var showNotAssignedCases = !isUserOrNormalAdmin ? true : user.ShowNotAssignedCases == 1;
		

			var fullAccessCustomers = this.DataContext.CustomerUsers
				.Where(o => o.User_Id == currentUserId && !departmentCustomerIds.Contains(o.Customer_Id))
				.Select(o => o.Customer_Id)
				.Distinct()
				.ToList();

			var searchFreeText = !string.IsNullOrWhiteSpace(freeTextSearch);

			int? caseId = null;
			bool useCaseNumberSearch = false;
			string[] words = new string[0];
			if (searchFreeText)
			{
				var regexCaseNumber = new Regex(@"^\#(?<nr>\d{1,10})$");

				var isCaseNumber = regexCaseNumber.Match(freeTextSearch.Trim());
				if (isCaseNumber.Success)
				{
					var caseNumber = int.Parse(isCaseNumber.Groups["nr"].Value);
					useCaseNumberSearch = true;

					caseId = this.DataContext.Cases.Where(o => o.CaseNumber == (decimal)caseNumber)
						.Select(o => o.Id)
						.SingleOrDefault();

					searchFreeText = false;
				}
				else
				{

					var wordList = new List<string>();

					var quotes = new Regex("\\\"(?<quote>.*?)\\\""); // Check quoted statements i.e. "my car" volvo saab
					var matches = quotes.Matches(freeTextSearch);
					foreach (Match match in matches)
					{
						var fullQuote = match.Captures[0].Value;                // Full quote i.e. "my car"
						var quote = match.Groups["quote"].Value;                // Content of quote my car

						wordList.Add(quote);                                    // Content of quote to search list
						freeTextSearch = freeTextSearch.Replace(fullQuote, ""); // Remove quoted query
					}

					wordList.AddRange(freeTextSearch.Split(' ').Where(o => !string.IsNullOrWhiteSpace(o)).ToArray());               // Split remaining unquoted words by space

					words = wordList.ToArray();
				}
			}


			var entities = from cs in this.DataContext.Cases
						   join cus in this.DataContext.Customers on cs.Customer_Id equals cus.Id
						   join cusu in this.DataContext.CustomerUsers on new { User_Id = currentUserId, Customer_Id = cus.Id } equals new { cusu.User_Id, cusu.Customer_Id }
						   join u in this.DataContext.Users on cusu.User_Id equals u.Id
						   where (!myCases || cs.Performer_User_Id == currentUserId) &&                                             // My cases logic
						   (cs.FinishingDate == null && cs.Deleted == 0) &&															// Active logic
						   (showNotAssignedCases || cs.Performer_User_Id.HasValue) &&                                               // Show not assigned cases
						   (showNotAssignedWorkingGroups || cs.WorkingGroup_Id.HasValue) &&                                         // Show not assigned cases
						   (!cusu.RestrictedCasePermission ||                                                                       // Restrict to own cases (only for normal admin and user)
								(user.UserGroup_Id == UserGroups.SystemAdministrator) ||
								(user.UserGroup_Id == UserGroups.CustomerAdministrator) ||
								(user.UserGroup_Id == UserGroups.Administrator && (cs.Performer_User_Id == user.Id || cs.CaseResponsibleUser_Id == user.Id)) ||
								(user.UserGroup_Id == UserGroups.User && (cs.ReportedBy.ToLower() == user.UserID.ToLower() || cs.User_Id == user.Id))
							) &&
							(!cs.WorkingGroup_Id.HasValue || workingGroupsIds.Contains(cs.WorkingGroup_Id.Value)) &&                // Working group logic
							(fullAccessCustomers.Contains(cs.Customer_Id) || departmentIds.Contains(cs.Department_Id.Value)) && // Department logic
							(!useCaseNumberSearch || caseId.HasValue && cs.Id == caseId.Value)	&&															// Case ID logixc
							  (!searchFreeText || (                                                                                 // Freetext search
								
									(words.Any(w => cs.ReportedBy.Contains(w) ||
									cs.PersonsName.Contains(w) ||
									cs.RegUserName.Contains(w) ||
									cs.PersonsEmail.Contains(w) ||
									cs.PersonsPhone.Contains(w) ||
									cs.PersonsCellphone.Contains(w) ||
									cs.Place.Contains(w) ||
									cs.Caption.Contains(w) ||
									cs.CaseNumber.ToString().Contains(w) ||
									cs.Description.Contains(w) ||
									cs.Miscellaneous.Contains(w) ||
									cs.ReferenceNumber.Contains(w) ||
									cs.InvoiceNumber.Contains(w) ||
									cs.InventoryNumber.Contains(w) ||
									( // Department
										cs.Department != null &&
										(
											cs.Department.DepartmentName.Contains(w) ||
											cs.Department.DepartmentId.Contains(w)
										)
									) ||
									(   // IsAbout
										cs.IsAbout != null &&
										(
											cs.IsAbout.ReportedBy.Contains(w) ||
											cs.IsAbout.Person_Name.Contains(w) ||
											cs.IsAbout.UserCode.Contains(w) ||
											cs.IsAbout.Person_Email.Contains(w) ||
											cs.IsAbout.Place.Contains(w) ||
											cs.IsAbout.Person_Cellphone.Contains(w) ||
											cs.IsAbout.Person_Phone.Contains(w)
										)
									) ||
									(   // Log 
										cs.Logs.Any(l => l.Text_External.Contains(w) || (searchInternalLog ? l.Text_Internal.Contains(w) : false))
									))

						   )))
						   select new
						   {
							   Id = cs.Id,
							   CaseNumber = cs.CaseNumber,
							   RegistrationDate = cs.RegTime,
							   ChangedDate = cs.ChangeTime,
							   Subject = cs.Caption,
							   InitiatorName = cs.PersonsName,
							   Description = cs.Description,
							   CustomerName = cus.Name,
							   CustomerId = cus.Id,
							   WorkingGroupName = cs.Workinggroup.WorkingGroupName,
							   WorkingGroupId = cs.WorkingGroup_Id,
							   PriorityName = cs.Priority.Name,
							   PriorityId = cs.Priority.Id,
							   PerformerName = cs.Performer_User_Id.HasValue ? cs.Administrator.SurName + " " + cs.Administrator.FirstName : null,
							   PerformerId = cs.Performer_User_Id,
							   StateSecondaryId = cs.StateSecondary_Id,
							   StateSecondary = cs.StateSecondary,
							   WatchDate = cs.WatchDate,
							   CaseIcon = cs.FinishingDate != null ? cs.CaseType.RequireApproving == 1 && cs.ApprovedDate == null ? GlobalEnums.CaseIcon.FinishedNotApproved : GlobalEnums.CaseIcon.Finished : GlobalEnums.CaseIcon.Normal,
							   Unread = cs.Unread == 1 ? true : false,
							   IncludeInCaseStatistics = cs.StateSecondary != null ? cs.StateSecondary.IncludeInCaseStatistics == 1 : false,
							   DepartmentID = cs.Department_Id,
							   RegTime = cs.RegTime,
							   SolutionTime = cs.Priority != null ? cs.Priority.SolutionTime : (int?)null,
							   ExternalTime = cs.ExternalTime,
                               DepartmentName = cs.Department != null ? cs.Department.DepartmentName : null,
						   }; 

            // filter on customer
            if (customerId.HasValue)
            {
                entities = entities.Where(o => o.CustomerId == customerId.Value);
            }

			if (!string.IsNullOrWhiteSpace(orderby))
			{
				switch(orderby)
				{
					case "Performer_User_Id":
						entities = orderbyAscending ? entities.OrderBy(o => o.PerformerId) : entities.OrderByDescending(o => o.PerformerId);
						break;
					case "CaseNumber":
						entities = orderbyAscending ? entities.OrderBy(o => o.CaseNumber) : entities.OrderByDescending(o => o.CaseNumber);
						break;
					case "ChangeTime":
						entities = orderbyAscending ? entities.OrderBy(o => o.ChangedDate) : entities.OrderByDescending(o => o.ChangedDate);
						break;
					case "Priority_Id":
						entities = orderbyAscending ? entities.OrderBy(o => o.PriorityId) : entities.OrderByDescending(o => o.PriorityId);
						break;
					case "StateSecondary_Id":
						entities = orderbyAscending ? entities.OrderBy(o => o.StateSecondaryId) : entities.OrderByDescending(o => o.StateSecondaryId);
						break;
					case "_temporary_LeadTime":
						entities = orderbyAscending ? entities.OrderBy(o => 0) : entities.OrderByDescending(o => 0);
						break;
					case "WatchDate":
						entities = orderbyAscending ? entities.OrderBy(o => o.WatchDate) : entities.OrderByDescending(o => o.WatchDate);
						break;
					case "WorkingGroup_Id":
						entities = orderbyAscending ? entities.OrderBy(o => o.WorkingGroupId) : entities.OrderByDescending(o => o.WorkingGroupId);
						break;
                    default:
                        //added to avoid "The method 'Skip' is only supported for sorted input in LINQ to Entities. The method 'OrderBy' must be called before the method 'Skip'." error.
                        entities = entities.OrderBy(o => o.PerformerId);
                        break;
				}
			}
            else
            {
                //added to avoid "The method 'Skip' is only supported for sorted input in LINQ to Entities. The method 'OrderBy' must be called before the method 'Skip'." error.
                entities = entities.OrderBy(o => o.PerformerId);
            }

			entities = entities.Skip(from).Take(count);

			return entities
				.Select(c => new CustomerUserCase
				{
					Id  = c.Id,
					CaseNumber = c.CaseNumber,
					RegistrationDate = c.RegistrationDate,
					ChangedDate = c.ChangedDate,
					Subject = c.Subject,
					InitiatorName = c.InitiatorName,
					Description = c.Description,
					CustomerName = c.CustomerName,
					PriorityName = c.PriorityName,
					WorkingGroupName = c.WorkingGroupName,
					PerformerName = c.PerformerName,
					CaseIcon = c.CaseIcon,
					WatchDate = c.WatchDate,
					StateSecondaryName = c.StateSecondary.Name,
					Unread = c.Unread,
					DepartmentID = c.DepartmentID,
                    DepartmentName = c.DepartmentName,
					IncludeInCaseStatistics = c.IncludeInCaseStatistics,
					CustomerID = c.CustomerId,
					SolutionTime = c.SolutionTime,
					ExternalTime = c.ExternalTime
				});
		}

		public IList<int> GetCasesIdsByType(int caseTypeId)
        {
            return DataContext.Cases
                              .Where(c => c.CaseType_Id == caseTypeId)
                              .Select(o => o.Id).ToList();
        }

        public IList<Case> GetProblemCases(int problemId)
        {
            return DataContext.Cases.Where(x => x.Problem_Id == problemId).ToList();
        }

        public void MarkCaseAsRead(int id)
        {
            SetCaseUnreadFlag(id);
        }

        private void SetCaseUnreadFlag(int id, bool unread = false)
        {
            var @case = GetById(id);
            @case.Unread = unread ? 1 : 0;
            Update(@case);
            
            Commit();
        }

        public Case GetCaseIncluding(int id)
        {
            return DataContext.Cases
                .Include(x => x.Department)
                .Include(x => x.Workinggroup)
                .Include(x => x.ProductArea)
                .FirstOrDefault(x => x.Id == id);
        }
        
        public CaseModel GetCase(int id)
        {
            var caseEntity =  DataContext.Cases.Find(id);
            return _caseToBusinessModelMapper.Map(caseEntity);
        }

        public int LookupLanguage(int custid, string notid, int regid, int depid, string notifierid)
        {
            string sql = string.Empty;
            int langid = 0;
            bool match = false;

            //Initiation
           // if (notid != string.Empty && notifierid != string.Empty)
            if (notifierid != string.Empty)
            {

                var not = this.DataContext.ComputerUsers.FirstOrDefault(c=> c.UserId == notifierid && c.Customer_Id == custid);
                // var not = this.DataContext.ComputerUsers.FirstOrDefault(c => c.UserCode == notid && c.UserId == notifierid && c.Customer_Id == custid);
                // var not = this.DataContext.ComputerUsers.Single(c=> c.UserId == notifierid);
                if (not != null)
                {
                    if (not.LanguageId > 0)
                    {
                        langid = Convert.ToInt32(not.LanguageId);
                        match = true;
                    }

                }
            }

            //Department
            if (match == false)
            {
                if (depid > 0)
                {
                    var dep = this.DataContext.Departments.Single(c => c.Id == depid);
                    if (dep != null)
                    {
                        if (dep.LanguageId > 0)
                        {
                            langid = Convert.ToInt32(dep.LanguageId);
                            match = true;
                        }

                    }


                }
            }

            //Region
            if (match == false)
            {
                if (regid > 0)
                {
                    var reg = this.DataContext.Regions.Single(c => c.Id == regid);
                    if (reg != null)
                    {
                        if (reg.LanguageId > 0)
                        {
                            langid = Convert.ToInt32(reg.LanguageId);
                            match = true;
                        }

                    }


                }
            }

            //Customer
            if (match == false)
            {
                if (custid > 0)
                {
                    var cust = this.DataContext.Customers.Single(c => c.Id == custid);
                    if (cust != null)
                    {
                        if (cust.Language_Id > 0)
                        {
                            langid = Convert.ToInt32(cust.Language_Id);
                            match = true;
                        }

                    }


                }
            }
            return langid;
        }

        public List<ChildCaseOverview> GetChildCasesFor(int parentCaseId)
        {
            var query =
                from childCase in DataContext.Cases
                join rel in DataContext.ParentChildRelations on childCase.Id equals rel.DescendantId
                join perf in DataContext.Users on childCase.Performer_User_Id equals perf.Id into perfGroup
                from perf in perfGroup.DefaultIfEmpty()
                join subState in DataContext.StateSecondaries on childCase.StateSecondary_Id equals subState.Id into subStatesGroup
                from subState in subStatesGroup.DefaultIfEmpty()
                where rel.AncestorId == parentCaseId
                select new ChildCaseOverview
                {
                    Id = childCase.Id,
                    ParentId = rel.AncestorId,
                    Indepandent = rel.Independent,
                    CaseNoDecimal = childCase.CaseNumber,
                    Subject = childCase.Caption,
                    CasePerformer = new UserNamesStruct
                    {
                        FirstName = perf.FirstName ?? "", //keep for linq to sql correct translation 
                        LastName = perf.SurName ?? "",
                    },
                    SubStatus = subState.Name ?? "",
                    Priority = childCase.Priority.Name ?? "",
                    CaseType = childCase.CaseType.Name ?? "",
                    IsRequriedToApprive = childCase.CaseType.RequireApproving == 1, 
                    RegistrationDate = childCase.RegTime,
                    ClosingDate = childCase.FinishingDate,
                    ApprovedDate = childCase.ApprovedDate
                };

            return query.AsNoTracking().ToList();
        }

        public List<MergedChildOverview> GetMergedCasesFor(int parentCaseId)
        {
            var query =
                from childCase in DataContext.Cases
                join rel in DataContext.MergedCases on childCase.Id equals rel.MergedChildId
                join perf in DataContext.Users on childCase.Performer_User_Id equals perf.Id into perfGroup
                from perf in perfGroup.DefaultIfEmpty()
                join subState in DataContext.StateSecondaries on childCase.StateSecondary_Id equals subState.Id into subStatesGroup
                from subState in subStatesGroup.DefaultIfEmpty()
                where rel.MergedParentId == parentCaseId
                select new MergedChildOverview
                {
                    Id = childCase.Id,
                    ParentId = rel.MergedParentId,
                    CaseNoDecimal = childCase.CaseNumber,
                    Subject = childCase.Caption,
                    CasePerformer = new UserNamesStruct
                    {
                        FirstName = perf.FirstName ?? "", //keep for linq to sql correct translation 
                        LastName = perf.SurName ?? "",
                    },
                    SubStatus = subState.Name ?? "",
                    Priority = childCase.Priority.Name ?? "",
                    CaseType = childCase.CaseType.Name ?? "",
                    IsRequriedToApprive = childCase.CaseType.RequireApproving == 1,
                    RegistrationDate = childCase.RegTime,
                    ClosingDate = childCase.FinishingDate,
                    ApprovedDate = childCase.ApprovedDate
                };

            return query.AsNoTracking().ToList();
        }

        public ParentCaseInfo GetParentInfo(int childCaseId)
        {
            var query =
                from parentCase in DataContext.Cases
                join rel in DataContext.ParentChildRelations on parentCase.Id equals rel.AncestorId
                join perf in DataContext.Users on parentCase.Performer_User_Id equals perf.Id into perfGroup
                from perf in perfGroup.DefaultIfEmpty()
                where rel.DescendantId == childCaseId
                select new ParentCaseInfo
                {
                    ParentId = rel.AncestorId,
                    CaseNumber = (int)parentCase.CaseNumber,
                    IsChildIndependent = rel.Independent,
                    CaseAdministrator = new UserNamesStruct
                    {
                        FirstName = perf.FirstName ?? "", //keep "" for linq correct translation
                        LastName = perf.SurName ?? "",
                    },
                    FinishingDate = parentCase.FinishingDate
                };

            return query.AsNoTracking().FirstOrDefault();
        }

        public MergedParentInfo GetMergedParentInfo(int childCaseId)
        {
            var query =
                from parentCase in DataContext.Cases
                join rel in DataContext.MergedCases on parentCase.Id equals rel.MergedParentId
                join perf in DataContext.Users on parentCase.Performer_User_Id equals perf.Id into perfGroup
                from perf in perfGroup.DefaultIfEmpty()
                where rel.MergedChildId == childCaseId
                select new MergedParentInfo
                {
                    ParentId = rel.MergedParentId,
                    CaseNumber = (int)parentCase.CaseNumber,
                    CaseAdministrator = new UserNamesStruct
                    {
                        FirstName = perf.FirstName ?? "", //keep "" for linq correct translation
                        LastName = perf.SurName ?? "",
                    },
                    FinishingDate = parentCase.FinishingDate
                };

            return query.AsNoTracking().FirstOrDefault();
        }

        public class CaseCustomer
		{
			public Case Case { get; set; }
			public Customer Customer { get; set; }
		}


        public List<Case> GetTop100CasesToTest()
        {
            return DataContext.Cases.Take(100).ToList();            
        }

        public Case GetCaseQuickOpen(UserOverview currentUser, Expression<Func<Case, bool>> casePermissionFilter, string searchFor)
        {

            IQueryable<Case> query;

            query = from _case in DataContext.Set<Case>()
                    from user in _case.Customer.Users
                    where (_case.CaseNumber.ToString() == searchFor.Replace("#", "")) && user.Id == currentUser.Id
                    orderby _case.RegTime descending
                    select _case;

            if (casePermissionFilter != null)
            {
                query = query.Where(casePermissionFilter);
            }

            if (query.Any())
                return query.FirstOrDefault();

            return null;
        }

        public int GetCaseCustomerId(int caseId)
        {
            return Table.Where(x => x.Id == caseId).Select(x => x.Customer_Id).First();
        }
    }
}