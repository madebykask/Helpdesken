namespace DH.Helpdesk.BusinessData.Models.Grid
{
    using System.Collections.Generic;
    using System.Linq;

    public partial class GridColumnsDefinition
    {
        private static Dictionary<int, string> _idNameMap;
        private static Dictionary<string, int> _nameIdMap;

        public static int GetFieldId(string fieldName)
        {
            var map = GetNameIdMap();
            return map[fieldName.ToLower()];
        }

        public static string GetFieldName(int id)
        {
            var map = GetIdNameMap();
            return map[id];
        }


        public static bool IsAvailavbleToViewInCaseoverview(string fieldName)
        {
            return !NotAvailableField.Contains(fieldName);
        }

        private static string[] CollectAllField()
        {
            var res = CaseOverviewColumns.ToList();
            res.AddRange(VirtualColumns);
            return res.ToArray();
        }

        private static Dictionary<int, string> GetIdNameMap()
        {
            if (_idNameMap == null)
            {
                var id = 1;
                _idNameMap = CollectAllField().Select(it => it.ToLower()).ToDictionary(it => id++, it => it);
            }

            return _idNameMap;
        }

        private static Dictionary<string, int> GetNameIdMap()
        {
            if (_nameIdMap == null)
            {
                var id = 1;
                _nameIdMap = CollectAllField().Select(it => it.ToLower()).ToDictionary(it => it, it => id++);
            }

            return _nameIdMap;
        }
    }
}