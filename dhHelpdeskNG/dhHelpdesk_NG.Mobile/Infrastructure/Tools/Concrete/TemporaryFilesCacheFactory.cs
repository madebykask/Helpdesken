namespace DH.Helpdesk.Mobile.Infrastructure.Tools.Concrete
{
    public sealed class TemporaryFilesCacheFactory : ITemporaryFilesCacheFactory
    {
        public ITemporaryFilesCache CreateForModule(string topic)
        {
            return new TemporaryFilesCache(topic);
        }
    }
}