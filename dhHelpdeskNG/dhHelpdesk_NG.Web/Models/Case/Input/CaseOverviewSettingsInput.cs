namespace DH.Helpdesk.Web.Models.Case.Input
{
    using System.Collections.Generic;

    public class ColumnSettings
    {
        public string ColumnName { get; set; }

        public string Style { get; set; }
    }

    public class CaseOverviewSettingsInput
    {
        public string SelectedFontStyle { get; set; }

        public ICollection<ColumnSettings> FieldStyle { get; set; }
    }
}