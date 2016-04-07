namespace DH.Helpdesk.SelfService.Infrastructure.Tools
{
    public interface IUserTemporaryFilesStorageFactory
    {
        IUserTemporaryFilesStorage Create(string topic);
    }
}