namespace DH.Helpdesk.BusinessData.Models.Grid
{
    using System.Collections.Generic;
    using System.Linq;

    public partial class GridColumnsDefinition
    {
        private static Dictionary<int, string> idNameMap;

        private static Dictionary<string, int> nameIdMap;

        public static int GetFieldId(string fieldName)
        {
            var map = GetNameIdMap();
            return map[fieldName];
        }

        public static string GetFieldName(int id)
        {
            var map = GetIdNameMap();
            return map[id];
        }


        public static bool IsAvailavbleToViewInCaseoverview(string fieldName)
        {
            return NotAvailableField.Contains(fieldName);
        }

        private static string[] collectAllField()
        {
            var res = caseOverviewColumns.ToList();
            res.AddRange(VirtualColumns);
            return res.ToArray();
        }

        private static Dictionary<int, string> GetIdNameMap()
        {
            if (idNameMap == null)
            {
                var id = 1;
                idNameMap = collectAllField().ToDictionary(it => id++, it => it);
            }

            return idNameMap;
        }

        private static Dictionary<string, int> GetNameIdMap()
        {
            if (nameIdMap == null)
            {
                var id = 1;
                nameIdMap = collectAllField().ToDictionary(it => it, it => id++);
            }

            return nameIdMap;
        }
    }
}