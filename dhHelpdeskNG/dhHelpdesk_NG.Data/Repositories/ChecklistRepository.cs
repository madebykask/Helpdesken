namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region CHECKLIST

    public interface IChecklistRepository : IRepository<Checklist>
    {
    }

    public class ChecklistRepository : RepositoryBase<Checklist>, IChecklistRepository
    {
        public ChecklistRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region CHECKLISTACTION

    public interface IChecklistActionRepository : IRepository<ChecklistAction>
    {
    }

    public class ChecklistActionRepository : RepositoryBase<ChecklistAction>, IChecklistActionRepository
    {
        public ChecklistActionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region CHECKLISTROW

    public interface IChecklistRowRepository : IRepository<ChecklistRow>
    {
    }

    public class ChecklistRowRepository : RepositoryBase<ChecklistRow>, IChecklistRowRepository
    {
        public ChecklistRowRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region CHECKLISTSERVICE

    public interface IChecklistServiceRepository : IRepository<ChecklistService>
    {
    }

    public class ChecklistServiceRepository : RepositoryBase<ChecklistService>, IChecklistServiceRepository
    {
        public ChecklistServiceRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
