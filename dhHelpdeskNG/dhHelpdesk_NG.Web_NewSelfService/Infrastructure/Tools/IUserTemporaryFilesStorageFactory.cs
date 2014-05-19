namespace DH.Helpdesk.NewSelfService.Infrastructure.Tools
{
    public interface IUserTemporaryFilesStorageFactory
    {
        IUserTemporaryFilesStorage Create(string topic);
    }
}