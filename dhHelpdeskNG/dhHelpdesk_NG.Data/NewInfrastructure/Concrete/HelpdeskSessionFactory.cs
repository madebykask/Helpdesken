namespace DH.Helpdesk.Dal.NewInfrastructure.Concrete
{
    using DH.Helpdesk.Dal.DbContext;
    using System.Data.SqlClient;

    public class HelpdeskSessionFactory : ISessionFactory
    {

        public string connectionString;
        private HelpdeskDbContext dataContext;
        private bool hasConnectionStringInConstructor = false;

        public HelpdeskSessionFactory(string connectionString)
        {
            hasConnectionStringInConstructor = true;
            this.connectionString = connectionString;

        }
        public HelpdeskSessionFactory()
        {

        }

        public IDbContext GetSession()
        {

            if(hasConnectionStringInConstructor)
            {
                var session = new HelpdeskSqlServerDbContext(new SqlConnection(connectionString));
                session.Configuration.LazyLoadingEnabled = false;
                return session;
            }
            else
            {
                return new HelpdeskSqlServerDbContext(); 
            }

        }

        public IDbContext GetSession(int timeout)
        {
            if (hasConnectionStringInConstructor)
            {
                var session = new HelpdeskSqlServerDbContext(new SqlConnection(connectionString));
                session.Configuration.LazyLoadingEnabled = false;
                return session;
            }
            else
            {
                return new HelpdeskSqlServerDbContext(timeout);
            }
        }

        public IDbContext GetSessionWithDisabledLazyLoading()
        {
            var session = new HelpdeskSqlServerDbContext();
            session.Configuration.LazyLoadingEnabled = false;
            return session;
        }
    }
}