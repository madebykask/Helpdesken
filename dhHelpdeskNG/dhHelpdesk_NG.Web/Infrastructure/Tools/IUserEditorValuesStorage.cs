namespace dhHelpdesk_NG.Web.Infrastructure.Tools
{
    using System.Collections.Generic;

    public interface IUserEditorValuesStorage
    {
        void AddDeletedRecordId(string recordGroupName, int id, params string[] topics);

        List<int> GetDeletedRecordIds(string recordGroupName, params string[] topics);
    }
}