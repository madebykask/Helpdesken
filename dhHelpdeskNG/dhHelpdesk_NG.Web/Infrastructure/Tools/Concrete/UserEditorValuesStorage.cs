namespace DH.Helpdesk.Web.Infrastructure.Tools.Concrete
{
    using System.Collections.Generic;

    public sealed class UserEditorValuesStorage : IUserEditorValuesStorage
    {
        public UserEditorValuesStorage(string topic)
        {
            this.topic = topic;
        }

        private readonly string topic;

        public void AddDeletedFileName(string fileName, int objectId, params string[] subtopics)
        {
            var key = this.ComposeDeletedFileNamesKey(objectId, subtopics);

            if (!SessionFacade.ContainsCustomKey(key))
            {
                var deletedFileNames = new List<string> { fileName };
                SessionFacade.SaveCustomValue(key, deletedFileNames);
            }
            else
            {
                var deletedFileNames = SessionFacade.GetCustomValue<List<string>>(key);
                deletedFileNames.Add(fileName);
                SessionFacade.SaveCustomValue(key, deletedFileNames);
            }
        }

        public List<string> GetDeletedFileNames(int objectId, params string[] subtopics)
        {
            var key = this.ComposeDeletedFileNamesKey(objectId, subtopics);

            return SessionFacade.ContainsCustomKey(key)
                ? SessionFacade.GetCustomValue<List<string>>(key)
                : new List<string>(0);
        }

        public void ClearDeletedFileNames(int objectId, params string[] subtopics)
        {
            var key = this.ComposeDeletedFileNamesKey(objectId, subtopics);
            SessionFacade.DeleteCustomValue(key);
        }

        public void AddDeletedItemId(string key, int id)
        {
            var composedKey = this.ComposeDeletedItemIdsKey(key);

            if (!SessionFacade.ContainsCustomKey(composedKey))
            {
                var deletedItemIds = new List<int> { id };
                SessionFacade.SaveCustomValue(composedKey, deletedItemIds);
            }
            else
            {
                var deletedItemIds = SessionFacade.GetCustomValue<List<int>>(composedKey);
                deletedItemIds.Add(id);
                SessionFacade.SaveCustomValue(composedKey, deletedItemIds);
            }
        }

        public List<int> GetDeletedItemIds(string key)
        {
            var composedKey = this.ComposeDeletedItemIdsKey(key);
            
            return SessionFacade.ContainsCustomKey(composedKey)
                ? SessionFacade.GetCustomValue<List<int>>(composedKey)
                : new List<int>(0);
        }

        public void ClearDeletedItemIds(string key)
        {
            var composedKey = this.ComposeDeletedItemIdsKey(key);
            SessionFacade.DeleteCustomValue(composedKey);
        }

        private string ComposeDeletedFileNamesKey(int objectId, params string[] subtopics)
        {
            return string.Join(".", this.topic, objectId, subtopics, "DeletedFileNames");
        }

        private string ComposeDeletedItemIdsKey(string key)
        {
            return string.Join(".", this.topic, "DeletedItemIds", key);
        }
    }
}