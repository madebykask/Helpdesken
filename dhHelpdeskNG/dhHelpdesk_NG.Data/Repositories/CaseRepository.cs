using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;

namespace dhHelpdesk_NG.Data.Repositories
{
    #region CASE

    public interface ICaseRepository : IRepository<Case>
    {
        Case GetCaseById(int id);
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
    }

    #endregion

    #region CASEFILE

    public interface ICaseFileRepository : IRepository<CaseFile>
    {
        //int GetNoCaseFiles(int caseid);
        IEnumerable<CaseFile> GetCaseFiles(int caseid);
    }

    public class CaseFileRepository : RepositoryBase<CaseFile>, ICaseFileRepository
    {
        public CaseFileRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        //public int GetNoCaseFiles(int caseid)
        //{
        //    var query = (from cfsl in this.DataContext.CaseFiles
        //                 where cfsl.Case_Id == caseid
        //                 orderby cfsl.Id
        //                 select cfsl.Id).Count();

        //    return query;
        //}

        public IEnumerable<CaseFile> GetCaseFiles(int caseid)
        {
            var query = (from cfsl in this.DataContext.CaseFiles
                         where cfsl.Case_Id == caseid
                         orderby cfsl.Id
                         select cfsl);

            return query.ToList();
        }
    }

    #endregion

    #region CASEHISTORY

    public interface ICaseHistoryRepository : IRepository<CaseHistory>
    {
    }

    public class CaseHistoryRepository : RepositoryBase<CaseHistory>, ICaseHistoryRepository
    {
        public CaseHistoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
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