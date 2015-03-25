namespace DH.Helpdesk.Dal.Infrastructure
{
    using DH.Helpdesk.Dal.DbContext;

    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private HelpdeskDbContext dataContext;

        public HelpdeskDbContext Get()
        {
            this.dataContext = this.dataContext ?? new HelpdeskSqlServerDbContext();

            return this.dataContext;
        }

        protected override void DisposeCore()
        {
            if (this.dataContext != null)
            {
                this.dataContext.Dispose();
            }
        }
    }
}
