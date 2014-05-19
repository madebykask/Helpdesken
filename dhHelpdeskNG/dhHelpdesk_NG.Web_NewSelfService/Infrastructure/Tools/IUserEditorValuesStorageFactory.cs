namespace DH.Helpdesk.NewSelfService.Infrastructure.Tools
{
    public interface IUserEditorValuesStorageFactory
    {
        IUserEditorValuesStorage Create(string topic);
    }
}