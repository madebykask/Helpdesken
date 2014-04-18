using DH.Helpdesk.BusinessData.Models.Case.Output;

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Faq.Output;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.Models.SelfService.Case;
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using System;

    #region CASE

    public interface ICaseRepository : IRepository<Case>
    {
        Case GetCaseById(int id, bool markCaseAsRead = false);
        Case GetCaseByGUID(Guid GUID);
        Case GetDetachedCaseById(int id);
        void SetNullProblemByProblemId(int problemId);
        void UpdateFinishedDate(int problemId, DateTime? time);
        void UpdateFollowUpDate(int caseId, DateTime? time);
        void Activate(int caseId);
        void MarkCaseAsUnread(int id);
        IEnumerable<CaseRelation> GetRelatedCases(int id, int customerId, string reportedBy, UserOverview user);
        IEnumerable<CaseOverview> GetCaseOverviews(int[] customers);

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
    }

    public class CaseRepository : RepositoryBase<Case>, ICaseRepository
    {
        public CaseRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public Case GetCaseById(int id, bool markCaseAsRead = false)
        {
            if (markCaseAsRead)
                MarkCaseAsRead(id);

            return (from w in this.DataContext.Set<Case>()
                    where w.Id == id
                    select w).FirstOrDefault();
        }

        public Case GetCaseByGUID(Guid GUID)
        {
            var caseEntity = DataContext.Cases.Where(c => c.CaseGUID == GUID )
                                              .FirstOrDefault();

            return caseEntity;
        }


        public Case GetDetachedCaseById(int id)
        {
            return (from w in this.DataContext.Set<Case>().AsNoTracking()
                    where w.Id == id
                    select w).FirstOrDefault();
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

        public void Activate(int caseId)
        {
            var cases = this.DataContext.Cases.Where(x => x.Id == caseId).FirstOrDefault();
            if (cases != null)
            {
                TimeSpan span = DateTime.UtcNow - cases.ChangeTime; 
                cases.FinishingDate = null;
                cases.ApprovedBy_User_Id = 0;
                cases.ApprovedDate = null;
                cases.ExternalTime = cases.ExternalTime + span.Minutes; 
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
            SetCaseUnreadFlag(id, 1);
        }

        private void MarkCaseAsRead(int id)
        {
            SetCaseUnreadFlag(id);
        }

        private void SetCaseUnreadFlag(int id, int unread = 0)
        {
            var cases = this.DataContext.Cases.Single(c => c.Id == id);
            cases.Unread = unread;
            this.Update(cases);
            this.Commit();
        }

        public IEnumerable<CaseRelation> GetRelatedCases(int id, int customerId, string reportedBy, UserOverview user)
        {
            var query = from c in this.DataContext.Cases
                        where c.Customer_Id == customerId 
                        && c.Id != id 
                        && c.ReportedBy.ToLower() == reportedBy.ToLower()
                        select c;
            //handläggare
            if (user.RestrictedCasePermission == 1 && user.UserGroupId == 2)                 
                query = query.Where(c => c.Performer_User_Id == user.Id || c.CaseResponsibleUser_Id == user.Id);
            
            //anmälare
            if (user.RestrictedCasePermission == 1 && user.UserGroupId == 1)
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
            return DataContext.Cases
                .Where(c => c.Id == caseId)
                .Select(c => new CaseOverview()
                {
                    CustomerId = c.Customer_Id,
                    Deleted = c.Deleted,
                    FinishingDate = c.FinishingDate,
                    StatusId = c.Status_Id,
                    UserId = c.User_Id,
                    CaseNumber = c.CaseNumber,
                    Department = c.Department,
                    PersonsCellphone = c.PersonsCellphone,
                    PersonsName = c.PersonsName,
                    PersonsPhone = c.PersonsPhone,
                    Region = c.Region,
                    RegionId = c.Region_Id,
                    RegistrationDate = c.RegTime,
                    ReportedBy = c.ReportedBy,
                    OuId = c.OU_Id,
                    Place = c.Place,
                    UserCode = c.UserCode,
                    PersonsEmail = c.PersonsEmail,
                    InventoryNumber = c.InventoryNumber,
                    InventoryLocation = c.InventoryLocation,
                    InventoryType = c.InventoryType,
                    IpAddress = c.IpAddress,
                    CaseTypeId = c.CaseType_Id,
                    SystemId = c.System_Id,
                    Urgency = c.Urgency,
                    ImpactId = c.Impact_Id,
                    SupplierId = c.Supplier_Id,
                    InvoiceNumber = c.InvoiceNumber,
                    ReferenceNumber = c.ReferenceNumber,
                    Caption = c.Caption,
                    Description = c.Description,
                    CategoryId = c.Category_Id,
                    Miscellaneous = c.Miscellaneous,
                    ProductAreaId = c.ProductArea_Id,
                    ContactBeforeAction = c.ContactBeforeAction,
                    Sms = c.SMS,
                    AgreedDate = c.AgreedDate,
                    Available = c.Available,
                    Cost = c.Cost,
                    OtherCost = c.OtherCost,
                    Currency = c.Currency,
                    WorkingGroupId = c.WorkingGroup_Id,
                    WorkingGroup = c.Workinggroup,
                    CaseResponsibleUserId = c.CaseResponsibleUser_Id,
                    PerformerUserId = c.Performer_User_Id,
                    Priority = c.Priority,
                    StateSecondaryId = c.StateSecondary_Id,
                    StateSecondary = c.StateSecondary,
                    ProjectId = c.Project_Id,
                    ProblemId = c.Problem_Id,
                    ChangeId = c.Change_Id,
                    WatchDate = c.WatchDate,
                    Verified = c.Verified,
                    VerifiedDescription = c.VerifiedDescription,
                    SolutionRate = c.SolutionRate,
                    CaseHistories = c.CaseHistories,
                    FinishingDescription = c.FinishingDescription,
                    CausingTypeId = c.CausingTypeId
                })
                .FirstOrDefault();
        }
    }

    #endregion

    #region CASEFILE

    public interface ICaseFileRepository : IRepository<CaseFile>
    {
        List<string> FindFileNamesByCaseId(int caseid);
        List<CaseFile> GetCaseFilesByCaseId(int caseid);
        byte[] GetFileContentByIdAndFileName(int caseId, string fileName);
        bool FileExists(int caseId, string fileName);
        void DeleteByCaseIdAndFileName(int caseId, string fileName);
    }

    public class CaseFileRepository : RepositoryBase<CaseFile>, ICaseFileRepository
    {
        private readonly IFilesStorage _filesStorage;

        public CaseFileRepository(IDatabaseFactory databaseFactory, IFilesStorage fileStorage)
            : base(databaseFactory)
        {
            this._filesStorage = fileStorage;
        }

        public byte[] GetFileContentByIdAndFileName(int caseId, string fileName)
        {
            return this._filesStorage.GetFileContent(TopicName.Cases, caseId, fileName);
        }

        public bool FileExists(int caseId, string fileName)
        {
            return this.DataContext.CaseFiles.Any(f => f.Case_Id == caseId && f.FileName == fileName.Trim());
        }

        public void DeleteByCaseIdAndFileName(int caseId, string fileName)
        {
            if (FileExists(caseId, fileName))
            {
                var cf = this.DataContext.CaseFiles.Single(f => f.Case_Id == caseId && f.FileName == fileName.Trim());
                this.DataContext.CaseFiles.Remove(cf);
                this.Commit();
            }
            this._filesStorage.DeleteFile(TopicName.Cases, caseId, fileName);
        }

        public List<string> FindFileNamesByCaseId(int caseId)
        {
            return this.DataContext.CaseFiles.Where(f => f.Case_Id == caseId).Select(f => f.FileName).ToList();
        }

        public List<CaseFile> GetCaseFilesByCaseId(int caseId)
        {
            return (from f in this.DataContext.CaseFiles
                    where f.Case_Id == caseId
                    select f).ToList();
        }
    }

    #endregion

    #region CASEINVOICEROW

    public interface ICaseInvoiceRowRepository : IRepository<CaseInvoiceRow>
    {
    }

    public class CaseInvoiceRowRepository : RepositoryBase<CaseInvoiceRow>, ICaseInvoiceRowRepository
    {
        public CaseInvoiceRowRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region CASEQUESTIONCATEGORY

    public interface ICaseQuestionCategoryRepository : IRepository<CaseQuestionCategory>
    {
    }

    public class CaseQuestionCategoryRepository : RepositoryBase<CaseQuestionCategory>, ICaseQuestionCategoryRepository
    {
        public CaseQuestionCategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region CASEQUESTIONHEADER

    public interface ICaseQuestionHeaderRepository : IRepository<CaseQuestionHeader>
    {
    }

    public class CaseQuestionHeaderRepository : RepositoryBase<CaseQuestionHeader>, ICaseQuestionHeaderRepository
    {
        public CaseQuestionHeaderRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region CASEQUESTION

    public interface ICaseQuestionRepository : IRepository<CaseQuestion>
    {
    }

    public class CaseQuestionRepository : RepositoryBase<CaseQuestion>, ICaseQuestionRepository
    {
        public CaseQuestionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region CASESETTING

    public interface ICaseSettingRepository : IRepository<CaseSettings>
    {
        string SetListCaseName(int labelId);
        void UpdateCaseSetting(CaseSettings updatedCaseSetting);
    }

    public class CaseSettingRepository : RepositoryBase<CaseSettings>, ICaseSettingRepository
    {
        public CaseSettingRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public string SetListCaseName(int labelId)
        {
            var query = from cfs in this.DataContext.CaseFieldSettings
                        join cs in this.DataContext.CaseSettings on cfs.Name equals cs.Name
                        where cfs.Id == labelId
                        group cfs by new { cfs.Name } into g
                        select new CaseSettingList
                        {
                            Name = g.Key.Name
                        };

            return query.First().Name;
        }

        public void UpdateCaseSetting(CaseSettings updatedCaseSetting)
        {
            var caseSettingEntity = this.DataContext.CaseSettings.Find(updatedCaseSetting.Id);

            caseSettingEntity.Name = updatedCaseSetting.Name;
            caseSettingEntity.Line = updatedCaseSetting.Line;
            caseSettingEntity.MinWidth = updatedCaseSetting.MinWidth;
            caseSettingEntity.UserGroup = updatedCaseSetting.UserGroup;
            caseSettingEntity.ColOrder = updatedCaseSetting.ColOrder;

        }
    }

    #endregion
}