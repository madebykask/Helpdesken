namespace DH.Helpdesk.Dal.NewInfrastructure.Concrete
{
    using DH.Helpdesk.Dal.DbContext;

    public class HelpdeskSessionFactory : ISessionFactory
    {
        public IDbContext GetSession()
        {
            return new HelpdeskSqlServerDbContext();
        }

        public IDbContext GetSession(int timeout)
        {
            return new HelpdeskSqlServerDbContext(timeout);
        }

        public IDbContext GetSessionWithDisabledLazyLoading()
        {
            var session = new HelpdeskSqlServerDbContext();
            session.Configuration.LazyLoadingEnabled = false;
            return session;
        }
    }
}