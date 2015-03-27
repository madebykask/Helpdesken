namespace DH.Helpdesk.BusinessData.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class CaseOverviewGridColumnSetting
    {
        #region Static Fields

        private static IEnumerable<CaseOverviewGridColumnSetting> allAvailableInSystem;

        #endregion

        /// <summary>
        /// List of fields that we use in case overview tabel but they does not exists in case
        /// </summary>
        public static readonly HashSet<string> VirtualColumns = new HashSet<string> { "_temporary_.LeadTime", "tblProblem.ResponsibleUser_Id" };

        /// <summary>
        /// List of case names that we can not use in case overview grid
        /// </summary>
        public static readonly HashSet<string> NotAvailableField = new HashSet<string> { "Filename", "text_internal", "tblLog.Text_External", "tblLog.Text_Internal", "tblLog.Charge", "tblLog.Filename" };

        #region Public Properties

        public string Name { get; set; }

        public int Order { get; set; }

        public string Style { get; set; }

        #endregion

        #region Public Methods and Operators

        public static IEnumerable<CaseOverviewGridColumnSetting> GetDefaulVirtualFields()
        {
            return VirtualColumns.Select(it => new CaseOverviewGridColumnSetting() { Name = it });
        }
        
        public static bool IsVirtualField(string fieldName)
        {
            return VirtualColumns.Contains(fieldName);
        }

        #endregion
    }
}