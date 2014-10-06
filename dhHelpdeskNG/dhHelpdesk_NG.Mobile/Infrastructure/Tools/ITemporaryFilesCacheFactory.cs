namespace DH.Helpdesk.Mobile.Infrastructure.Tools
{
    public interface ITemporaryFilesCacheFactory
    {
        ITemporaryFilesCache CreateForModule(string topic);
    }
}