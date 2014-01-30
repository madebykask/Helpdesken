namespace dhHelpdesk_NG.Web.Infrastructure.Tools
{
    public interface IUserEditorValuesStorageFactory
    {
        IUserEditorValuesStorage Create(string topic);
    }
}