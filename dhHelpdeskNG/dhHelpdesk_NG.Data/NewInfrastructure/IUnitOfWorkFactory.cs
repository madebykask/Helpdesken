namespace DH.Helpdesk.Dal.NewInfrastructure
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}