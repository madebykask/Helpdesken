namespace DH.Helpdesk.Web.Infrastructure.Tools
{
    public interface IUserTemporaryFilesStorageFactory
    {
        IUserTemporaryFilesStorage Create(string topic);
    }
}