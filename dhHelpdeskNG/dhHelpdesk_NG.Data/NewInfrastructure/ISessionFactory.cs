namespace DH.Helpdesk.Dal.NewInfrastructure
{
    public interface ISessionFactory
    {
        IDbContext GetSession();

        IDbContext GetSessionWithDisabledLazyLoading();
    }
}