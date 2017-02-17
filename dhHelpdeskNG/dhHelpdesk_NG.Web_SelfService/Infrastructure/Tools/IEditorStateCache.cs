using System.Collections.Generic;

namespace DH.Helpdesk.SelfService.Infrastructure.Tools
{
    public interface IEditorStateCache
    {
        void AddDeletedFile(string fileName, int objectId, params string[] subtopics);

        List<string> FindDeletedFileNames(int objectId, params string[] subtopics);

        void ClearObjectDeletedFiles(int objectId);

        void AddDeletedItem(int itemId, string key, int objectId);

        List<int> GetDeletedItemIds(int objectId, string key);

        void ClearObjectDeletedItems(int objectId, string key);
    }
}