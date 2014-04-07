namespace DH.Helpdesk.Dal.Infrastructure.Context
{
    public interface IWorkContext
    {
        IUserContext User { get; }        
    }
}