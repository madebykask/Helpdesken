namespace DH.Helpdesk.Dal.Infrastructure
{
    using System;

    using DH.Helpdesk.Dal.DbContext;

    [Obsolete("Use transactions insted of this.")]
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory _databaseFactory;
        private HelpdeskDbContext _dataContext;

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            this._databaseFactory = databaseFactory;
        }

        protected HelpdeskDbContext DataContext
        {
            get { return this._dataContext ?? (this._dataContext = this._databaseFactory.Get()); }
        }

        public void Commit()
        {
            this.DataContext.Commit();
        }
    }
}
