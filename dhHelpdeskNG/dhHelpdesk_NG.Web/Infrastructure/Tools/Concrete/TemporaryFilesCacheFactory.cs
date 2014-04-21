namespace DH.Helpdesk.Web.Infrastructure.Tools.Concrete
{
    public sealed class TemporaryFilesCacheFactory : ITemporaryFilesCacheFactory
    {
        public ITemporaryFilesCache CreateForModule(string topic)
        {
            return new TemporaryFilesCache(topic);
        }
    }
}