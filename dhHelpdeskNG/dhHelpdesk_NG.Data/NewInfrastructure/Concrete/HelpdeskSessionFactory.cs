namespace DH.Helpdesk.Dal.NewInfrastructure.Concrete
{
    using DH.Helpdesk.Dal.DbContext;

    public class HelpdeskSessionFactory : ISessionFactory
    {
        public IDbContext GetSession()
        {
            return new HelpdeskSqlServerDbContext();
        }
    }
}