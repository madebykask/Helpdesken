namespace DH.Helpdesk.Web.Infrastructure.Tools.Concrete
{
    public sealed class UserTemporaryFilesStorageFactory : IUserTemporaryFilesStorageFactory
    {
        public IUserTemporaryFilesStorage Create(string topic)
        {
            return new UserTemporaryFilesStorage(topic);
        }
    }
}