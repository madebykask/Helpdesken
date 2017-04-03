using System.Collections.Generic;
using System.Linq;

namespace DH.Helpdesk.SelfService.Infrastructure.Tools.Concrete
{
    public sealed class EditorStateCache : IEditorStateCache
    {
        private readonly string topic;

        public EditorStateCache(string topic)
        {
            this.topic = topic;
        }

        public void AddDeletedFile(string fileName, int objectId, params string[] subtopics)
        {
            var key = this.ComposeDeletedFilesKey(objectId);

            var deletedItemSubtopic = this.ComposeDeletedFileSubtopic(subtopics);
            var deletedFile = new DeletedFile(fileName, deletedItemSubtopic);

            if (!SessionFacade.ContainsCustomKey(key))
            {
                var deletedFiles = new List<DeletedFile> { deletedFile };
                SessionFacade.SaveCustomValue(key, deletedFiles);
            }
            else
            {
                var deletedFiles = SessionFacade.GetCustomValue<List<DeletedFile>>(key);
                deletedFiles.Add(deletedFile);
                SessionFacade.SaveCustomValue(key, deletedFiles);
            }
        }

        public List<string> FindDeletedFileNames(int objectId, params string[] subtopics)
        {
            var key = this.ComposeDeletedFilesKey(objectId);

            if (SessionFacade.ContainsCustomKey(key))
            {
                var deletedFiles = SessionFacade.GetCustomValue<List<DeletedFile>>(key);
                return deletedFiles.Select(f => f.Name).ToList();
            }

            return new List<string>(0);
        }

        public void ClearObjectDeletedFiles(int objectId)
        {
            var key = this.ComposeDeletedFilesKey(objectId);
            SessionFacade.DeleteCustomValue(key);
        }

        public void AddDeletedItem(int itemId, string key, int objectId)
        {
            var composedKey = this.ComposeDeletedItemsKey(objectId);
            var deletedItem = new DeletedItem(itemId, key);

            if (!SessionFacade.ContainsCustomKey(composedKey))
            {
                var deletedItems = new List<DeletedItem> { deletedItem };
                SessionFacade.SaveCustomValue(composedKey, deletedItems);
            }
            else
            {
                var deletedItems = SessionFacade.GetCustomValue<List<DeletedItem>>(composedKey);
                deletedItems.Add(deletedItem);
                SessionFacade.SaveCustomValue(composedKey, deletedItems);
            }
        }

        public List<int> GetDeletedItemIds(int objectId, string key)
        {
            var composedKey = this.ComposeDeletedItemsKey(objectId);

            if (SessionFacade.ContainsCustomKey(composedKey))
            {
                var deletedItems = SessionFacade.GetCustomValue<List<DeletedItem>>(composedKey);
                return deletedItems.Select(i => i.Id).ToList();
            }

            return new List<int>(0);
        }

        public void ClearObjectDeletedItems(int objectId, string key)
        {
            var composedKey = this.ComposeDeletedItemsKey(objectId);
            SessionFacade.DeleteCustomValue(composedKey);
        }

        private string ComposeDeletedFilesKey(int objectId)
        {
            return string.Join(".", this.topic, objectId, "DeletedFiles");
        }

        private string ComposeDeletedItemsKey(int objectId)
        {
            return string.Join(".", this.topic, objectId, "DeletedItems");
        }

        private string ComposeDeletedFileSubtopic(params string[] subtopics)
        {
            return string.Join(".", this.topic, subtopics);
        }
    }
}