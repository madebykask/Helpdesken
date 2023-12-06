namespace DH.Helpdesk.Dal.Infrastructure
{
	using DH.Helpdesk.Dal.DbContext;
	using System.Data.SqlClient;

	public class DatabaseFactory : Disposable, IDatabaseFactory
    {
		private string connectionString = null;
		private HelpdeskDbContext dataContext;

		public DatabaseFactory()
		{

        }

		public DatabaseFactory(string connectionString)
		{
			this.connectionString = connectionString;
		}

        public HelpdeskDbContext Get()
        {
            this.dataContext = this.dataContext ?? (connectionString == null ? new HelpdeskSqlServerDbContext() : 
				new HelpdeskSqlServerDbContext(new SqlConnection(connectionString)));

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
