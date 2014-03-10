namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region CASE

    public interface ICaseRepository : IRepository<Case>
    {
        Case GetCaseById(int id, bool markCaseAsRead = false);
        Case GetDetachedCaseById(int id);
        void SetNullProblemByProblemId(int problemId);
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

        public void SetNullProjectByProjectId(int projectId)
        {
            var cases = this.DataContext.Cases.Where(x => x.Project_Id == projectId).ToList();

            foreach (var item in cases)
            {
                item.Project_Id = null;
            }
        }

        private void MarkCaseAsRead(int id)
        {
            var cases = this.DataContext.Cases.Single(c => c.Id == id);
            cases.Unread = 0;
            this.Update(cases);
            this.Commit();
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