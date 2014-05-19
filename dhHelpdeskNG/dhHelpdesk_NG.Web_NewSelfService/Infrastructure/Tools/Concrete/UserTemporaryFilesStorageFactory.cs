namespace DH.Helpdesk.NewSelfService.Infrastructure.Tools.Concrete
{
    public sealed class UserTemporaryFilesStorageFactory : IUserTemporaryFilesStorageFactory
    {
        public IUserTemporaryFilesStorage Create(string topic)
        {
            return new UserTemporaryFilesStorage(topic);
        }
    }
}