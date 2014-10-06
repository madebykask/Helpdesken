namespace DH.Helpdesk.Mobile.Infrastructure.Tools
{
    public interface IEditorStateCacheFactory
    {
        IEditorStateCache CreateForModule(string topic);
    }
}