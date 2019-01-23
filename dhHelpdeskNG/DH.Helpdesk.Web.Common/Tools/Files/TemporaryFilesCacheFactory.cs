namespace DH.Helpdesk.Web.Common.Tools.Files
{
    public sealed class TemporaryFilesCacheFactory : ITemporaryFilesCacheFactory
    {
        public ITemporaryFilesCache CreateForModule(string topic)
        {
            return new TemporaryFilesCache(topic);
        }
    }
}