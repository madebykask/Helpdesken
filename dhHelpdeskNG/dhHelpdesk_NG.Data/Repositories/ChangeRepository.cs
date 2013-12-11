using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    #region CHANGE

    public interface IChangeRepository : IRepository<Change>
    {
        IList<Change> GetChanges(int customer);
    }

    public class ChangeRepository : RepositoryBase<Change>, IChangeRepository
    {
        public ChangeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IList<Change> GetChanges(int customer)
        {
            return (from w in this.DataContext.Set<Change>()
                    where w.Customer_Id == customer
                    select w).ToList();
        }
    }

    #endregion

    #region CHANGECATEGORY

    public interface IChangeCategoryRepository : IRepository<ChangeCategory>
    {
    }

    public class ChangeCategoryRepository : RepositoryBase<ChangeCategory>, IChangeCategoryRepository
    {
        public ChangeCategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region CHANGEEMAILLOG

    public interface IChangeEMailLogRepository : IRepository<ChangeEMailLog>
    {
    }

    public class ChangeEMailLogRepository : RepositoryBase<ChangeEMailLog>, IChangeEMailLogRepository
    {
        public ChangeEMailLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region CHANGEFIELDSETTIING

    public interface IChangeFieldSettingsRepository : IRepository<ChangeFieldSettings>
    {
    }

    public class ChangeFieldSettingsRepository : RepositoryBase<ChangeFieldSettings>, IChangeFieldSettingsRepository
    {
        public ChangeFieldSettingsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

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

    public interface IChangeGroupRepository : IRepository<ChangeGroup>
    {
    }

    public class ChangeGroupRepository : RepositoryBase<ChangeGroup>, IChangeGroupRepository
    {
        public ChangeGroupRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region CHANGEIMPLEMENTATION

    public interface IChangeImplementationStatusRepository : IRepository<ChangeImplementationStatus>
    {
    }

    public class ChangeImplementationStatusRepository : RepositoryBase<ChangeImplementationStatus>, IChangeImplementationStatusRepository
    {
        public ChangeImplementationStatusRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

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

    public interface IChangeObjectRepository : IRepository<ChangeObject>
    {
    }

    public class ChangeObjectRepository : RepositoryBase<ChangeObject>, IChangeObjectRepository
    {
        public ChangeObjectRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region CHANGEPRIORITY

    public interface IChangePriorityRepository : IRepository<ChangePriority>
    {
    }

    public class ChangePriorityRepository : RepositoryBase<ChangePriority>, IChangePriorityRepository
    {
        public ChangePriorityRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region CHANGESTATUS

    public interface IChangeStatusRepository : IRepository<ChangeStatus>
    {
    }

    public class ChangeStatusRepository : RepositoryBase<ChangeStatus>, IChangeStatusRepository
    {
        public ChangeStatusRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
