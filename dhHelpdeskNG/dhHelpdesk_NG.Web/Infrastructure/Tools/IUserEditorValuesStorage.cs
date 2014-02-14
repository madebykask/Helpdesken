namespace DH.Helpdesk.Web.Infrastructure.Tools
{
    using System.Collections.Generic;

    public interface IUserEditorValuesStorage
    {
        void AddDeletedFileName(string fileName, int objectId, params string[] subtopics);

        List<string> GetDeletedFileNames(int objectId, params string[] subtopics);

        void ClearDeletedFileNames(int objectId, params string[] subtopics);
        
        void ClearDeletedFileNames(int objectId);

        void AddDeletedItemId(int objectId, string key, int id);

        List<int> GetDeletedItemIds(int id, string key);

        void ClearDeletedItemIds(int id, string key);

        void ClearDeletedItemIds(string id, string key);
    }
}