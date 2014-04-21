namespace DH.Helpdesk.Web.Infrastructure.Tools
{
    public interface IEditorStateCacheFactory
    {
        IEditorStateCache CreateForModule(string topic);
    }
}