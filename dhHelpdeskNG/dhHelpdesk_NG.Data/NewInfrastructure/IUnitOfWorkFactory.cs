namespace DH.Helpdesk.Dal.NewInfrastructure
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();

        IUnitOfWork Create(int timeout);

        IUnitOfWork CreateWithDisabledLazyLoading();
    }
}