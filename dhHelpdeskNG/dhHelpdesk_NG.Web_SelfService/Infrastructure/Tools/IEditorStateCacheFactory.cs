namespace DH.Helpdesk.SelfService.Infrastructure.Tools
{
    public interface IEditorStateCacheFactory
    {
        IEditorStateCache CreateForModule(string topic);
    }
}