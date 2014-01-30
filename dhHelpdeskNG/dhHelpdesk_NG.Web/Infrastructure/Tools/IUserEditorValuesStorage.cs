namespace dhHelpdesk_NG.Web.Infrastructure.Tools
{
    using System.Collections.Generic;

    public interface IUserEditorValuesStorage
    {
        void AddDeletedFileName(string fileName, int objectId, params string[] subtopics);

        List<string> GetDeletedFileNames(int objectId, params string[] subtopics);

        void ClearDeletedFileNames(int objectId, params string[] subtopics);
    }
}