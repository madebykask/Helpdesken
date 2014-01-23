using System.Collections.Generic;
using System.Linq;

using dhHelpdesk_NG.Data.Enums;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;

namespace dhHelpdesk_NG.Data.Repositories
{
    #region CASE

    public interface ICaseRepository : IRepository<Case>
    {
        Case GetCaseById(int id);

        void SetNullProblemByProblemId(int problemId);
    }

    public class CaseRepository : RepositoryBase<Case>, ICaseRepository
    {
        public CaseRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public Case GetCaseById(int id)
        {
            return (from w in this.DataContext.Set<Case>()
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
    }

    #endregion

    #region CASEFILE

    public interface ICaseFileRepository : IRepository<CaseFile>
    {
        //IEnumerable<CaseFile> GetCaseFiles(int caseid);
        List<string> FindFileNamesByCaseId(int caseid);
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
            _filesStorage = fileStorage;
        }

        public byte[] GetFileContentByIdAndFileName(int caseId, string fileName)
        {
            return _filesStorage.GetFileContent(Topic.Case, caseId, fileName);
        }

        public bool FileExists(int caseId, string fileName)
        {
            return this.DataContext.CaseFiles.Any(f => f.Case_Id == caseId && f.FileName == fileName);
        }

        public void DeleteByCaseIdAndFileName(int caseId, string fileName)
        {
            var cf = this.DataContext.CaseFiles.Single(f => f.Case_Id == caseId && f.FileName == fileName);
            this.DataContext.CaseFiles.Remove(cf);
            this.Commit();
            _filesStorage.DeleteFile(fileName, Topic.Case, caseId);
        }

        //public IEnumerable<CaseFile> GetCaseFiles(int caseid)
        //{
        //    var query = (from cfsl in this.DataContext.CaseFiles
        //                 where cfsl.Case_Id == caseid
        //                 orderby cfsl.FileName
        //                 select cfsl);

        //    return query.ToList();
        //}

        public List<string> FindFileNamesByCaseId(int caseId)
        {
            return this.DataContext.CaseFiles.Where(f => f.Case_Id == caseId).Select(f => f.FileName).ToList();
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
    }

    #endregion
}