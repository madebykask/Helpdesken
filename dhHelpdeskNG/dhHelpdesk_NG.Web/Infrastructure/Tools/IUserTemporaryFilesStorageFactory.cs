namespace dhHelpdesk_NG.Web.Infrastructure.Tools
{
    public interface IUserTemporaryFilesStorageFactory
    {
        IUserTemporaryFilesStorage Create(string topic);
    }
}