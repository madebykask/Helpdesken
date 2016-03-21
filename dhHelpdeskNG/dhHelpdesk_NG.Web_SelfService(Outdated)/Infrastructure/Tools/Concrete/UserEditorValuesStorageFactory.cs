namespace DH.Helpdesk.SelfService.Infrastructure.Tools.Concrete
{
    public sealed class UserEditorValuesStorageFactory : IUserEditorValuesStorageFactory
    {
        public IUserEditorValuesStorage Create(string topic)
        {
            return new UserEditorValuesStorage(topic);
        }
    }
}