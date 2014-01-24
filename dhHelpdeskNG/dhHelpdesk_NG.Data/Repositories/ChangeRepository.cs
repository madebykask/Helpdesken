using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{

    #region CHANGECATEGORY

    #endregion

    #region CHANGEEMAILLOG

    #endregion

    #region CHANGEFILE

    public interface IChangeFileRepository : IRepository<ChangeFile>
    {
    }

    public class ChangeFileRepository : RepositoryBase<ChangeFile>, IChangeFileRepository
    {
        public ChangeFileRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region CHANGEGROUP

    #endregion

    #region CHANGEIMPLEMENTATION

    #endregion

    #region CHANGELOG

    public interface IChangeLogRepository : IRepository<ChangeLog>
    {
    }

    public class ChangeLogRepository : RepositoryBase<ChangeLog>, IChangeLogRepository
    {
        public ChangeLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region CHANGEOBJECT

    #endregion

    #region CHANGEPRIORITY

    #endregion

    #region CHANGESTATUS

    #endregion
}
