namespace DH.Helpdesk.SelfService.Infrastructure.Tools
{
    public interface ITemporaryFilesCacheFactory
    {
        ITemporaryFilesCache CreateForModule(string topic);
    }
}