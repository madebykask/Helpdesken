namespace DH.Helpdesk.Web.Infrastructure.Tools
{
    using System.Collections.Generic;

    public interface IUserEditorValuesStorage
    {
        void AddDeletedFile(string fileName, int objectId, params string[] subtopics);

        List<string> GetDeletedFileNames(int objectId, params string[] subtopics);

        void ClearDeletedFiles(int objectId);

        void AddDeletedItem(int itemId, string key, int objectId);

        List<int> GetDeletedItemIds(int objectId, string key);

        void ClearDeletedItemIds(int objectId, string key);
    }
}