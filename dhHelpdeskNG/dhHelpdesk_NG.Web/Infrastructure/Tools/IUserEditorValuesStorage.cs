namespace DH.Helpdesk.Web.Infrastructure.Tools
{
    using System.Collections.Generic;

    public interface IUserEditorValuesStorage
    {
        void AddDeletedFileName(string fileName, int objectId, params string[] subtopics);

        List<string> GetDeletedFileNames(int objectId, params string[] subtopics);

        void ClearDeletedFileNames(int objectId);

        void AddDeletedItemId(int itemId, string key, int objectId);

        List<int> GetDeletedItemIds(int objectId, string key);

        void ClearDeletedItemIds(int objectId, string key);
    }
}