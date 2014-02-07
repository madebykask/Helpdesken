namespace DH.Helpdesk.Web.Infrastructure.Tools
{
    public interface IUserEditorValuesStorageFactory
    {
        IUserEditorValuesStorage Create(string topic);
    }
}