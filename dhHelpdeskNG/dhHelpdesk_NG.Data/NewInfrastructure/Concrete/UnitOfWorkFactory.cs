namespace DH.Helpdesk.Dal.NewInfrastructure.Concrete
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly ISessionFactory sessionFactory;

        public UnitOfWorkFactory(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork(this.sessionFactory.GetSession());
        }

        public IUnitOfWork CreateWithDisabledLazyLoading()
        {
            return new UnitOfWork(this.sessionFactory.GetSessionWithDisabledLazyLoading());
        }
    }
}
