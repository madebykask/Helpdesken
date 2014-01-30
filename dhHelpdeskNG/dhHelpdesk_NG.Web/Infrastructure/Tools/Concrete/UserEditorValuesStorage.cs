namespace dhHelpdesk_NG.Web.Infrastructure.Tools.Concrete
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
            var key = ComposeKey(objectId, subtopics);

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
            var key = ComposeKey(objectId, subtopics);

            return SessionFacade.ContainsCustomKey(key)
                ? SessionFacade.GetCustomValue<List<string>>(key)
                : new List<string>(0);
        }

        public void ClearDeletedFileNames(int objectId, params string[] subtopics)
        {
            var key = ComposeKey(objectId, subtopics);
            SessionFacade.DeleteCustomValue(key);
        }

        private string ComposeKey(int objectId, params string[] subtopics)
        {
            return string.Join(".", topic, objectId, subtopics, "DeletedFileNames");
        }
    }
}