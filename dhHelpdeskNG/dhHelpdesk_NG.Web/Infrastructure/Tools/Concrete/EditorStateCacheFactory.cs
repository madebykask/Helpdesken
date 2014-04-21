namespace DH.Helpdesk.Web.Infrastructure.Tools.Concrete
{
    public sealed class EditorStateCacheFactory : IEditorStateCacheFactory
    {
        public IEditorStateCache CreateForModule(string topic)
        {
            return new EditorStateCache(topic);
        }
    }
}