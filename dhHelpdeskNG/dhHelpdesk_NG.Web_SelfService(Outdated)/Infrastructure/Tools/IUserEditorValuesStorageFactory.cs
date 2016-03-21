namespace DH.Helpdesk.SelfService.Infrastructure.Tools
{
    public interface IUserEditorValuesStorageFactory
    {
        IUserEditorValuesStorage Create(string topic);
    }
}