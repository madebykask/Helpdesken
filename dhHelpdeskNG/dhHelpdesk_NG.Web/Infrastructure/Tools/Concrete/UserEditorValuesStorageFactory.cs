namespace dhHelpdesk_NG.Web.Infrastructure.Tools.Concrete
{
    public sealed class UserEditorValuesStorageFactory : IUserEditorValuesStorageFactory
    {
        public IUserEditorValuesStorage Create(string topic)
        {
            return new UserEditorValuesStorage(topic);
        }
    }
}