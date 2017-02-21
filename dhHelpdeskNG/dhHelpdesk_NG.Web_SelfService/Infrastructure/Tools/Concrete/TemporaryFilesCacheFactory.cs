namespace DH.Helpdesk.SelfService.Infrastructure.Tools.Concrete
{
    public sealed class TemporaryFilesCacheFactory : ITemporaryFilesCacheFactory
    {
        public ITemporaryFilesCache CreateForModule(string topic)
        {
            return new TemporaryFilesCache(topic);
        }
    }
}