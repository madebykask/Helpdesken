namespace DH.Helpdesk.BusinessData.Models
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Grid;

    public class CaseOverviewGridColumnSetting
    {
        #region Public Properties

        public string Name { get; set; }

        public int Order { get; set; }

        public string Style { get; set; }

        #endregion

        #region Public Methods and Operators

        public static IEnumerable<CaseOverviewGridColumnSetting> GetDefaulVirtualFields()
        {
            return GridColumnsDefinition.VirtualColumns.Select(it => new CaseOverviewGridColumnSetting() { Name = it });
        }
        
        public static bool IsVirtualField(string fieldName)
        {
            return GridColumnsDefinition.VirtualColumns.Contains(fieldName);
        }

        #endregion
    }
}