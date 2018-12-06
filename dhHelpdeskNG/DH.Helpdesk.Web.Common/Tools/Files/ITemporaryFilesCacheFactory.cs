namespace DH.Helpdesk.Web.Common.Tools.Files
{
    public interface ITemporaryFilesCacheFactory
    {
        ITemporaryFilesCache CreateForModule(string topic);
    }
}