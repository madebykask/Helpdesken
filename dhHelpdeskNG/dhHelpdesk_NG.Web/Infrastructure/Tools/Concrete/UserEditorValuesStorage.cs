namespace dhHelpdesk_NG.Web.Infrastructure.Tools.Concrete
{
    using System.Collections.Generic;

    public sealed class UserEditorValuesStorage : IUserEditorValuesStorage
    {
        public void AddDeletedRecordId(string recordGroupName, int id, params string[] topics)
        {
            var key = ComposeKey(recordGroupName);

            if (SessionFacade.ContainsCustomKey(key, topics))
            {
                var deletedRecordIds = SessionFacade.GetCustomValue<List<int>>(key, topics);
                deletedRecordIds.Add(id);
                SessionFacade.SaveCustomValue(key, deletedRecordIds, topics);
            }
            else
            {
                var deletedRecordIds = new List<int> { id };
                SessionFacade.SaveCustomValue(key, deletedRecordIds, topics);
            }
        }

        public List<int> GetDeletedRecordIds(string recordGroupName, params string[] topics)
        {
            var key = ComposeKey(recordGroupName);

            return SessionFacade.ContainsCustomKey(key, topics)
                ? SessionFacade.GetCustomValue<List<int>>(key, topics)
                : new List<int>(0);
        }

        private static string ComposeKey(string recordGroupName)
        {
            return "Deleted\\" + recordGroupName;
        }
    }
}