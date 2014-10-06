namespace DH.Helpdesk.Web.Infrastructure.Tools
{
    public interface ITemporaryFilesCacheFactory
    {
        ITemporaryFilesCache CreateForModule(string topic);
    }
}