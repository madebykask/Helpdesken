namespace DH.Helpdesk.Mobile.Infrastructure.Tools.Concrete
{
    public sealed class EditorStateCacheFactory : IEditorStateCacheFactory
    {
        public IEditorStateCache CreateForModule(string topic)
        {
            return new EditorStateCache(topic);
        }
    }
}